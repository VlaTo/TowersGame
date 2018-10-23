using System;
using LibraProgramming.Windows.Games.Engine.Infrastructure;

namespace LibraProgramming.Windows.Games.Engine
{
    public class DefaultEntityFactory : IEntityFactory
    {
        public IIdPool IdPool
        {
            get;
        }

        public IComponentRepository ComponentRepository
        {
            get;
        }

        public DefaultEntityFactory(IIdPool idPool, IComponentRepository componentRepository)
        {
            IdPool = idPool;
            ComponentRepository = componentRepository;
        }

        public int GetId(int? id = null)
        {
            if (false == id.HasValue)
            {
                return IdPool.AllocateInstance();
            }

            IdPool.AllocateSpecificId(id.Value);

            return id.Value;
        }

        public IEntity Create(int? arg)
        {
            if (arg.HasValue && 0 == arg.Value)
            {
                throw new ArgumentException("id must be null or > 0");
            }

            var usedId = GetId(arg);

            return new Entity(usedId, ComponentRepository);
        }

        public void Destroy(int entityId) => IdPool.ReleaseInstance(entityId);
    }
}