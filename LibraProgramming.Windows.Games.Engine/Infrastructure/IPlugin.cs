using System;
using System.Collections.Generic;
using LibraProgramming.Windows.Games.Engine.Infrastructure.Dependencies;

namespace LibraProgramming.Windows.Games.Engine.Infrastructure
{
    public interface IPlugin
    {
        string Name
        {
            get;
        }

        Version Version
        {
            get;
        }

        void SetupDependencies(IDependencyContainer container);

        IEnumerable<ISystem> GetSystemsForRegistration(IDependencyContainer container);
    }
}