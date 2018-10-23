using System.Collections.Generic;

namespace LibraProgramming.Windows.Games.Engine
{
    public interface IComponentStore
    {
        int ComponentsCount
        {
            get;
        }

        IComponent Get(int componentTypeId, int entityId);

        bool Has(int componentTypeId, int entityId);

        void Add(int componentTypeId, int entityId, IComponent component);

        void Remove(int componentTypeId, int entityId);

        IEnumerable<IComponent> GetAllFor(int entityId);

        void RemoveAllFor(int entityId);
    }
}