using System;

namespace LibraProgramming.Windows.Games.Engine.Reactive
{
    public static class Disposable
    {
        public static readonly IDisposable Empty;

        static Disposable()
        {
            Empty = EmptyDisposable.Instance;
        }
    }
}