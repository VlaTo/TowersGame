using System;
using System.Collections.Generic;

namespace LibraProgramming.Windows.Games.Engine
{
    public interface IComponentRepository
    {
        Type[] GetTypesFor(params int[] componentTypeIds);

        int[] GetTypesFor(params Type[] componentTypes);

        IEnumerable<IComponent> GetAllFor(int entityId);

        bool Has(int entityId, int componentTypeId);

        bool Has(int entityId, Type componenType);

        IComponent Get(int entityId, int componentTypeId);

        IComponent Get(int entityId, Type componenType);

        int Add<TComponent>(int entityId, TComponent component)
            where TComponent : class, IComponent;

        void Remove(int entityId, Type componentType);

        void Remove(int entityId, int componentTypeId);

        void RemoveAll(int entityId);
    }
}