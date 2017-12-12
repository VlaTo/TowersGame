using System;
using Windows.Foundation;
using LibraProgramming.Windows.Games.Towers.Core.ServiceContainer;
using Microsoft.Graphics.Canvas;

namespace LibraProgramming.Windows.Games.Towers.GameEngine
{
    /// <summary>
    /// 
    /// </summary>
    public class Scene : SceneNode, IScene
    {
        public ResourcesLoader Resources
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourcesLoader"></param>
        [PrefferedConstructor]
        public Scene(ResourcesLoader resourcesLoader)
        {
            Resources = resourcesLoader;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="creator"></param>
        /// <returns></returns>
        public IAsyncAction CreateResourcesAsync(ICanvasResourceCreatorWithDpi creator)
        {
            return Resources.CreateAsync(creator).AsAsyncAction();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
        }
    }
}