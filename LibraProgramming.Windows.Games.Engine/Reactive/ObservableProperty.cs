using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Threading;

namespace LibraProgramming.Windows.Games.Engine.Reactive
{
    [Serializable]
    public class ObservableProperty<TValue> : IObservableProperty<TValue>, IDisposable
    {
        [NonSerialized] private bool canPublishValueOnSubscribe;
        [NonSerialized] private bool disposed;
        [NonSerialized] private Subject<TValue> subject;
        [NonSerialized] private IDisposable subscription;
        [NonSerialized] private Exception lastException;
        private TValue value;

        TValue IReadOnlyObservableProperty<TValue>.Value => value;

        public TValue Value
        {
            get => value;
            set
            {
                if (false == canPublishValueOnSubscribe)
                {
                    canPublishValueOnSubscribe = true;

                    SetValueAndForceNotify(value);

                    return;
                }

                if (false == EqualityComparer.Equals(this.value, value))
                {
                    SetValueAndForceNotify(value);
                }
            }
        }

        public bool HasValue => canPublishValueOnSubscribe;

        protected IEqualityComparer<TValue> EqualityComparer => EqualityComparer<TValue>.Default;

        public ObservableProperty(TValue value)
        {
            canPublishValueOnSubscribe = true;
            SetValue(value);
        }

        public ObservableProperty()
            : this(default(TValue))
        {
        }

        public ObservableProperty(IObservable<TValue> source)
        {
            subscription = source.Subscribe(new ObservablePropertyObserver(this));
            canPublishValueOnSubscribe = false;
        }

        public ObservableProperty(IObservable<TValue> source, TValue value)
        {
            canPublishValueOnSubscribe = false;
            Value = value;
            subscription = source.Subscribe(new ObservablePropertyObserver(this));
        }

        public IDisposable Subscribe(IObserver<TValue> observer)
        {
            if (null != lastException)
            {
                observer.OnError(lastException);
                return Disposable.Empty;
            }

            if (disposed)
            {
                observer.OnCompleted();
                return Disposable.Empty;
            }

            if (null == subject)
            {
                // Interlocked.CompareExchange is bit slower, guarantee threasafety is overkill.
                // Interlocked.CompareExchange(ref subject, new Subject<TValue>(), null);
                subject = new Subject<TValue>();
            }

            var publisher = subject;

            if (null != publisher)
            {
                var token = publisher.Subscribe(observer);

                if (canPublishValueOnSubscribe)
                {
                    observer.OnNext(value); // raise latest value on subscribe
                }

                return token;
            }

            observer.OnCompleted();

            return Disposable.Empty;
        }

        public bool IsRequiredSubscribeOnCurrentThread()
        {
            return false;
        }

        public void SetValueAndForceNotify(TValue value)
        {
            SetValue(value);

            if (disposed)
            {
                return;
            }

            subject?.OnNext(this.value);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void SetValueInternal(TValue value)
        {
            this.value = value;
        }

        protected virtual void Dispose(bool dispose)
        {
            if (disposed)
            {
                return;
            }

            try
            {
                if (dispose)
                {
                    var sc = subscription;

                    if (null != sc)
                    {
                        sc.Dispose();
                        subscription = null;
                    }

                    var publisher = subject;

                    if (null != publisher)
                    {
                        // when dispose, notify OnCompleted
                        try
                        {
                            publisher.OnCompleted();
                        }
                        finally
                        {
                            publisher.Dispose();
                            subject = null;
                        }
                    }
                }
            }
            finally
            {
                disposed = true;
            }
        }

        private void SetValue(TValue value)
        {
            SetValueInternal(value);
        }

        private class ObservablePropertyObserver : IObserver<TValue>
        {
            private readonly ObservableProperty<TValue> parent;
            private int isStopped = 0;

            public ObservablePropertyObserver(ObservableProperty<TValue> parent)
            {
                this.parent = parent;
            }

            public void OnNext(TValue value)
            {
                parent.Value = value;
            }

            public void OnError(Exception error)
            {
                if (1 == Interlocked.Increment(ref isStopped))
                {
                    parent.lastException = error;
                    parent.subject?.OnError(error);
                    parent.Dispose(); // complete subscription
                }
            }

            public void OnCompleted()
            {
                if (1 != Interlocked.Increment(ref isStopped))
                {
                    return;
                }

                // source was completed but can publish from .Value yet.
                var sc = parent.subscription;

                parent.subscription = null;

                sc?.Dispose();
            }
        }
    }
}