using LibraProgramming.Windows.Games.Engine.Reactive;

namespace LibraProgramming.Windows.Games.Engine.Events
{
    public class ComponentsChangedEvent
    {
        public IEntity Entity
        {
            get;
        }

        public IEntityCollection Collection
        {
            get;
        }

        public int[] ComponentTypeIds
        {
            get;
        }

        public ComponentsChangedEvent(IEntity entity, IEntityCollection collection, int[] componentTypeIds)
        {
            Entity = entity;
            Collection = collection;
            ComponentTypeIds = componentTypeIds;
        }
    }
}