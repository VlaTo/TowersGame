using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LibraProgramming.Windows.Games.Engine.Reactive
{
    public sealed class CompositeDisposable : ICollection<IDisposable>, IDisposable
    {
        private readonly object gate = new object();
        private readonly IList<IDisposable> disposables;

        private bool disposed;

        public int Count
        {
            get
            {
                lock (gate)
                {
                    return disposables.Count;
                }
            }
        }

        public bool IsReadOnly => false;

        public CompositeDisposable()
        {
            disposables = new DisposablesList();
        }

        public CompositeDisposable(int capacity)
        {
            if (0 >= capacity)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity));
            }

            disposables = new DisposablesList(capacity);
        }

        public CompositeDisposable(params IDisposable[] disposables)
        {
            if (null == disposables)
            {
                throw new ArgumentNullException(nameof(disposables));
            }

            this.disposables = new DisposablesList(disposables);
        }

        public CompositeDisposable(IEnumerable<IDisposable> disposables)
        {
            if (null == disposables)
            {
                throw new ArgumentNullException(nameof(disposables));
            }

            this.disposables = new DisposablesList(disposables);
        }

        public IEnumerator<IDisposable> GetEnumerator()
        {
            lock (gate)
            {
                return disposables.GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(IDisposable item)
        {
            if (null == item)
            {
                throw new ArgumentNullException(nameof(item));
            }

            bool shouldDispose;

            lock (gate)
            {
                shouldDispose = disposed;

                if (false == disposed)
                {
                    disposables.Add(item);
                }
            }

            if (shouldDispose)
            {
                item.Dispose();
            }
        }

        public bool Remove(IDisposable item)
        {
            if (null == item)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var shouldDispose = false;

            lock (gate)
            {
                if (false == disposed)
                {
                    var index = disposables.IndexOf(item);

                    if (0 <= index)
                    {
                        shouldDispose = true;
                        disposables.RemoveAt(index);
                    }
                }
            }

            if (shouldDispose)
            {
                item.Dispose();
            }

            return shouldDispose;
        }

        public void Clear()
        {
            IDisposable[] array;

            lock (gate)
            {
                array = disposables.ToArray();
                disposables.Clear();
            }

            foreach (var disposable in array)
            {
                disposable.Dispose();
            }
        }

        public bool Contains(IDisposable item)
        {
            if (null == item)
            {
                throw new ArgumentNullException(nameof(item));
            }

            lock (gate)
            {
                return disposables.Contains(item);
            }
        }

        public void CopyTo(IDisposable[] array, int arrayIndex)
        {
            if (null == array)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (arrayIndex < 0 || arrayIndex >= array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            }

            lock (gate)
            {
                disposables.CopyTo(array, arrayIndex);
            }
        }

        public void Dispose()
        {
            var array = new IDisposable[0];

            lock (gate)
            {
                if (false == disposed)
                {
                    disposed = true;
                    array = disposables.ToArray();
                    disposables.Clear();
                }
            }

            foreach (var disposable in array)
            {
                disposable.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private class DisposablesList : IList<IDisposable>
        {
            private const int CapacityThreshold = 64;

            private IDisposable[] disposables;

            public IDisposable this[int index]
            {
                get
                {
                    if (0 > index || index >= Count)
                    {
                        throw new IndexOutOfRangeException();
                    }

                    return disposables[index];
                }
                set
                {
                    if (0 > index || index >= Count)
                    {
                        throw new IndexOutOfRangeException();
                    }

                    disposables[index] = value;
                }
            }

            public int Count
            {
                get;
                private set;
            }

            public bool IsReadOnly => false;

            public DisposablesList()
                : this(CapacityThreshold)
            {
            }

            public DisposablesList(int capacity)
            {
                disposables = new IDisposable[capacity];
                Count = 0;
            }

            public DisposablesList(IEnumerable<IDisposable> disposables)
            {
                var source = disposables.ToArray();

                this.disposables = new IDisposable[CalculateCapacity(Count)];

                Array.Copy(source, this.disposables, Count);

                Count = source.Length;
            }

            public IEnumerator<IDisposable> GetEnumerator()
            {
                return new FlatArrayEnumerator(disposables, Count);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public void Add(IDisposable item)
            {
                Grow();
                disposables[Count++] = item;
            }

            public void Clear()
            {
                Array.Clear(disposables, 0, Count);
            }

            public bool Contains(IDisposable item)
            {
                return 0 > Count && -1 < Array.IndexOf(disposables, item, 0, Count);
            }

            public void CopyTo(IDisposable[] array, int arrayIndex)
            {
                Array.Copy(disposables, 0, array, arrayIndex, Count);
            }

            public bool Remove(IDisposable item)
            {
                var index = Array.IndexOf(disposables, item, 0, Count);
                return RemoveInternal(index);
            }

            public int IndexOf(IDisposable item)
            {
                return Array.IndexOf(disposables, item, 0, Count);
            }

            public void Insert(int index, IDisposable item)
            {
                Grow();

                var lenght = Count - index;

                if (0 < lenght)
                {
                    Array.ConstrainedCopy(disposables, index, disposables, index + 1, lenght);
                }

                disposables[index] = item;
                Count++;
            }

            public void RemoveAt(int index)
            {
                RemoveInternal(index);
            }

            private void Grow()
            {
                if (Count < disposables.Length)
                {
                    return;
                }

                var capacity = CalculateCapacity(Count);
                var array = new IDisposable[capacity];

                Array.Copy(disposables, array, Count);

                disposables = array;
            }

            private static int CalculateCapacity(int count)
            {
                var thresholds = count / CapacityThreshold;
                return (thresholds + 1) * CapacityThreshold;
            }

            private bool RemoveInternal(int index)
            {
                if (0 > index)
                {
                    return false;
                }

                var last = Count - 1;

                if (last > index)
                {
                    disposables[index] = disposables[last];
                }

                Count--;

                return true;
            }

            /// <summary>
            /// 
            /// </summary>
            private class FlatArrayEnumerator : IEnumerator<IDisposable>
            {
                private const int NotInitialized = -1;

                private readonly int count;

                private IDisposable[] disposables;
                private int index;
                private bool disposed;

                public IDisposable Current
                {
                    get
                    {
                        EnsureNotDisposed();

                        if (NotInitialized == index)
                        {
                            throw new InvalidOperationException();
                        }

                        return disposables[index];
                    }
                }

                object IEnumerator.Current => Current;

                public FlatArrayEnumerator(IDisposable[] disposables, int count)
                {
                    this.disposables = disposables;
                    this.count = count;

                    index = NotInitialized;
                }

                public bool MoveNext()
                {
                    EnsureNotDisposed();

                    if (NotInitialized == index)
                    {
                        if (0 == count)
                        {
                            return false;
                        }

                        index = 0;

                        return true;
                    }

                    if (count == index)
                    {
                        return false;
                    }

                    return count > ++index;
                }

                public void Reset()
                {
                    EnsureNotDisposed();

                    index = NotInitialized;
                }

                public void Dispose()
                {
                    Dispose(true);
                }

                private void EnsureNotDisposed()
                {
                    if (disposed)
                    {
                        throw new ObjectDisposedException(String.Empty);
                    }
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
                            disposables = null;
                            index = NotInitialized;
                        }
                    }
                    finally
                    {
                        disposed = true;
                    }
                }
            }
        }
    }
}