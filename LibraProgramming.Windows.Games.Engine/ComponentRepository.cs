using System;
using System.Collections.Generic;

namespace LibraProgramming.Windows.Games.Engine
{
    public sealed class ComponentRepository : IComponentRepository
    {
        public IComponentTypeLookup Lookup
        {
            get;
        }

        public IComponentStore Store
        {
            get;
        }

        public ComponentRepository(IComponentTypeLookup lookup, IComponentStore store, int expansionSize = 1024)
        {
            Lookup = lookup;
            Store = store;
        }

        public Type[] GetTypesFor(params int[] componentTypeIds) => Lookup.GetComponentTypes(componentTypeIds);

        public int[] GetTypesFor(params Type[] componentTypes) => Lookup.GetComponentTypes(componentTypes);

        public IEnumerable<IComponent> GetAllFor(int entityId) => Store.GetAllFor(entityId);

        public bool Has(int entityId, int componentTypeId) => Store.Has(componentTypeId, entityId);

        public bool Has(int entityId, Type componenType)
        {
            var componentTypeId = Lookup.GetComponentType(componenType);
            return Store.Has(componentTypeId, entityId);
        }

        public IComponent Get(int entityId, int componentTypeId) => Store.Get(componentTypeId, entityId);

        public IComponent Get(int entityId, Type componenType)
        {
            var componentTypeId = Lookup.GetComponentType(componenType);
            return Store.Get(componentTypeId, entityId);
        }

        public int Add<TComponent>(int entityId, TComponent component)
            where TComponent : class, IComponent
        {
            if (null == component)
            {
                throw new ArgumentNullException(nameof(component));
            }

            var type = component.GetType();
            var componentTypeId = Lookup.GetComponentType(type);

            Store.Add(componentTypeId, entityId, component);

            return componentTypeId;
        }

        public void Remove(int entityId, int componentTypeId) => Store.Remove(componentTypeId, entityId);

        public void Remove(int entityId, Type componentType)
        {
            var componentTypeId = Lookup.GetComponentType(componentType);
            Remove(entityId, componentTypeId);
        }

        public void RemoveAll(int entityId) => Store.RemoveAllFor(entityId);
    }
}