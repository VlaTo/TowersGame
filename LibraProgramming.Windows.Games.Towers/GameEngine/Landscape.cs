using System.Numerics;
using Windows.Foundation;
using Windows.UI;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;

namespace LibraProgramming.Windows.Games.Towers.GameEngine
{
    public sealed class Landscape : SceneNode
    {
        //        private readonly IResourceProvider<CanvasBitmap> bitmaps;
        //private readonly IResourceProvider<ICanvasBrush> brushes;
        private readonly long[,] map;
        private readonly Size size;

        //[PrefferedConstructor]
        public Landscape(long[,] map, Size size)
        {
//            this.bitmaps = bitmaps;
            //this.brushes = brushes;
            this.map = map;
            this.size = size;
        }

        public override void Draw(CanvasDrawingSession session)
        {
            /*var bitmap = bitmaps["terrain"];

            using (session.CreateLayer(1.0f))
            {
                var rect = new Rect(new Point(), new Size(50.0d, 50.0d));
                session.DrawImage(bitmap, rect);
            }*/

            //var brush = brushes["grey"];
            var rows = map.GetLength(0);
            var columns = map.GetLength(1);

            using (session.CreateLayer(0.7f))
            {
                session.Antialiasing = CanvasAntialiasing.Antialiased;

                for (var column = 0; column < columns; column++)
                {
                    var x = column * 25.0f;

                    for (var row = 0; row < rows; row++)
                    {
                        if (0 <= map[row, column])
                        {
                            continue;
                        }

                        var y = row * 25.0f;

                        session.FillRectangle(new Rect(x, y, 25.0d, 25.0d), Colors.Aqua);
                    }
                }
            }
        }
    }
}