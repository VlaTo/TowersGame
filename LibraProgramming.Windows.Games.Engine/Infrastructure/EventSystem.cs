using System;
using LibraProgramming.Windows.Games.Engine.Reactive;

namespace LibraProgramming.Windows.Games.Engine.Infrastructure
{
    public class EventSystem : IEventSystem
    {
        public IMessageBroker MessageBroker
        {
            get;
        }

        public EventSystem(IMessageBroker messageBroker)
        {
            MessageBroker = messageBroker;
        }

        public void Publish<T>(T message) => MessageBroker.Publish(message);

        public IObservable<T> Receive<T>() => MessageBroker.Receive<T>();
    }
}