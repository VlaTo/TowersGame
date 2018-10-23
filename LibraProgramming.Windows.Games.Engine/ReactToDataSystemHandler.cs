using System;
using System.Collections.Generic;
using System.Reflection;
using LibraProgramming.Windows.Games.Engine.Attributes;
using LibraProgramming.Windows.Games.Engine.Reactive;
using LibraProgramming.Windows.Games.Engine.Reactive.Extensions;

namespace LibraProgramming.Windows.Games.Engine
{
    [Priority(3)]
    public class ReactToDataSystemHandler : IConventionalSystemHandler
    {
        private readonly MethodInfo processEntityMethod;

        public IDictionary<ISystem, IDisposable> SystemSubscriptions
        {
            get;
        }

        public IDictionary<ISystem, IDictionary<int, IDisposable>> EntitySubscriptions
        {
            get;
        }

        public IEntityCollectionManager EntityCollectionManager
        {
            get;
        }

        public ReactToDataSystemHandler(IEntityCollectionManager entityCollectionManager)
        {
            EntityCollectionManager = entityCollectionManager;
            SystemSubscriptions = new Dictionary<ISystem, IDisposable>();
            EntitySubscriptions = new Dictionary<ISystem, IDictionary<int, IDisposable>>();
            processEntityMethod = GetType().GetMethod(nameof(ProcessEntity));
        }

        // TODO: This is REALLY bad but currently no other way around the dynamic invocation lookup stuff
        public Func<IEntity, IDisposable> CreateEntityProcessorFunction(ISystem system)
        {
            var genericDataType = system.GetGenericDataType();
            var genericMethod = processEntityMethod.MakeGenericMethod(genericDataType);
            return entity => (IDisposable)genericMethod.Invoke(this, new object[] { system, entity });
        }

        public IDisposable ProcessEntity<T>(IReactToDataSystem<T> system, IEntity entity)
        {
            var hasEntityPredicate = system.Group is IHasPredicate;
            var reactObservable = system.ReactToData(entity);

            if (false == hasEntityPredicate)
            {
                return reactObservable.Subscribe(x => system.Process(entity, x));
            }

            var groupPredicate = (IHasPredicate) system.Group;

            return reactObservable.Subscribe(x =>
            {
                if (groupPredicate.CanProcessEntity(entity))
                {
                    system.Process(entity, x);
                }
            });
        }

        public bool CanHandleSystem(ISystem system) => system.IsReactiveDataSystem();

        public void SetupSystem(ISystem system)
        {
            var processEntityFunction = CreateEntityProcessorFunction(system);
            var entityChangeSubscriptions = new CompositeDisposable();

            SystemSubscriptions.Add(system, entityChangeSubscriptions);

            var entitySubscriptions = new Dictionary<int, IDisposable>();

            EntitySubscriptions.Add(system, entitySubscriptions);

            var observableGroup = EntityCollectionManager.GetObservableGroup(system.Group);

            entityChangeSubscriptions.Add(
                observableGroup.EntityAdded.Subscribe(x =>
                {
                    var subscription = processEntityFunction(x);
                    entitySubscriptions.Add(x.Id, subscription);
                })
            );

            entityChangeSubscriptions.Add(
                observableGroup.EntityRemoved.Subscribe(x => entitySubscriptions.RemoveAndDispose(x.Id))
            );

            foreach (var entity in observableGroup)
            {
                var subscription = processEntityFunction(entity);
                entitySubscriptions.Add(entity.Id, subscription);
            }
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