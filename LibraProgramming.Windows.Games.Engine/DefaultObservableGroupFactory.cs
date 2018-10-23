namespace LibraProgramming.Windows.Games.Engine
{
    public class DefaultObservableGroupFactory : IObservableGroupFactory
    {
        public IObservableGroup Create(ObservableGroupConfiguration arg)
        {
            return new ObservableGroup(arg.ObservableGroupToken, arg.InitialEntities, arg.NotifyingCollection);
        }
    }
}