using System;
using System.Collections.Generic;
using LibraProgramming.Windows.Games.Engine.Attributes;
using LibraProgramming.Windows.Games.Engine.Reactive;
using LibraProgramming.Windows.Games.Engine.Reactive.Extensions;

namespace LibraProgramming.Windows.Games.Engine
{
    [Priority(3)]
    public class ReactToEntitySystemHandler : IConventionalSystemHandler
    {
        public IDictionary<ISystem, IDictionary<int, IDisposable>> EntitySubscriptions
        {
            get;
        }

        public IDictionary<ISystem, IDisposable> SystemSubscriptions
        {
            get;
        }

        public IEntityCollectionManager EntityCollectionManager
        {
            get;
        }

        public ReactToEntitySystemHandler(IEntityCollectionManager entityCollectionManager)
        {
            EntityCollectionManager = entityCollectionManager;
            SystemSubscriptions = new Dictionary<ISystem, IDisposable>();
            EntitySubscriptions = new Dictionary<ISystem, IDictionary<int, IDisposable>>();
        }

        public bool CanHandleSystem(ISystem system) => system is IReactToEntitySystem;

        public void SetupSystem(ISystem system)
        {
            var observableGroup = EntityCollectionManager.GetObservableGroup(system.Group);
            var entitySubscriptions = new Dictionary<int, IDisposable>();
            var entityChangeSubscriptions = new CompositeDisposable();

            SystemSubscriptions.Add(system, entityChangeSubscriptions);

            var castSystem = (IReactToEntitySystem)system;

            entityChangeSubscriptions.Add(
                observableGroup.EntityAdded
                    .Subscribe(entity =>
                    {
                        var entitySubscription = ProcessEntity(castSystem, entity);
                        entitySubscriptions.Add(entity.Id, entitySubscription);
                    })
            );

            entityChangeSubscriptions.Add(
                observableGroup.EntityRemoved
                    .Subscribe(entity =>
                    {
                        entitySubscriptions.RemoveAndDispose(entity.Id);
                    })
            );

            foreach (var entity in observableGroup)
            {
                var entitySubscription = ProcessEntity(castSystem, entity);
                entitySubscriptions.Add(entity.Id, entitySubscription);
            }

            EntitySubscriptions.Add(system, entitySubscriptions);
        }

        public IDisposable ProcessEntity(IReactToEntitySystem system, IEntity entity)
        {
            var hasEntityPredicate = system.Group is IHasPredicate;
            var reactObservable = system.ReactToEntity(entity);

            if (false == hasEntityPredicate)
            {
                return reactObservable.Subscribe(system.Process);
            }

            var groupPredicate = (IHasPredicate) system.Group;

            return reactObservable.Subscribe(x =>
            {
                if (groupPredicate.CanProcessEntity(x))
                {
                    system.Process(x);
                }
            });
        }

        public void TeardownSystem(ISystem system)
        {
            SystemSubscriptions.RemoveAndDispose(system);

            var entitySubscriptions = EntitySubscriptions[system];

            entitySubscriptions.Values.DisposeAll();
            entitySubscriptions.Clear();

            EntitySubscriptions.Remove(system);
        }

        public void Dispose()
        {
            SystemSubscriptions.DisposeAll();

            foreach (var entitySubscriptions in EntitySubscriptions.Values)
            {
                entitySubscriptions.Values.DisposeAll();
                entitySubscriptions.Clear();
            }

            EntitySubscriptions.Clear();
        }
    }
}