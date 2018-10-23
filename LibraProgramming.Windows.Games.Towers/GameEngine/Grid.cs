using System.Numerics;
using Windows.Foundation;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;

namespace LibraProgramming.Windows.Games.Towers.GameEngine
{
    public class Grid : SceneNode
    {
        private readonly IResourceProvider<ICanvasBrush> brushes;
        private readonly Size cell;
        private readonly Size size;

        public Grid(IResourceProvider<ICanvasBrush> brushes, Size cell, Size size)
        {
            this.brushes = brushes;
            this.cell = cell;
            this.size = size;
        }

        public override void Draw(CanvasDrawingSession session)
        {
            var brush = brushes["grey"];

            using (session.CreateLayer(0.7f))
            {
                session.Antialiasing = CanvasAntialiasing.Antialiased;

                for (var x = 0.0f; x < size.Width; x += (float) cell.Width)
                {
                    session.DrawLine(new Vector2(x, 0.0f), new Vector2(x, (float) size.Height), brush, 1.0f);
                }

                for (var y = 0.0f; y < size.Height; y += (float) cell.Height)
                {
                    session.DrawLine(new Vector2(0.0f, y), new Vector2((float) size.Width, y), brush, 1.0f);
                }
            }
        }
    }
}