using System;
using System.Collections.Generic;

namespace LibraProgramming.Windows.Games.Engine.Reactive.Events
{
    public struct CollectionAddEvent<T> : IEquatable<CollectionAddEvent<T>>
    {
        public int Index
        {
            get;
        }

        public T Value
        {
            get;
        }

        public CollectionAddEvent(int index, T value)
            : this()
        {
            Index = index;
            Value = value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is CollectionAddEvent<T> @event && Equals(@event);
        }

        public override int GetHashCode()
        {
            return Index.GetHashCode() ^ EqualityComparer<T>.Default.GetHashCode(Value) << 2;
        }

        public bool Equals(CollectionAddEvent<T> other)
        {
            return Index.Equals(other.Index) && EqualityComparer<T>.Default.Equals(Value, other.Value);
        }

        public override string ToString()
        {
            return $"Index:{Index} Value:{Value}";
        }
    }
}