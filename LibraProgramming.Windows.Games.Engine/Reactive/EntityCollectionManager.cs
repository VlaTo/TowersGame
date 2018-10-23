using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using LibraProgramming.Windows.Games.Engine.Events;
using LibraProgramming.Windows.Games.Engine.Reactive.Extensions;

namespace LibraProgramming.Windows.Games.Engine.Reactive
{
    public class EntityCollectionManager : IEntityCollectionManager, IDisposable
    {
        public const string DefaultPoolName = "default";

        private readonly IDictionary<ObservableGroupToken, IObservableGroup> observableGroups;
        private readonly IDictionary<string, IEntityCollection> collections;
        private readonly IDictionary<string, IDisposable> subscriptions;
        private readonly Subject<CollectionEntityEvent> entityAdded;
        private readonly Subject<CollectionEntityEvent> entityRemoved;
        private readonly Subject<ComponentsChangedEvent> entityComponentsAdded;
        private readonly Subject<ComponentsChangedEvent> entityComponentsRemoving;
        private readonly Subject<ComponentsChangedEvent> entityComponentsRemoved;
        private readonly Subject<IEntityCollection> collectionAdded;
        private readonly Subject<IEntityCollection> collectionRemoved;

        public IEnumerable<IEntityCollection> Collections => collections.Values;

        public IEntityCollectionFactory CollectionFactory
        {
            get;
        }

        public IObservableGroupFactory GroupFactory
        {
            get;
        }

        public IComponentTypeLookup ComponentTypeLookup
        {
            get;
        }

        public IObservable<CollectionEntityEvent> EntityAdded => entityAdded;

        public IObservable<CollectionEntityEvent> EntityRemoved => entityRemoved;

        public IObservable<ComponentsChangedEvent> EntityComponentsAdded => entityComponentsAdded;

        public IObservable<ComponentsChangedEvent> EntityComponentsRemoving => entityComponentsRemoving;

        public IObservable<ComponentsChangedEvent> EntityComponentsRemoved => entityComponentsRemoved;

        public IObservable<IEntityCollection> CollectionAdded => collectionAdded;

        public IObservable<IEntityCollection> CollectionRemoved => collectionRemoved;

        public EntityCollectionManager(IEntityCollectionFactory collectionFactory, IObservableGroupFactory groupFactory,
            IComponentTypeLookup componentTypeLookup)
        {
            CollectionFactory = collectionFactory;
            GroupFactory = groupFactory;
            ComponentTypeLookup = componentTypeLookup;

            observableGroups = new Dictionary<ObservableGroupToken, IObservableGroup>();
            collections = new Dictionary<string, IEntityCollection>();
            subscriptions = new Dictionary<string, IDisposable>();
            entityAdded = new Subject<CollectionEntityEvent>();
            entityRemoved = new Subject<CollectionEntityEvent>();
            entityComponentsAdded = new Subject<ComponentsChangedEvent>();
            entityComponentsRemoving = new Subject<ComponentsChangedEvent>();
            entityComponentsRemoved = new Subject<ComponentsChangedEvent>();
            collectionAdded = new Subject<IEntityCollection>();
            collectionRemoved = new Subject<IEntityCollection>();

            CreateCollection(DefaultPoolName);
        }

        public IEnumerable<IEntity> GetEntitiesFor(IGroup @group, string collectionName = null)
        {
            if (group is EmptyGroup)
            {
                return new IEntity[0];
            }

            if (null != collectionName)
            {
                return collections[collectionName].MatchingGroup(group);
            }

            return Collections.GetAllEntities().MatchingGroup(group);
        }

        public IEnumerable<IEntity> GetEntitiesFor(ILookupGroup lookupGroup, string collectionName = null)
        {
            if (0 == lookupGroup.RequiredComponents.Length && 0 == lookupGroup.ExcludedComponents.Length)
            {
                return new IEntity[0];
            }

            if (null != collectionName)
            {
                return collections[collectionName].MatchingGroup(lookupGroup);
            }

            return Collections.GetAllEntities().MatchingGroup(lookupGroup);
        }

        public IObservableGroup GetObservableGroup(IGroup group, string collectionName = null)
        {
            var requiredComponents = ComponentTypeLookup.GetComponentTypes(group.RequiredComponents);
            var excludedComponents = ComponentTypeLookup.GetComponentTypes(group.ExcludedComponents);
            var lookupGroup = new LookupGroup(requiredComponents, excludedComponents);
            var observableGroupToken = new ObservableGroupToken(lookupGroup, collectionName);

            if (observableGroups.ContainsKey(observableGroupToken))
            {
                return observableGroups[observableGroupToken];
            }

            var entityMatches = GetEntitiesFor(lookupGroup, collectionName);
            var configuration = new ObservableGroupConfiguration
            {
                ObservableGroupToken = observableGroupToken,
                InitialEntities = entityMatches
            };

            if (collectionName != null)
            {
                configuration.NotifyingCollection = collections[collectionName];
            }
            else
            {
                configuration.NotifyingCollection = this;
            }

            var observableGroup = GroupFactory.Create(configuration);

            observableGroups.Add(observableGroupToken, observableGroup);

            return observableGroups[observableGroupToken];
        }

        public void SubscribeToCollection(IEntityCollection collection)
        {
            var disposer = new CompositeDisposable
            {
                collection.EntityAdded.Subscribe(entity => entityAdded.OnNext(entity)),
                collection.EntityRemoved.Subscribe(entity => entityRemoved.OnNext(entity)),
                collection.EntityComponentsAdded.Subscribe(components => entityComponentsAdded.OnNext(components)),
                collection.EntityComponentsRemoving.Subscribe(components => entityComponentsRemoving.OnNext(components)),
                collection.EntityComponentsRemoved.Subscribe(components => entityComponentsRemoved.OnNext(components))
            };

            subscriptions.Add(collection.Name, disposer);
        }

        public void UnsubscribeFromCollection(string collectionName) => subscriptions.RemoveAndDispose(collectionName);

        public IEntityCollection CreateCollection(string name)
        {
            var collection = CollectionFactory.Create(name);

            collections.Add(name, collection);

            SubscribeToCollection(collection);

            collectionAdded.OnNext(collection);

            return collection;
        }

        public IEntityCollection GetCollection(string name) => collections[name ?? DefaultPoolName];

        public void RemoveCollection(string name, bool disposeEntities = true)
        {
            if (false == collections.ContainsKey(name))
            {
                return;
            }

            var collection = collections[name];

            collections.Remove(name);

            UnsubscribeFromCollection(name);

            collectionRemoved.OnNext(collection);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}