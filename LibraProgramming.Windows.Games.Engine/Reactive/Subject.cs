using System;
using System.Collections.Immutable;

namespace LibraProgramming.Windows.Games.Engine.Reactive
{
    /*public sealed class Subject<TSource> : ISubject<TSource>, IDisposable
    {
        private readonly object gate = new object();

        private bool stopped;
        private bool disposed;
        private IObserver<TSource> observer;
        private Exception lastError;

        public Subject()
        {
            observer = EmptyObserver<TSource>.Instance;
        }

        public void OnCompleted()
        {
            IObserver<TSource> current;

            lock (gate)
            {
                EnsureNotDisposed();

                if (stopped)
                {
                    return;
                }

                current = observer;
                observer = EmptyObserver<TSource>.Instance;
                stopped = true;
            }

            current.OnCompleted();
        }

        public void OnError(Exception error)
        {
            if (null == error)
            {
                throw new ArgumentNullException(nameof(error));
            }

            IObserver<TSource> current;

            lock (gate)
            {
                EnsureNotDisposed();

                if (stopped)
                {
                    return;
                }

                current = observer;
                observer = EmptyObserver<TSource>.Instance;
                stopped = true;
                lastError = error;
            }

            current.OnError(error);

        }

        public void OnNext(TSource value)
        {
            observer.OnNext(value);
        }

        public IDisposable Subscribe(IObserver<TSource> observer)
        {
            if (null == observer)
            {
                throw new ArgumentNullException(nameof(observer));
            }

            Exception error = null;

            lock (gate)
            {
                EnsureNotDisposed();

                if (false == stopped)
                {
                    var listObserver = this.observer as ListObserver<TSource>;

                    if (null != listObserver)
                    {
                        listObserver.Add(observer);
                    }
                    else if(this.observer is EmptyObserver<TSource>)
                    {
                        this.observer = observer;
                    }
                    else
                    {
                        var observers = ImmutableList<IObserver<TSource>>.Empty
                            .AddRange(new[] {this.observer, observer});

                        this.observer = new ListObserver<TSource>(observers);
                    }

                    return new Subscription(this, observer);
                }

                error = lastError;
            }

            if (null != error)
            {
                observer.OnError(error);
            }
            else
            {
                observer.OnCompleted();
            }

            return Disposable.Empty;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool dispose)
        {
            if (disposed)
            {
                return;
            }

            try
            {
                if (dispose)
                {
                    observer = EmptyObserver<TSource>.Instance;
                    lastError = null;
                }
            }
            finally
            {
                disposed = true;
            }
        }

        private void EnsureNotDisposed()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(String.Empty);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private class Subscription : IDisposable
        {
            private readonly object gate = new object();

            private readonly Subject<TSource> parent;
            private readonly IObserver<TSource> unsubsciber;

            public Subscription(Subject<TSource> parent, IObserver<TSource> unsubsciber)
            {
                this.parent = parent;
                this.unsubsciber = unsubsciber;
            }

            public void Dispose()
            {
                lock (gate)
                {
                    if (null == parent)
                    {
                        return;
                    }

                    lock (parent.gate)
                    {
                        parent.observer = parent.observer is ListObserver<TSource> listObserver
                            ? listObserver.Remove(unsubsciber)
                            : EmptyObserver<TSource>.Instance;
                    }
                }
            }
        }
    }*/
}