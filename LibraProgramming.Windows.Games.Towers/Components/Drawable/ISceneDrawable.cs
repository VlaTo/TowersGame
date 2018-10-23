using System.Numerics;
using LibraProgramming.Windows.Games.Engine;
using Microsoft.Graphics.Canvas;

namespace LibraProgramming.Windows.Games.Towers.Components.Drawable
{
    public interface ISceneDrawable : IComponent
    {
        void Draw(CanvasDrawingSession drawingSession, Vector2 position);
    }
}