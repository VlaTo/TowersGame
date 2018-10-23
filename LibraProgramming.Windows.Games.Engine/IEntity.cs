using System;
using System.Collections.Generic;

namespace LibraProgramming.Windows.Games.Engine
{
    public interface IEntity : IDisposable
    {
        int Id
        {
            get;
        }

        IEnumerable<IComponent> Components
        {
            get;
        }

        IObservable<int[]> ComponentsAdded
        {
            get;
        }

        IObservable<int[]> ComponentsRemoving
        {
            get;
        }

        IObservable<int[]> ComponentsRemoved
        {
            get;
        }

        IComponent GetComponent(Type componentType);

        IComponent GetComponent(int componentTypeId);

        void AddComponents(params IComponent[] components);

        bool HasComponent(Type componentType);

        bool HasComponent(int componentTypeId);

        void ClearComponents();

        void RemoveComponents(params Type[] componenTypes);

        void RemoveComponents(params int[] componentTypeIds);
    }
}