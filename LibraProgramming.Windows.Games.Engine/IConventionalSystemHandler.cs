using System;

namespace LibraProgramming.Windows.Games.Engine
{
    public interface IConventionalSystemHandler : IDisposable
    {
        bool CanHandleSystem(ISystem system);

        void SetupSystem(ISystem system);

        void TeardownSystem(ISystem system);
    }
}