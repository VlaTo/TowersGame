namespace LibraProgramming.Windows.Games.Engine.Infrastructure
{
    public interface IIdPool : IPool<int>
    {
        bool IsAvailable(int id);

        void AllocateSpecificId(int id);
    }
}