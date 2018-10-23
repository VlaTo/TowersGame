using System;
using System.Collections.Generic;
using System.Linq;
using LibraProgramming.Windows.Games.Engine.Reactive.Extensions;

namespace LibraProgramming.Windows.Games.Engine
{
    public class SystemExecutor : ISystemExecutor, IDisposable
    {
        public IList<ISystem> Systems
        {
            get;
        }

        public IEnumerable<IConventionalSystemHandler> ConventionalSystemHandlers
        {
            get;
        }

        IEnumerable<ISystem> ISystemExecutor.Systems => Systems;

        public SystemExecutor(IEnumerable<IConventionalSystemHandler> conventionalSystemHandlers)
        {
            ConventionalSystemHandlers = conventionalSystemHandlers;
            Systems = new List<ISystem>();
        }

        public void RemoveSystem(ISystem system)
        {
            var applicableHandlers = ConventionalSystemHandlers
                .Where(handler => handler.CanHandleSystem(system))
                .OrderByPriority();

            foreach (var handler in applicableHandlers)
            {
                handler.TeardownSystem(system);
            }

            Systems.Remove(system);
        }

        public void AddSystem(ISystem system)
        {
            var applicableHandlers = ConventionalSystemHandlers
                .Where(handler => handler.CanHandleSystem(system))
                .OrderByPriority();

            foreach (var handler in applicableHandlers)
            {
                handler.SetupSystem(system);
            }

            Systems.Add(system);
        }

        public void Dispose()
        {
            for (var index = Systems.Count - 1; index >= 0; index--)
            {
                RemoveSystem(Systems[index]);
            }

            Systems.Clear();
            ConventionalSystemHandlers.DisposeAll();
        }
    }
}