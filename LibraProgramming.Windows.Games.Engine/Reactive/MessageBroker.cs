using System;
using System.Collections.Generic;
using System.Reactive.Subjects;

namespace LibraProgramming.Windows.Games.Engine.Reactive
{
    public sealed class MessageBroker : IMessageBroker, IDisposable
    {
        public static readonly IMessageBroker Instance;

        private readonly object gate = new object();

        private bool disposed;
        private readonly Dictionary<Type, object> notifiers;

        static MessageBroker()
        {
            Instance = new MessageBroker();
        }

        private MessageBroker()
        {
            notifiers = new Dictionary<Type, object>();
        }

        public IObservable<TMessage> Receive<TMessage>()
        {
            object notifier;

            lock (gate)
            {
                if (disposed)
                {
                    throw new ObjectDisposedException(String.Empty);
                }

                if (false == notifiers.TryGetValue(typeof(TMessage), out notifier))
                {
                    notifier = new Subject<TMessage>();
                    notifiers.Add(typeof(TMessage), notifier);
                }
            }

            return (IObservable<TMessage>) notifier;
        }

        public void Publish<TMessage>(TMessage message)
        {
            object notifier;

            lock (gate)
            {
                if (disposed)
                {
                    return;
                }

                if (false == notifiers.TryGetValue(typeof(TMessage), out notifier))
                {
                    return;
                }
            }

            ((ISubject<TMessage>) notifier).OnNext(message);
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
                    lock (gate)
                    {
                        notifiers.Clear();
                    }
                }
            }
            finally
            {
                disposed = true;
            }
        }
    }
}