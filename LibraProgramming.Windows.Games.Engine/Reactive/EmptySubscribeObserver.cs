using System;
using System.Threading;

namespace LibraProgramming.Windows.Games.Engine.Reactive
{
    internal class EmptySubscribeObserver<TSource> : IObserver<TSource>
    {
        private readonly Action<Exception> error;
        private readonly Action completed;
        private int stopped;

        public EmptySubscribeObserver(Action<Exception> error, Action completed)
        {
            this.error = error;
            this.completed = completed;
        }

        public void OnCompleted()
        {
            if (1 == Interlocked.Increment(ref stopped))
            {
                completed.Invoke();
            }
        }

        public void OnError(Exception exception)
        {
            if (1 == Interlocked.Increment(ref stopped))
            {
                error.Invoke(exception);
            }
        }

        public void OnNext(TSource value)
        {
        }
    }
}