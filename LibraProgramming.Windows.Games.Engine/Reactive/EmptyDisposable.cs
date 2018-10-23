using System;

namespace LibraProgramming.Windows.Games.Engine.Reactive
{
    public sealed class EmptyDisposable : IDisposable
    {
        public static readonly EmptyDisposable Instance;

        static EmptyDisposable()
        {
            Instance = new EmptyDisposable();
        }

        private EmptyDisposable()
        {
        }

        public void Dispose()
        {
        }
    }
}