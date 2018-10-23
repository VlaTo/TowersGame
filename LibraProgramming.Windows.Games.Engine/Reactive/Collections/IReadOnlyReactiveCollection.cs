using System;
using System.Collections.Generic;
using System.Reactive;
using LibraProgramming.Windows.Games.Engine.Reactive.Events;

namespace LibraProgramming.Windows.Games.Engine.Reactive.Collections
{
    public interface IReadOnlyReactiveCollection<T> : IReadOnlyCollection<T>
    {
        IObservable<CollectionAddEvent<T>> Added
        {
            get;
        }

        IObservable<CollectionMoveEvent<T>> Moved
        {
            get;
        }

        IObservable<CollectionRemoveEvent<T>> Removed
        {
            get;
        }

        IObservable<CollectionReplaceEvent<T>> Replaced
        {
            get;
        }

        IObservable<Unit> Resetted
        {
            get;
        }

        IObservable<int> CountChanged
        {
            get;
        }
    }
}