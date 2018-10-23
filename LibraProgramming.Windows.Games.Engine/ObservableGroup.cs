using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using LibraProgramming.Windows.Games.Engine.Events;
using LibraProgramming.Windows.Games.Engine.Reactive;
using LibraProgramming.Windows.Games.Engine.Reactive.Extensions;

namespace LibraProgramming.Windows.Games.Engine
{
    public class ObservableGroup : IObservableGroup, IDisposable
    {
        private readonly Subject<IEntity> entityAdded;
        private readonly Subject<IEntity> entityRemoved;
        private readonly Subject<IEntity> entityRemoving;

        public IDictionary<int, IEntity> CachedEntities
        {
            get;
        }
        public IList<IDisposable> Subscriptions
        {
            get;
        }

        public IObservable<IEntity> EntityAdded => entityAdded;

        public IObservable<IEntity> EntityRemoved => entityRemoved;

        public IObservable<IEntity> EntityRemoving => entityRemoving;

        public ObservableGroupToken Token
        {
            get;
        }

        public INotifyingEntityCollection NotifyingCollection
        {
            get;
        }

        public ObservableGroup(ObservableGroupToken token, IEnumerable<IEntity> initialEntities, INotifyingEntityCollection notifyingCollection)
        {
            Token = token;
            NotifyingCollection = notifyingCollection;

            entityAdded = new Subject<IEntity>();
            entityRemoved = new Subject<IEntity>();
            entityRemoving = new Subject<IEntity>();

            Subscriptions = new List<IDisposable>();
            CachedEntities = initialEntities.Where(x => Token.LookupGroup.Matches(x)).ToDictionary(x => x.Id, x => x);

            MonitorEntityChanges();
        }

        public IEnumerator<IEntity> GetEnumerator() => CachedEntities.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool HasEntity(int id) => CachedEntities.ContainsKey(id);

        public void Dispose()
        {
            Subscriptions.DisposeAll();
            entityAdded.Dispose();
            entityRemoved.Dispose();
            entityRemoving.Dispose();
        }

        public void OnEntityComponentRemoved(ComponentsChangedEvent args)
        {
            if (CachedEntities.ContainsKey(args.Entity.Id))
            {
                if (args.Entity.HasAllComponents(Token.LookupGroup.RequiredComponents))
                {
                    return;
                }

                CachedEntities.Remove(args.Entity.Id);
                entityRemoved.OnNext(args.Entity);

                return;
            }

            if (false == Token.LookupGroup.Matches(args.Entity))
            {
                return;
            }

            CachedEntities.Add(args.Entity.Id, args.Entity);

            entityAdded.OnNext(args.Entity);
        }

        public void OnEntityComponentRemoving(ComponentsChangedEvent args)
        {
            if (false == CachedEntities.ContainsKey(args.Entity.Id))
            {
                return;
            }

            if (Token.LookupGroup.ContainsAnyRequiredComponents(args.ComponentTypeIds))
            {
                entityRemoving.OnNext(args.Entity);
            }
        }

        public void OnEntityComponentAdded(ComponentsChangedEvent args)
        {
            if (CachedEntities.ContainsKey(args.Entity.Id))
            {
                if (false == Token.LookupGroup.ContainsAnyExcludedComponents(args.Entity))
                {
                    return;
                }

                entityRemoving.OnNext(args.Entity);

                CachedEntities.Remove(args.Entity.Id);

                entityRemoved.OnNext(args.Entity);

                return;
            }

            if (false == Token.LookupGroup.Matches(args.Entity))
            {
                return;
            }

            CachedEntities.Add(args.Entity.Id, args.Entity);

            entityAdded.OnNext(args.Entity);
        }

        public void OnEntityAddedToCollection(CollectionEntityEvent args)
        {
            // This is because you may have fired a blueprint before it is created
            if (CachedEntities.ContainsKey(args.Entity.Id))
            {
                return;
            }

            if (false == Token.LookupGroup.Matches(args.Entity))
            {
                return;
            }

            CachedEntities.Add(args.Entity.Id, args.Entity);

            entityAdded.OnNext(args.Entity);
        }

        public void OnEntityRemovedFromCollection(CollectionEntityEvent args)
        {
            if (false == CachedEntities.ContainsKey(args.Entity.Id))
            {
                return;
            }

            CachedEntities.Remove(args.Entity.Id);

            entityRemoved.OnNext(args.Entity);
        }

        private void MonitorEntityChanges()
        {
            Subscriptions.Add(NotifyingCollection.EntityAdded.Subscribe(OnEntityAddedToCollection));
            Subscriptions.Add(NotifyingCollection.EntityRemoved.Subscribe(OnEntityRemovedFromCollection));
            Subscriptions.Add(NotifyingCollection.EntityComponentsAdded.Subscribe(OnEntityComponentAdded));
            Subscriptions.Add(NotifyingCollection.EntityComponentsRemoving.Subscribe(OnEntityComponentRemoving));
            Subscriptions.Add(NotifyingCollection.EntityComponentsRemoved.Subscribe(OnEntityComponentRemoved));
        }
    }
}