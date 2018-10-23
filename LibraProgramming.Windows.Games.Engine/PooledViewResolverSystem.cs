using LibraProgramming.Windows.Games.Engine.Reactive.Extensions;

namespace LibraProgramming.Windows.Games.Engine
{
    public abstract class PooledViewResolverSystem : IViewResolverSystem, IManualSystem
    {
        public IGroup Group => new Group(typeof(ViewComponent));

        public IEventSystem EventSystem
        {
            get;
        }

        public IViewPool ViewPool
        {
            get;
            private set;
        }

        protected PooledViewResolverSystem(IEventSystem eventSystem)
        {
            EventSystem = eventSystem;
        }

        public void Setup(IEntity entity)
        {
            var viewComponent = entity.GetComponent<ViewComponent>();
            AllocateView(entity, viewComponent);
        }

        public void Start(IObservableGroup @group)
        {
            ViewPool = CreateViewPool();
            OnPoolStarting();
        }

        public void Teardown(IEntity entity)
        {
            var viewComponent = entity.GetComponent<ViewComponent>();
            RecycleView(entity, viewComponent);
        }

        public void Stop(IObservableGroup @group)
        {
            ViewPool.EmptyPool();
        }

        protected abstract IViewPool CreateViewPool();

        protected abstract void OnPoolStarting();

        protected abstract void OnViewRecycled(object view, IEntity entity);

        protected abstract void OnViewAllocated(object view, IEntity entity);

        protected virtual void RecycleView(IEntity entity, ViewComponent viewComponent)
        {
            var view = viewComponent.View;

            ViewPool.ReleaseInstance(view);
            viewComponent.View = null;

            OnViewRecycled(view, entity);
        }

        protected virtual object AllocateView(IEntity entity, ViewComponent viewComponent)
        {
            var viewToAllocate = ViewPool.AllocateInstance();

            viewComponent.View = viewToAllocate;
            OnViewAllocated(viewToAllocate, entity);

            return viewToAllocate;
        }
    }
}