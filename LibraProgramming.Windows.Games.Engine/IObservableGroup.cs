using System;
using System.Collections.Generic;

namespace LibraProgramming.Windows.Games.Engine
{
    public interface IObservableGroup : IEnumerable<IEntity>
    {
        IObservable<IEntity> EntityAdded
        {
            get;
        }

        IObservable<IEntity> EntityRemoved
        {
            get;
        }

        IObservable<IEntity> EntityRemoving
        {
            get;
        }

        ObservableGroupToken Token
        {
            get;
        }

        bool HasEntity(int id);
    }
}