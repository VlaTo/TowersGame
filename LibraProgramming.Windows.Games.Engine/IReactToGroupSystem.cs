using System;

namespace LibraProgramming.Windows.Games.Engine
{
    public interface IReactToGroupSystem : ISystem
    {
        IObservable<IObservableGroup> ReactToGroup(IObservableGroup @group);

        void Process(IEntity entity);
    }
}