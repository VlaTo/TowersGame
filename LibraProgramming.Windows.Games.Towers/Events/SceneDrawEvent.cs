using Microsoft.Graphics.Canvas;

namespace LibraProgramming.Windows.Games.Towers.Events
{
    public class SceneDrawEvent
    {
        public CanvasDrawingSession DrawingSession
        {
            get;
        }

        public SceneDrawEvent(CanvasDrawingSession drawingSession)
        {
            DrawingSession = drawingSession;
        }
    }
}