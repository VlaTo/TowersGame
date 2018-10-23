namespace LibraProgramming.Windows.Games.Engine
{
    public interface IManualSystem : ISystem
    {
        void Start(IObservableGroup group);

        void Stop(IObservableGroup group);
    }
}