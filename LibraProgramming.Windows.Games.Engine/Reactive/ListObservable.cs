using System;
using System.Collections.Immutable;

namespace LibraProgramming.Windows.Games.Engine.Reactive
{
    public sealed class ListObserver<TSource> : IObserver<TSource>
    {
        private readonly ImmutableList<IObserver<TSource>> observers;

        public ListObserver(ImmutableList<IObserver<TSource>> observers)
        {
            this.observers = observers;
        }

        public void OnCompleted()
        {
            for (var index = 0; index < observers.Count; index++)
            {
                observers[index].OnCompleted();
            }
        }

        public void OnError(Exception error)
        {
            for (var index = 0; index < observers.Count; index++)
            {
                observers[index].OnError(error);
            }
        }

        public void OnNext(TSource value)
        {
            for (var index = 0; index < observers.Count; index++)
            {
                observers[index].OnNext(value);
            }
        }

        internal IObserver<TSource> Add(IObserver<TSource> observer)
        {
            return new ListObserver<TSource>(observers.Add(observer));
        }

        internal IObserver<TSource> Remove(IObserver<TSource> observer)
        {
            var index = observers.IndexOf(observer);

            if (0 > index)
            {
                return this;
            }

            if (1 == observers.Count)
            {
                return EmptyObserver<TSource>.Instance;
            }

            return new ListObserver<TSource>(observers.Remove(observer));
        }
    }
}