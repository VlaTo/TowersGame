using System;
using System.Collections.Generic;
using Windows.Foundation.Collections;
using LibraProgramming.Windows.Games.Engine.Events;

namespace LibraProgramming.Windows.Games.Engine.Reactive.Collections
{
    public interface IComputedCollection<TValue> : IComputed<IEnumerable<TValue>>, IEnumerable<TValue>
    {
        TValue this[int index]
        {
            get;
        }

        int Count
        {
            get;
        }

        IObservable<CollectionElementChangedEvent<TValue>> Added
        {
            get;
        }

        IObservable<CollectionElementChangedEvent<TValue>> Removed
        {
            get;
        }

        IObservable<CollectionElementChangedEvent<TValue>> Changed
        {
            get;
        }
    }
}