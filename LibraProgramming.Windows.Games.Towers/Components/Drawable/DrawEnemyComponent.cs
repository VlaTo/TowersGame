using System.Numerics;
using Windows.UI;
using LibraProgramming.Windows.Games.Engine;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;

namespace LibraProgramming.Windows.Games.Towers.Components.Drawable
{
    public class DrawEnemyComponent : ISceneDrawable
    {
        public Color Color
        {
            get;
        }

        public DrawEnemyComponent(Color color)
        {
            Color = color;
        }

        public void Draw(CanvasDrawingSession drawingSession, Vector2 position)
        {
            var brush = new CanvasSolidColorBrush(drawingSession, Color);

            drawingSession.DrawCircle(position, 8.0f, brush);
        }
    }
}