using System.Collections.Generic;
using LibraProgramming.Windows.Games.Engine.Infrastructure.Dependencies;
using LibraProgramming.Windows.Games.Engine.Reactive;

namespace LibraProgramming.Windows.Games.Engine.Infrastructure
{
    public interface IApplication
    {
        IDependencyContainer Container
        {
            get;
        }

        /// <summary>
        /// The system executor, this orchestrates the systems
        /// </summary>
        ISystemExecutor Executor
        {
            get;
        }

        /// <summary>
        /// The event system to publish and subscribe to events
        /// </summary>
        IEventSystem EventSystem
        {
            get;
        }

        /// <summary>
        /// The entity collection manager, allows you to create entity collections and observable groups
        /// </summary>
        IEntityCollectionManager EntityCollectionManager
        {
            get;
        }

        /// <summary>
        /// Any plugins which have been registered within the application
        /// </summary>
        IEnumerable<IPlugin> Plugins
        {
            get;
        }

        void StartApplication();
    }
}