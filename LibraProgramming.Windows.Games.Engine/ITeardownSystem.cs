namespace LibraProgramming.Windows.Games.Engine
{
    public interface ITeardownSystem : ISystem
    {
        void Teardown(IEntity entity);
    }
}