namespace LibraProgramming.Windows.Games.Engine.Reactive
{
    public interface IMessagePublisher
    {
        void Publish<TMessage>(TMessage message);
    }
}