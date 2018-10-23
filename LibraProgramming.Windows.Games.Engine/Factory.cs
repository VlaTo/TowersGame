namespace LibraProgramming.Windows.Games.Engine
{
    public interface IFactory
    {
    }

    public interface IFactory<out TEntity> : IFactory
    {
        TEntity Create();
    }

    public interface IFactory<out TEntity, in T1> : IFactory
    {
        TEntity Create(T1 arg);
    }

    public interface IFactory<out TEntity, in T1, in T2> : IFactory
    {
        TEntity Create(T1 arg1, T2 arg2);
    }
}