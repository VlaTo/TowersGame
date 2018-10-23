using System.Collections.Generic;

namespace LibraProgramming.Windows.Games.Engine.Reactive
{
    public interface IEntityCollection : IEnumerable<IEntity>, INotifyingEntityCollection
    {
        string Name
        {
            get;
        }

        bool ContainsEntity(int entityId);

        IEntity CreateEntity(IBlueprint blueprint = null);

        void AddEntity(IEntity entity);

        IEntity GetEntity(int entityId);

        void RemoveEntity(int entityId, bool disposeOnRemoval = true);
    }
}