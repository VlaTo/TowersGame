using System;
using LibraProgramming.Windows.Games.Engine.Reactive;

namespace LibraProgramming.Windows.Games.Engine
{
    public abstract class EventReactionSystem<TEvent> : IManualSystem
    {
        private IDisposable subscription;

        public IGroup Group => new EmptyGroup();

        public IEventSystem System
        {
            get;
        }

        protected EventReactionSystem(IEventSystem system)
        {
            System = system;
        }

        public void Start(IObservableGroup @group)
        {
            subscription = System.Receive<TEvent>().Subscribe(OnEventReceived);
        }

        public void Stop(IObservableGroup @group)
        {
            subscription.Dispose();
        }

        public abstract void OnEventReceived(TEvent @event);
    }
}