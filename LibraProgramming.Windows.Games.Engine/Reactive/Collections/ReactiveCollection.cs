using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using LibraProgramming.Windows.Games.Engine.Reactive.Events;

namespace LibraProgramming.Windows.Games.Engine.Reactive.Collections
{
    [Serializable]
    public class ReactiveCollection<T> : Collection<T>, IReactiveCollection<T>, IDisposable
    {
        [NonSerialized] private Subject<CollectionAddEvent<T>> added;
        [NonSerialized] private Subject<int> countChanged;
        [NonSerialized] private Subject<CollectionReplaceEvent<T>> replaced;
        [NonSerialized] private Subject<CollectionMoveEvent<T>> moved;
        [NonSerialized] private Subject<CollectionRemoveEvent<T>> removed;
        [NonSerialized] private Subject<Unit> resetted;
        [NonSerialized] private bool disposed;

        public IObservable<CollectionAddEvent<T>> Added
        {
            get
            {
                if (disposed)
                {
                    return Observable.Empty<CollectionAddEvent<T>>();
                }

                return added ?? (added = new Subject<CollectionAddEvent<T>>());
            }
        }

        public IObservable<CollectionMoveEvent<T>> Moved
        {
            get
            {
                if (disposed)
                {
                    return Observable.Empty<CollectionMoveEvent<T>>();
                }

                return moved ?? (moved = new Subject<CollectionMoveEvent<T>>());
            }
        }

        public IObservable<CollectionReplaceEvent<T>> Replaced
        {
            get
            {
                if (disposed)
                {
                    return Observable.Empty<CollectionReplaceEvent<T>>();
                }

                return replaced ?? (replaced = new Subject<CollectionReplaceEvent<T>>());
            }
        }

        public IObservable<CollectionRemoveEvent<T>> Removed
        {
            get
            {
                if (disposed)
                {
                    return Observable.Empty<CollectionRemoveEvent<T>>();
                }

                return removed ?? (removed = new Subject<CollectionRemoveEvent<T>>());
            }
        }

        public IObservable<Unit> Resetted
        {
            get
            {
                if (disposed)
                {
                    return Observable.Empty<Unit>();
                }

                return resetted ?? (resetted = new Subject<Unit>());
            }
        }

        public IObservable<int> CountChanged
        {
            get
            {
                if (disposed)
                {
                    return Observable.Empty<int>();
                }

                return countChanged ?? (countChanged = new Subject<int>());
            }
        }

        public ReactiveCollection()
        {
        }

        public ReactiveCollection(IEnumerable<T> source)
        {
            if (null == source)
            {
                throw new ArgumentNullException(nameof(source));
            }

            foreach (var item in source)
            {
                Add(item);
            }
        }

        public ReactiveCollection(IList<T> list)
            : base(list != null ? new List<T>(list) : null)
        {
        }

        public void Move(int oldIndex, int newIndex)
        {
            MoveItem(oldIndex, newIndex);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected override void ClearItems()
        {
            var count = Count;

            base.ClearItems();

            resetted.OnNext(Unit.Default);

            if (count > 0)
            {
                countChanged?.OnNext(Count);
            }
        }

        protected override void SetItem(int index, T item)
        {
            var oldItem = this[index];

            base.SetItem(index, item);

            replaced?.OnNext(new CollectionReplaceEvent<T>(index, oldItem, item));
        }

        protected override void InsertItem(int index, T item)
        {
            base.InsertItem(index, item);

            added?.OnNext(new CollectionAddEvent<T>(index, item));
            countChanged?.OnNext(Count);
        }

        protected override void RemoveItem(int index)
        {
            var item = this[index];

            base.RemoveItem(index);

            removed?.OnNext(new CollectionRemoveEvent<T>(index, item));
            countChanged?.OnNext(Count);
        }

        protected virtual void MoveItem(int oldIndex, int newIndex)
        {
            var item = this[oldIndex];

            base.RemoveItem(oldIndex);
            base.InsertItem(newIndex, item);

            moved?.OnNext(new CollectionMoveEvent<T>(oldIndex, newIndex, item));
        }

        private void Dispose(bool dispose)
        {
            if (disposed)
            {
                return;
            }

            try
            {
                if (dispose)
                {
                    DisposeSubject(ref added);
                    DisposeSubject(ref removed);
                    DisposeSubject(ref moved);
                    DisposeSubject(ref replaced);
                    DisposeSubject(ref resetted);
                    DisposeSubject(ref countChanged);
                }
            }
            finally
            {
                disposed = true;
            }
        }

        private static void DisposeSubject<TSubject>(ref Subject<TSubject> subject)
        {
            if (null == subject)
            {
                return;
            }

            try
            {
                subject.OnCompleted();
            }
            finally
            {
                subject.Dispose();
                subject = null;
            }
        }
    }
}