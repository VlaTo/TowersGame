using System.Collections.Generic;

namespace LibraProgramming.Windows.Games.Engine
{
    public interface ISystemExecutor
    {
        IEnumerable<ISystem> Systems
        {
            get;
        }

        void RemoveSystem(ISystem system);

        void AddSystem(ISystem system);
    }
}