namespace LibraProgramming.Windows.Games.Engine
{
    public interface IEntityFactory : IFactory<IEntity, int?>
    {
        void Destroy(int entityId);
    }
}