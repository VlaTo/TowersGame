using LibraProgramming.Windows.Games.Engine.Reactive;

namespace LibraProgramming.Windows.Games.Engine.Events
{
    public class CollectionEntityEvent
    {
        public IEntity Entity
        {
            get;
        }

        public IEntityCollection Collection
        {
            get;
        }

        public CollectionEntityEvent(IEntity entity, IEntityCollection collection)
        {
            Entity = entity;
            Collection = collection;
        }
    }
}