using System;
using System.Collections.Generic;
using LibraProgramming.Windows.Games.Engine.Reactive;
using LibraProgramming.Windows.Games.Engine.Reactive.Extensions;

namespace LibraProgramming.Windows.Games.Engine
{
    public class TeardownSystemHandler : IConventionalSystemHandler
    {
        public IEntityCollectionManager EntityCollectionManager
        {
            get;
        }

        public IDictionary<ISystem, IDisposable> Subscriptions
        {
            get;
        }

        public TeardownSystemHandler()
        {
            EntityCollectionManager = null;
            Subscriptions = new Dictionary<ISystem, IDisposable>();
        }

        public bool CanHandleSystem(ISystem system) => system is ITeardownSystem;

        public void SetupSystem(ISystem system)
        {
            var disposer = new CompositeDisposable();

            Subscriptions.Add(system, disposer);

            var castSystem = (ITeardownSystem) system;
            var accessor = EntityCollectionManager.GetObservableGroup(system.Group);

            disposer.Add(
                accessor.EntityRemoved.Subscribe(castSystem.Teardown)
            );
        }

        public void TeardownSystem(ISystem system)
        {
            Subscriptions.RemoveAndDispose(system);
        }

        public void Dispose()
        {
            Subscriptions.DisposeAll();
        }
    }
}