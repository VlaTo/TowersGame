using System;
using System.Collections;
using System.Collections.Generic;
using System.Reactive.Subjects;
using LibraProgramming.Windows.Games.Engine.Events;
using LibraProgramming.Windows.Games.Engine.Reactive.Extensions;

namespace LibraProgramming.Windows.Games.Engine.Reactive
{
    public class EntityCollection : IEntityCollection, IDisposable
    {
        private readonly Subject<CollectionEntityEvent> entityAdded;
        private readonly Subject<CollectionEntityEvent> entityRemoved;
        private readonly Subject<ComponentsChangedEvent> entityComponentsAdded;
        private readonly Subject<ComponentsChangedEvent> entityComponentsRemoving;
        private readonly Subject<ComponentsChangedEvent> entityComponentsRemoved;

        public IDictionary<int, IEntity> EntityLookup
        {
            get;
        }

        public IDictionary<int, IDisposable> EntitySubscriptions
        {
            get;
        }

        public string Name
        {
            get;
        }

        public IEntityFactory EntityFactory
        {
            get;
        }

        public IObservable<CollectionEntityEvent> EntityAdded => entityAdded;

        public IObservable<CollectionEntityEvent> EntityRemoved => entityRemoved;

        public IObservable<ComponentsChangedEvent> EntityComponentsAdded => entityComponentsAdded;

        public IObservable<ComponentsChangedEvent> EntityComponentsRemoving => entityComponentsRemoving;

        public IObservable<ComponentsChangedEvent> EntityComponentsRemoved => entityComponentsRemoved;

        public EntityCollection(string name, IEntityFactory factory)
        {
            EntityLookup = new Dictionary<int, IEntity>();
            EntitySubscriptions = new Dictionary<int, IDisposable>();
            Name = name;
            EntityFactory = factory;

            entityAdded = new Subject<CollectionEntityEvent>();
            entityRemoved = new Subject<CollectionEntityEvent>();
            entityComponentsAdded = new Subject<ComponentsChangedEvent>();
            entityComponentsRemoving = new Subject<ComponentsChangedEvent>();
            entityComponentsRemoved = new Subject<ComponentsChangedEvent>();
        }

        public void SubscribeToEntity(IEntity entity)
        {
            var disposer = new CompositeDisposable
            {
                entity.ComponentsAdded.Subscribe(ids =>
                    entityComponentsAdded.OnNext(new ComponentsChangedEvent(entity, this, ids))
                ),
                entity.ComponentsRemoving.Subscribe(ids =>
                    entityComponentsRemoving.OnNext(new ComponentsChangedEvent(entity, this, ids))
                ),
                entity.ComponentsRemoved.Subscribe(ids =>
                    entityComponentsRemoved.OnNext(new ComponentsChangedEvent(entity, this, ids))
                )
            };

            EntitySubscriptions.Add(entity.Id, disposer);
        }

        public void UnsubscribeFromEntity(int entityId) => EntitySubscriptions.RemoveAndDispose(entityId);

        public IEnumerator<IEntity> GetEnumerator() => EntityLookup.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEntity CreateEntity(IBlueprint blueprint = null)
        {
            var entity = EntityFactory.Create(null);

            EntityLookup.Add(entity.Id, entity);

            entityAdded.OnNext(new CollectionEntityEvent(entity, this));

            SubscribeToEntity(entity);

            blueprint?.Apply(entity);

            return entity;
        }

        public IEntity GetEntity(int id) => EntityLookup[id];

        public void AddEntity(IEntity entity)
        {
            EntityLookup.Add(entity.Id, entity);

            entityAdded.OnNext(new CollectionEntityEvent(entity, this));

            SubscribeToEntity(entity);
        }

        public void RemoveEntity(int id, bool disposeOnRemoval = true)
        {
            var entity = GetEntity(id);

            EntityLookup.Remove(id);

            var entityId = entity.Id;

            if (disposeOnRemoval)
            {
                entity.Dispose();
                EntityFactory.Destroy(entityId);
            }

            UnsubscribeFromEntity(entityId);

            entityRemoved.OnNext(new CollectionEntityEvent(entity, this));
        }

        public bool ContainsEntity(int entityId) => EntityLookup.ContainsKey(entityId);

        public void RemoveEntity(IEntity entity)
        {
            RemoveEntity(entity.Id, false);
        }

        public void Dispose()
        {
            entityAdded.Dispose();
            entityRemoved.Dispose();
            entityComponentsAdded.Dispose();
            entityComponentsRemoving.Dispose();
            entityComponentsRemoved.Dispose();

            EntityLookup.Clear();
            EntitySubscriptions.RemoveAndDisposeAll();
        }
    }
}