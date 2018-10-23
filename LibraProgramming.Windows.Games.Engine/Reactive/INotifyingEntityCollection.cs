using System;
using LibraProgramming.Windows.Games.Engine.Events;

namespace LibraProgramming.Windows.Games.Engine.Reactive
{
    public interface INotifyingEntityCollection
    {
        IObservable<CollectionEntityEvent> EntityAdded
        {
            get;
        }

        IObservable<CollectionEntityEvent> EntityRemoved
        {
            get;
        }

        IObservable<ComponentsChangedEvent> EntityComponentsAdded
        {
            get;
        }

        IObservable<ComponentsChangedEvent> EntityComponentsRemoving
        {
            get;
        }

        IObservable<ComponentsChangedEvent> EntityComponentsRemoved
        {
            get;
        }
    }
}