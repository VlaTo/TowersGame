using System;
using LibraProgramming.Windows.Games.Engine.Infrastructure;
using LibraProgramming.Windows.Games.Engine.Infrastructure.Dependencies;

namespace LibraProgramming.Windows.Games.Engine
{
    public abstract class WindowsUniversalPlatformApplication : Application, IDisposable
    {
        private bool disposed;

        public override IDependencyContainer Container
        {
            get;
        }

        protected WindowsUniversalPlatformApplication()
        {
        }

        public void Dispose()
        {
            if (disposed)
            {
                return;
            }

            Dispose(true);
        }

        protected abstract void Dispose(bool dispose);
    }
}