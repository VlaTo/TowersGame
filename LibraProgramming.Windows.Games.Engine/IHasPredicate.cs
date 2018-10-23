namespace LibraProgramming.Windows.Games.Engine
{
    public interface IHasPredicate
    {
        bool CanProcessEntity(IEntity entity);
    }
}