using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.UI;

namespace LibraProgramming.Windows.Games.Towers.GameEngine
{
    public class Seeker : StateAwareSceneNode
    {
        private readonly Color borderColor;
        private readonly Color fillColor;
        private readonly float speed;
        private ICanvasBrush borderBrush;
        private ICanvasBrush fillBrush;

        public double Angle
        {
            get;
            protected set;
        }

        public Vector2 Position
        {
            get;
            private set;
        }

        public Seeker(Vector2 position, float speed, Color borderColor, Color fillColor)
        {
            this.borderColor = borderColor;
            this.fillColor = fillColor;

            this.speed = speed;

            Position = position;
        }

        public void MoveTo(Vector2 destination)
        {
            State = new MoveToState(destination);
        }

        public void LookAt(Vector2 target)
        {
            Angle = Math.Atan2(target.Y - Position.Y, target.X - Position.X);
        }

        public override void CreateResources(ICanvasResourceCreatorWithDpi creator, CanvasCreateResourcesReason reason)
        {
            borderBrush = new CanvasSolidColorBrush(creator, borderColor);
            fillBrush = new CanvasSolidColorBrush(creator, fillColor);
        }

        public override void Draw(CanvasDrawingSession session)
        {
            var direction = new Point(Math.Cos(Angle), Math.Sin(Angle));
            var end = Position + direction.ToVector2() * 11.0f;

            session.DrawCircle(Position, 8.0f, borderBrush);
            session.FillCircle(Position, 8.0f, fillBrush);
            session.DrawLine(Position, end, borderBrush);

            base.Draw(session);

            var point0 = new Point(Position.X - 100.0f / 2.0d, Position.Y + 22.0d);

            session.DrawText(Angle.ToString("F1"), point0.ToVector2(), borderColor);
        }

        /// <summary>
        /// 
        /// </summary>
        private class MoveToState : SceneNodeState
        {
            private readonly Vector2 destination;
            private Seeker seeker;

            public MoveToState(Vector2 destination)
            {
                this.destination = destination;
            }

            public override void Leave(ISceneNode node)
            {
                seeker = null;
            }

            public override void Enter(ISceneNode node)
            {
                seeker = (Seeker) node;
            }

            public override void Update(TimeSpan elapsed)
            {
                if (1.0f >= Vector2.Distance(seeker.Position, destination))
                {
                    seeker.State = Empty;
                    return;
                }

                var direction = new Point(Math.Cos(seeker.Angle), Math.Sin(seeker.Angle));

                seeker.Position += direction.ToVector2() * seeker.speed;
            }
        }
    }
}
