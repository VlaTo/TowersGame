using System.Collections.Generic;

namespace LibraProgramming.Windows.Games.Engine.Reactive
{
    public interface IEntityCollectionQuery
    {
        IEnumerable<IEntity> Execute(IEnumerable<IEntity> entities);
    }
}