using System;
using Windows.Foundation;
using Microsoft.Graphics.Canvas;

namespace LibraProgramming.Windows.Games.Towers.GameEngine
{
    /// <summary>
    /// 
    /// </summary>
    public interface IScene : ISceneNode, IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="creator"></param>
        /// <returns></returns>
        IAsyncAction CreateResourcesAsync(ICanvasResourceCreatorWithDpi creator);
    }
}