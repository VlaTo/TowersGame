using System;
using System.Collections.Generic;
using Microsoft.Graphics.Canvas;

namespace LibraProgramming.Windows.Games.Towers.GameEngine
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISceneNode
    {
        /// <summary>
        /// 
        /// </summary>
        ISceneNode Parent
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        ICollection<ISceneNode> Children
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        void Draw(CanvasDrawingSession session);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="elapsed"></param>
        void Update(TimeSpan elapsed);
    }
}