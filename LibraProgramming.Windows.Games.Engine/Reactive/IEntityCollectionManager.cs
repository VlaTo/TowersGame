using System;
using System.Collections.Generic;

namespace LibraProgramming.Windows.Games.Engine.Reactive
{
    public interface IEntityCollectionManager : INotifyingEntityCollection
    {
        IEnumerable<IEntityCollection> Collections
        {
            get;
        }

        IObservable<IEntityCollection> CollectionAdded
        {
            get;
        }

        IObservable<IEntityCollection> CollectionRemoved
        {
            get;
        }

        IEnumerable<IEntity> GetEntitiesFor(IGroup @group, string collectionName = null);

        IObservableGroup GetObservableGroup(IGroup @group, string collectionName = null);

        IEntityCollection CreateCollection(string name);

        IEntityCollection GetCollection(string name = null);

        void RemoveCollection(string name, bool disposeEntities = true);
    }
}