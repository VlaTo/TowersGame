using System;

namespace LibraProgramming.Windows.Games.Engine.Reactive
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    public sealed class EmptyObserver<TSource> : IObserver<TSource>
    {
        public static readonly EmptyObserver<TSource> Instance;

        static EmptyObserver()
        {
            Instance = new EmptyObserver<TSource>();
        }

        private EmptyObserver()
        {
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(TSource value)
        {
        }
    }
}