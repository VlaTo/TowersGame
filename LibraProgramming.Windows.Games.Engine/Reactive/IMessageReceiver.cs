using System;

namespace LibraProgramming.Windows.Games.Engine.Reactive
{
    public interface IMessageReceiver
    {
        IObservable<TMessage> Receive<TMessage>();
    }
}