using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;

namespace LibraProgramming.Windows.Games.Engine
{
    public class Entity : IEntity
    {
        private readonly Subject<int[]> componentsAdded;
        private readonly Subject<int[]> componentsRemoving;
        private readonly Subject<int[]> componentsRemoved;

        private bool disposed;

        public int Id
        {
            get;
        }

        public IEnumerable<IComponent> Components => Repository.GetAllFor(Id);

        public IComponentRepository Repository
        {
            get;
        }

        public IObservable<int[]> ComponentsAdded => componentsAdded;

        public IObservable<int[]> ComponentsRemoving => componentsRemoving;

        public IObservable<int[]> ComponentsRemoved => componentsRemoved;

        public Entity(int id, IComponentRepository repository)
        {
            Id = id;
            Repository = repository;

            componentsAdded = new Subject<int[]>();
            componentsRemoving = new Subject<int[]>();
            componentsRemoved = new Subject<int[]>();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public IComponent GetComponent(Type componentType)
        {
            return Repository.Get(Id, componentType);
        }

        public IComponent GetComponent(int componentTypeId)
        {
            return Repository.Get(Id, componentTypeId);
        }

        public void AddComponents(params IComponent[] components)
        {
            var componentTypeIds = new int[components.Length];

            for (var index = 0; index < components.Length; index++)
            {
                componentTypeIds[index] = Repository.Add(Id, components[index]);
            }

            componentsAdded.OnNext(componentTypeIds);
        }

        public bool HasComponent(Type componentType)
        {
            return Repository.Has(Id, componentType);
        }

        public bool HasComponent(int componentTypeId)
        {
            return Repository.Has(Id, componentTypeId);
        }

        public void ClearComponents()
        {
            var componentTypes = Components.Select(x => x.GetType()).ToArray();
            var componentTypeIds = Repository.GetTypesFor(componentTypes);

            RemoveComponents(componentTypeIds);
        }

        public void RemoveComponents(params Type[] componenTypes)
        {
            var componentTypeIds = Repository.GetTypesFor(componenTypes);
            RemoveComponents(componentTypeIds);
        }

        public void RemoveComponents(params int[] componentTypeIds)
        {
            var sanitisedComponentsIds = componentTypeIds.Where(HasComponent).ToArray();

            if (0 == sanitisedComponentsIds.Length)
            {
                return;
            }

            componentsRemoving.OnNext(sanitisedComponentsIds);

            foreach (var componentId in sanitisedComponentsIds)
            {
                Repository.Remove(Id, componentId);
            }

            componentsRemoved.OnNext(sanitisedComponentsIds);
        }

        public override int GetHashCode() => Id;

        private void Dispose(bool dispose)
        {
            if (disposed)
            {
                return;
            }

            try
            {
                if (dispose)
                {
                    componentsAdded.Dispose();
                    componentsRemoved.Dispose();
                }
            }
            finally
            {
                disposed = true;
            }
        }
    }
}