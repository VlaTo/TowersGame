using System;

namespace LibraProgramming.Windows.Games.Engine
{
    public interface IReactToDataSystem<TData> : ISystem
    {
        IObservable<TData> ReactToData(IEntity entity);

        void Process(IEntity entity, TData data);
    }
}