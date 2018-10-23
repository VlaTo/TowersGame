using System;
using System.Threading;

namespace LibraProgramming.Windows.Games.Engine.Reactive
{
    public sealed class SubscribeObserver<TSource> : IObserver<TSource>
    {
        private readonly Action<TSource> onNext;
        private readonly Action<Exception> onError;
        private readonly Action onCompleted;

        private int stops;

        public SubscribeObserver(Action<TSource> onNext, Action<Exception>onError, Action onCompleted)
        {
            this.onNext = onNext;
            this.onError = onError;
            this.onCompleted = onCompleted;
        }

        public void OnCompleted()
        {
            if (1 == Interlocked.Increment(ref stops))
            {
                onCompleted.Invoke();
            }
        }

        public void OnError(Exception error)
        {
            if (1 == Interlocked.Increment(ref stops))
            {
                onError.Invoke(error);
            }
        }

        public void OnNext(TSource value)
        {
            if (0 == stops)
            {
                onNext.Invoke(value);
            }
        }
    }
}