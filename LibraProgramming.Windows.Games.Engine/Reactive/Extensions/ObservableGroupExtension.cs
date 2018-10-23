using System.Collections.Generic;

namespace LibraProgramming.Windows.Games.Engine.Reactive.Extensions
{
    public static class ObservableGroupExtension
    {
        public static IEnumerable<IEntity> Query(this IObservableGroup observableGroupAccesssor,
            IObservableGroupQuery query) =>
            query.Execute(observableGroupAccesssor);
    }
}