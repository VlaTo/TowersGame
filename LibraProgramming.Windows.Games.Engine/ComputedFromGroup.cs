using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using LibraProgramming.Windows.Games.Engine.Reactive;
using LibraProgramming.Windows.Games.Engine.Reactive.Extensions;

namespace LibraProgramming.Windows.Games.Engine
{
    public abstract class ComputedFromGroup<TValue> : IComputed<TValue>, IDisposable
    {
        private readonly Subject<TValue> dataChanged;
        private bool needsUpdate;

        public TValue Value => GetData();

        public IObservableGroup ObservableGroup
        {
            get;
        }

        public IList<IDisposable> Subscriptions
        {
            get;
        }

        public TValue CachedData
        {
            get;
            protected set;
        }

        public abstract IObservable<bool> RefreshWhen
        {
            get;
        }

        protected ComputedFromGroup(IObservableGroup observableGroup)
        {
            ObservableGroup = observableGroup;
            Subscriptions = new List<IDisposable>();

            dataChanged = new Subject<TValue>();

            MonitorChanges();
            RefreshData();
        }

        public IDisposable Subscribe(IObserver<TValue> observer)
        {
            return dataChanged.Subscribe(observer);
        }

        public void MonitorChanges()
        {
            Subscriptions.Add(ObservableGroup.EntityAdded.Subscribe(RequestUpdate));
            Subscriptions.Add(ObservableGroup.EntityRemoving.Subscribe(RequestUpdate));
            Subscriptions.Add(RefreshWhen.Subscribe(x => RequestUpdate()));
        }

        public void RequestUpdate(object obj = null)
        {
            needsUpdate = true;

            if (dataChanged.HasObservers)
            {
                RefreshData();
            }
        }

        public abstract TValue Transform(IObservableGroup observableGroup);

        public void RefreshData()
        {
            var newData = Transform(ObservableGroup);

            if (newData.Equals(CachedData))
            {
                return;
            }

            CachedData = newData;

            dataChanged.OnNext(CachedData);
            needsUpdate = false;
        }

        public TValue GetData()
        {
            if (needsUpdate)
            {
                RefreshData();
            }

            return CachedData;
        }

        public void Dispose()
        {
            Subscriptions.DisposeAll();
            dataChanged.Dispose();
        }
    }
}