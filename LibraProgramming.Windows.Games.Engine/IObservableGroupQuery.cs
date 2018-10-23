using System.Collections.Generic;

namespace LibraProgramming.Windows.Games.Engine
{
    public interface IObservableGroupQuery
    {
        IEnumerable<IEntity> Execute(IObservableGroup observableGroup);
    }
}