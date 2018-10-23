using System;
using System.Collections.Generic;
using System.Linq;
using LibraProgramming.Windows.Games.Engine.Attributes;
using LibraProgramming.Windows.Games.Engine.Reactive;
using LibraProgramming.Windows.Games.Engine.Reactive.Extensions;

namespace LibraProgramming.Windows.Games.Engine
{
    [Priority(6)]
    public class ReactToGroupSystemHandler : IConventionalSystemHandler
    {
        public IEntityCollectionManager EntityCollectionManager
        {
            get;
        }

        public IDictionary<ISystem, IDisposable> SystemSubscriptions
        {
            get;
        }

        public ReactToGroupSystemHandler(IEntityCollectionManager entityCollectionManager)
        {
            EntityCollectionManager = entityCollectionManager;
            SystemSubscriptions = new Dictionary<ISystem, IDisposable>();
        }

        public bool CanHandleSystem(ISystem system) => system is IReactToGroupSystem;

        public void SetupSystem(ISystem system)
        {
            var observableGroup = EntityCollectionManager.GetObservableGroup(system.Group);
            var hasEntityPredicate = system.Group is IHasPredicate;
            var castSystem = (IReactToGroupSystem)system;
            var reactObservable = castSystem.ReactToGroup(observableGroup);

            if (false == hasEntityPredicate)
            {
                var noPredicateSub = reactObservable.Subscribe(x => ExecuteForGroup(x, castSystem));
                SystemSubscriptions.Add(system, noPredicateSub);

                return;
            }

            var groupPredicate = system.Group as IHasPredicate;
            var subscription = reactObservable.Subscribe(
                x => ExecuteForGroup(x.Where(groupPredicate.CanProcessEntity), castSystem)
            );

            SystemSubscriptions.Add(system, subscription);
        }

        private static void ExecuteForGroup(IEnumerable<IEntity> entities, IReactToGroupSystem castSystem)
        {
            foreach (var entity in entities)
            { castSystem.Process(entity); }
        }

        public void TeardownSystem(ISystem system) => SystemSubscriptions.RemoveAndDispose(system);

        public void Dispose()
        {
            SystemSubscriptions.Values.DisposeAll();
            SystemSubscriptions.Clear();
        }
    }
}