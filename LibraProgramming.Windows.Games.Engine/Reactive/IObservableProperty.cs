namespace LibraProgramming.Windows.Games.Engine.Reactive
{
    public interface IObservableProperty<TValue> : IReadOnlyObservableProperty<TValue>
    {
        new TValue Value
        {
            get;
            set;
        }
    }
}