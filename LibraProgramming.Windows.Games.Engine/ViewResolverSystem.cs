using LibraProgramming.Windows.Games.Engine.Reactive.Extensions;

namespace LibraProgramming.Windows.Games.Engine
{
    public abstract class ViewResolverSystem : IViewResolverSystem
    {
        public IGroup Group => new Group(typeof(ViewComponent));

        public IEventSystem EventSystem
        {
            get;
        }

        public abstract IViewHandler ViewHandler
        {
            get;
        }

        protected ViewResolverSystem(IEventSystem eventSystem)
        {
            EventSystem = eventSystem;
        }

        public virtual void Setup(IEntity entity)
        {
            var viewComponent = entity.GetComponent<ViewComponent>();

            if (null != viewComponent.View)
            {
                return;
            }

            viewComponent.View = ViewHandler.CreateView();

            OnViewCreated(entity, viewComponent);
        }

        public virtual void Teardown(IEntity entity)
        {
            var viewComponent = entity.GetComponent<ViewComponent>();
            OnViewRemoved(entity, viewComponent);
        }

        protected abstract void OnViewCreated(IEntity entity, ViewComponent viewComponent);

        protected virtual void OnViewRemoved(IEntity entity, ViewComponent viewComponent) =>
            ViewHandler.DestroyView(viewComponent.View);
    }
}