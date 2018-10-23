using System;

namespace LibraProgramming.Windows.Games.Engine.Reactive
{
    public interface IReadOnlyObservableProperty<out TValue> : IObservable<TValue>
    {
        TValue Value
        {
            get;
        }

        bool HasValue
        {
            get;
        }
    }
}