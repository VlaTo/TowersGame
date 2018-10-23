using System;

namespace LibraProgramming.Windows.Games.Engine.Reactive
{
    public interface IComputed<out TValue> : IObservable<TValue>
    {
        TValue Value
        {
            get;
        }
    }
}