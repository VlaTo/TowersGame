using System.Collections.Generic;
using LibraProgramming.Windows.Games.Engine.Reactive;

namespace LibraProgramming.Windows.Games.Engine
{
    public class ObservableGroupConfiguration
    {
        public ObservableGroupToken ObservableGroupToken
        {
            get;
            set;
        }

        public INotifyingEntityCollection NotifyingCollection
        {
            get;
            set;
        }

        public IEnumerable<IEntity> InitialEntities
        {
            get;
            set;
        }
    }
}