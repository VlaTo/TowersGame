using LibraProgramming.Windows.Games.Engine.Reactive;

namespace LibraProgramming.Windows.Games.Engine
{
    public class DefaultEntityCollectionFactory : IEntityCollectionFactory
    {
        private readonly IEntityFactory entityFactory;
        public DefaultEntityCollectionFactory(IEntityFactory entityFactory)
        {
            this.entityFactory = entityFactory;
        }

        public IEntityCollection Create(string arg)
        {
            return new EntityCollection(arg, entityFactory);
        }
    }
}