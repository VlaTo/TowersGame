using System;

namespace LibraProgramming.Windows.Games.Engine.Reactive
{
    public sealed class DisposedObserver<TSource> : IObserver<TSource>
    {
        public static readonly DisposedObserver<TSource> Instance;

        static DisposedObserver()
        {
            Instance = new DisposedObserver<TSource>();
        }

        private DisposedObserver()
        {
        }

        public void OnCompleted()
        {
            throw new ObjectDisposedException(String.Empty);
        }

        public void OnError(Exception error)
        {
            throw new ObjectDisposedException(String.Empty);
        }

        public void OnNext(TSource value)
        {
            throw new ObjectDisposedException(String.Empty);
        }
    }
}