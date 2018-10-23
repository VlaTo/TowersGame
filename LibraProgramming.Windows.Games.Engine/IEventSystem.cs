using System;

namespace LibraProgramming.Windows.Games.Engine
{
    public interface IEventSystem
    {
        void Publish<TEvent>(TEvent @event);

        IObservable<TEvent> Receive<TEvent>();
    }
}