using System;
using System.Collections.Generic;
using LibraProgramming.Windows.Games.Engine.Attributes;
using LibraProgramming.Windows.Games.Engine.Reactive;
using LibraProgramming.Windows.Games.Engine.Reactive.Extensions;

namespace LibraProgramming.Windows.Games.Engine
{
    [Priority(10)]
    public class SetupSystemHandler : IConventionalSystemHandler
    {
        public IEntityCollectionManager EntityCollectionManager
        {
            get;
        }

        public IDictionary<ISystem, IDictionary<int, IDisposable>> EntitySubscriptions
        {
            get;
        }

        public IDictionary<ISystem, IDisposable> SystemSubscriptions
        {
            get;
        }

        public SetupSystemHandler(IEntityCollectionManager entityCollectionManager)
        {
            EntityCollectionManager = entityCollectionManager;
            SystemSubscriptions = new Dictionary<ISystem, IDisposable>();
            EntitySubscriptions = new Dictionary<ISystem, IDictionary<int, IDisposable>>();
        }

        public bool CanHandleSystem(ISystem system) => system is ISetupSystem;

        public void SetupSystem(ISystem system)
        {
            var entitySubscriptions = new Dictionary<int, IDisposable>();

            EntitySubscriptions.Add(system, entitySubscriptions);

            var entityChangeSubscriptions = new CompositeDisposable();

            SystemSubscriptions.Add(system, entityChangeSubscriptions);

            var castSystem = (ISetupSystem)system;
            var observableGroup = EntityCollectionManager.GetObservableGroup(system.Group);

            entityChangeSubscriptions.Add(
                observableGroup.EntityAdded.Subscribe(entity =>
                {
                    var possibleSubscription = ProcessEntity(castSystem, entity);

                    if (possibleSubscription != null)
                    {
                        entitySubscriptions.Add(entity.Id, possibleSubscription);
                    }
                })
            );

            entityChangeSubscriptions.Add(
                observableGroup.EntityRemoved
                    .Subscribe(entity =>
                    {
                        if (entitySubscriptions.ContainsKey(entity.Id))
                        {
                            entitySubscriptions.RemoveAndDispose(entity.Id);
                        }
                    })
            );

            foreach (var entity in observableGroup)
            {
                var possibleSubscription = ProcessEntity(castSystem, entity);

                if (null != possibleSubscription)
                {
                    entitySubscriptions.Add(entity.Id, possibleSubscription);
                }
            }
        }

        public void TeardownSystem(ISystem system) => SystemSubscriptions.RemoveAndDispose(system);

        public IDisposable ProcessEntity(ISetupSystem system, IEntity entity)
        {
            var hasEntityPredicate = system.Group is IHasPredicate;

            if (false == hasEntityPredicate)
            {
                system.Setup(entity);
                return null;
            }

            var groupPredicate = system.Group as IHasPredicate;

            if (groupPredicate.CanProcessEntity(entity))
            {
                system.Setup(entity);
                return null;
            }

            var disposable = entity
                .WaitForPredicateMet(groupPredicate.CanProcessEntity)
                .ContinueWith(x =>
                {
                    EntitySubscriptions[system].Remove(x.Result.Id);
                    system.Setup(x.Result);
                });

            return disposable;
        }

        public void Dispose()
        {
            SystemSubscriptions.DisposeAll();
        }
    }
}