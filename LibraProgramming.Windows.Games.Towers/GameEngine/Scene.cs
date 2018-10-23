using Windows.ApplicationModel.Store.Preview.InstallControl;
using Windows.Foundation;
using Microsoft.Graphics.Canvas;

namespace LibraProgramming.Windows.Games.Towers.GameEngine
{
    /// <summary>
    /// 
    /// </summary>
    public class Scene : SceneNode, IScene
    {
        public void SetController(GameplayController value)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
        }

        public IAsyncAction CreateResourcesAsync(ICanvasResourceCreatorWithDpi creator)
        {
            throw new System.NotImplementedException();
        }
    }
}