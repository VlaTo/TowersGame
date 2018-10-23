using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using LibraProgramming.Windows.Games.Engine.Reactive;
using LibraProgramming.Windows.Games.Engine.Reactive.Extensions;

namespace LibraProgramming.Windows.Games.Engine
{
    public abstract class ComputedFromData<TInput, TOutput> : IComputed<TOutput>, IDisposable
    {
        private readonly Subject<TOutput> dataChanged;
        private bool needsUpdate;

        public IList<IDisposable> Subscriptions
        {
            get;
        }

        public TOutput Value => GetData();

        public TOutput CachedData
        {
            get;
            private set;
        }

        public TInput DataSource
        {
            get;
        }

        public abstract IObservable<bool> RefreshWhen
        {
            get;
        }
        
        protected ComputedFromData(TInput source)
        {
            DataSource = source;
            Subscriptions = new List<IDisposable>();

            dataChanged = new Subject<TOutput>();

            Initialize();
        }

        public IDisposable Subscribe(IObserver<TOutput> observer) => dataChanged.Subscribe(observer);

        public virtual void MonitorChanges() => Subscriptions.Add(RefreshWhen.Subscribe(x => RequestUpdate()));

        public void RequestUpdate(object obj = null)
        {
            needsUpdate = true;

            if (dataChanged.HasObservers)
            {
                RefreshData();
            }
        }

        public void RefreshData()
        {
            var newData = Transform(DataSource);

            if (newData.Equals(CachedData))
            {
                return;
            }

            CachedData = newData;

            dataChanged.OnNext(CachedData);
            needsUpdate = false;
        }

        public abstract TOutput Transform(TInput dataSource);

        public TOutput GetData()
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

        private void Initialize()
        {
            MonitorChanges();
            RefreshData();
        }
    }
}