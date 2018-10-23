using System;

namespace LibraProgramming.Windows.Games.Engine
{
    public interface IReactToEntitySystem : ISystem
    {
        IObservable<IEntity> ReactToEntity(IEntity entity);

        void Process(IEntity entity);
    }
}