using System.Collections.Generic;
using System.Linq;
using LibraProgramming.Windows.Games.Engine.Infrastructure.Dependencies;
using LibraProgramming.Windows.Games.Engine.Infrastructure.Extensions;
using LibraProgramming.Windows.Games.Engine.Reactive;

namespace LibraProgramming.Windows.Games.Engine.Infrastructure
{
    public abstract class Application : IApplication
    {
        private readonly IList<IPlugin> plugins;

        public ISystemExecutor Executor
        {
            get;
            private set;
        }

        public IEventSystem EventSystem
        {
            get;
            private set;
        }

        public IEntityCollectionManager EntityCollectionManager
        {
            get;
            private set;
        }

        public IEnumerable<IPlugin> Plugins => plugins;

        public abstract IDependencyContainer Container
        {
            get;
        }

        protected Application()
        {
            plugins = new List<IPlugin>();
        }

        public void StartApplication()
        {
            RegisterModules();
            ApplicationStarting();
            RegisterAllPluginDependencies();
            SetupAllPluginSystems();
            ApplicationStarted();
        }

        protected virtual IDependencyModule GetFrameworkModule()
        {
            return new FrameworkModule();
        }

        protected virtual void RegisterModules()
        {
            Container.LoadModule(GetFrameworkModule());
            Executor = Container.Resolve<ISystemExecutor>();
            EventSystem = Container.Resolve<IEventSystem>();
            EntityCollectionManager = Container.Resolve<IEntityCollectionManager>();
        }

        protected virtual void ApplicationStarting()
        {
        }

        protected abstract void ApplicationStarted();

        protected virtual void RegisterAllPluginDependencies()
        {
            foreach (var plugin in plugins)
            {
                plugin.SetupDependencies(Container);
            }
        }

        protected virtual void SetupAllPluginSystems()
        {
            var systems = plugins.SelectMany(x => x.GetSystemsForRegistration(Container));

            foreach (var system in systems)
            {
                Executor.AddSystem(system);
            }
        }

        protected void RegisterPlugin(IPlugin plugin) => plugins.Add(plugin);

        protected virtual void RegisterAllBoundSystems()
        {
            var allSystems = Container.ResolveAll<ISystem>();
            var orderedSystems = allSystems
                .OrderByDescending(x => x is ViewResolverSystem)
                .ThenByDescending(x => x is ISetupSystem);

            foreach (var system in orderedSystems)
            {
                Executor.AddSystem(system);
            }
        }

        protected virtual void RegisterSystem<TSystem>()
            where TSystem : ISystem
        {
            var system = Container.Resolve<TSystem>();
            Executor.AddSystem(system);
        }
    }
}