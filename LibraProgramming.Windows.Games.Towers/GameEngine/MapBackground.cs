using System;
using System.Threading.Tasks;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.UI;

namespace LibraProgramming.Windows.Games.Towers.GameEngine
{
    public sealed class MapBackground : SceneNode
    {
        private readonly Uri spritesheetPath;
        private ICanvasBrush spritesheetBrush;

        public MapBackground(Uri spritesheetPath = null)
        {
            this.spritesheetPath = spritesheetPath;
        }

        public override async Task CreateResourcesAsync(ICanvasResourceCreatorWithDpi creator, CanvasCreateResourcesReason reason)
        {
            if (null != spritesheetPath)
            {
                var image = await CanvasBitmap.LoadAsync(creator, spritesheetPath);
                spritesheetBrush = new CanvasImageBrush(creator, image);
            }

            await base.CreateResourcesAsync(creator, reason);
        }

        public override void Draw(CanvasDrawingSession session)
        {
            using (var batch=session.)
            {
                
            }
            base.Draw(session);
        }
    }
}