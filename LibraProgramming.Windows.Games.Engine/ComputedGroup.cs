using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using LibraProgramming.Windows.Games.Engine.Reactive.Extensions;

namespace LibraProgramming.Windows.Games.Engine
{
    public abstract class ComputedGroup : IComputedGroup, IDisposable
    {
        private readonly Subject<IEntity> entityAdded;
        private readonly Subject<IEntity> entityRemoving;
        private readonly Subject<IEntity> entityRemoved;

        public IObservable<IEntity> EntityAdded => entityAdded;

        public IObservable<IEntity> EntityRemoving => entityRemoving;

        public IObservable<IEntity> EntityRemoved => entityRemoved;

        public IObservableGroup ObservableGroup
        {
            get;
        }
        public abstract IObservable<bool> RefreshWhen
        {
            get;
        }

        public ObservableGroupToken Token => ObservableGroup.Token;

        public IList<IDisposable> Subscriptions
        {
            get;
        }

        public IDictionary<int, IEntity> CachedEntities
        {
            get;
        }

        protected ComputedGroup(IObservableGroup observableGroup)
        {
            ObservableGroup = observableGroup;
            Subscriptions = new List<IDisposable>();
            CachedEntities = new Dictionary<int, IEntity>();

            entityAdded = new Subject<IEntity>();
            entityRemoving = new Subject<IEntity>();
            entityRemoved = new Subject<IEntity>();

            MonitorChanges();
            RefreshEntities();
        }

        public IEnumerator<IEntity> GetEnumerator() => PostProcess(CachedEntities.Values).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool HasEntity(int id) => CachedEntities.ContainsKey(id);

        public void MonitorChanges()
        {
            Subscriptions.Add(ObservableGroup.EntityAdded.Subscribe(OnEntityAddedToGroup));
            Subscriptions.Add(ObservableGroup.EntityRemoving.Subscribe(OnEntityRemovingFromGroup));
            Subscriptions.Add(RefreshWhen.Subscribe(x => RefreshEntities()));
        }

        public void Dispose()
        {
            Subscriptions.DisposeAll();
            entityAdded.Dispose();
            entityRemoved.Dispose();
            entityRemoving.Dispose();
        }

        public abstract bool IsEntityApplicable(IEntity entity);

        public virtual IEnumerable<IEntity> PostProcess(IEnumerable<IEntity> entities) => entities;

        public void RefreshEntities()
        {
            var applicableEntities = ObservableGroup.Where(IsEntityApplicable).ToArray();
            var entitiesToRemove = ObservableGroup.Where(x => applicableEntities.All(y => y.Id != x.Id)).ToArray();
            var entitiesToAdd = applicableEntities.Where(x => !CachedEntities.ContainsKey(x.Id)).ToArray();

            for (var index = entitiesToAdd.Length - 1; index >= 0; index--)
            {
                CachedEntities.Add(entitiesToAdd[index].Id, entitiesToAdd[index]);
                entityAdded.OnNext(entitiesToAdd[index]);
            }

            for (var index = entitiesToRemove.Length - 1; index >= 0; index--)
            {
                entityRemoving.OnNext(entitiesToRemove[index]);
                CachedEntities.Remove(entitiesToRemove[index].Id);
                entityRemoved.OnNext(entitiesToRemove[index]);
            }
        }

        public void OnEntityAddedToGroup(IEntity entity)
        {
            if (false == IsEntityApplicable(entity))
            {
                return;
            }

            CachedEntities.Add(entity.Id, entity);

            entityAdded.OnNext(entity);
        }

        public void OnEntityRemovingFromGroup(IEntity entity)
        {
            if (false == CachedEntities.ContainsKey(entity.Id))
            {
                return;
            }

            entityRemoving.OnNext(entity);

            CachedEntities.Remove(entity.Id);

            entityRemoved.OnNext(entity);
        }
    }
}