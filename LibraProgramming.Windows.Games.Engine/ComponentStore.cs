using System.Collections.Generic;
using LibraProgramming.Windows.Games.Engine.Infrastructure.Extensions;

namespace LibraProgramming.Windows.Games.Engine
{
    public class ComponentStore : IComponentStore
    {
        public int ComponentsCount
        {
            get
            {
                if (0 == EntityComponents.Length)
                {
                    return 0;
                }

                return EntityComponents[0].Count;
            }
        }

        public List<IComponent>[] EntityComponents
        {
            get;
            private set;
        }

        public IComponentTypeLookup ComponentTypeLookup
        {
            get;
        }

        public ComponentStore(IComponentTypeLookup componentTypeLookup, int entityCapacity = 1024)
        {
            ComponentTypeLookup = componentTypeLookup;
            Initialize(entityCapacity);
        }

        public void Initialize(int entityCapacity)
        {
            var componentCount = ComponentTypeLookup.GetAllComponentTypes().Count;

            EntityComponents = new List<IComponent>[componentCount];

            for (var index = 0; index < componentCount; index++)
            {
                EntityComponents[index] = new List<IComponent>();
            }

            EnsureEntityCapacity(entityCapacity);
        }

        public void EnsureEntityCapacity(int entityCapacity)
        {
            var delta = entityCapacity - ComponentsCount;

            for (var index = 0; index < EntityComponents.Length; index++)
            {
                EntityComponents[index].ExpandBy(delta);
            }
        }

        public IComponent Get(int componentTypeId, int entityId) => EntityComponents[componentTypeId][entityId];

        public bool Has(int componentTypeId, int entityId)
        {
            if (entityId >= EntityComponents[componentTypeId].Count)
            {
                return false;
            }

            return null != EntityComponents[componentTypeId][entityId];
        }

        public void Add(int componentTypeId, int entityId, IComponent component) => EntityComponents[componentTypeId][entityId] = component;

        public void Remove(int componentTypeId, int entityId) => EntityComponents[componentTypeId][entityId] = null;

        public IEnumerable<IComponent> GetAllFor(int entityId)
        {
            for (var index = EntityComponents.Length - 1; index >= 0; index--)
            {
                var component = EntityComponents[index][entityId];

                if (component != null)
                {
                    yield return component;
                }
            }
        }

        public void RemoveAllFor(int entityId)
        {
            for (var index = EntityComponents.Length - 1; index >= 0; index--)
            {
                Remove(index, entityId);
            }
        }
    }
}