using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using Windows.Foundation;
using Windows.UI;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.UI;

namespace LibraProgramming.Windows.Games.Towers.GameEngine
{
    public class Enemy : StateAwareSceneNode
    {
        private static readonly Size HealthBarSize = new Size(18.0d, 4.0d);

        private readonly double healthAmount;
        private readonly double speed;
        private double health;

        public double Angle
        {
            get;
            protected set;
        }

        public TimeSpan Age
        {
            get;
            private set;
        }

        public ICollection<Point> Waypoints
        {
            get;
        }

        public Vector2 Position
        {
            get;
            private set;
        }

        public double Damage
        {
            get;
        }

        public double Health
        {
            get => health;
            private set => health = Math.Max(0.0d, value);
        }

        public bool IsAlive => 0.0d < Health;

        protected ICanvasBrush DrawBrush
        {
            get;
            set;
        }

        protected ICanvasBrush FillBrush
        {
            get;
            set;
        }

        protected ICanvasBrush HealthBarBrush
        {
            get;
            set;
        }

        protected ICanvasBrush GoodHealthBrush
        {
            get;
            set;
        }

        protected ICanvasBrush PoorHealthBrush
        {
            get;
            set;
        }

        public Enemy(Point origin, double health, ICollection<Point> waypoints, double speed, double damage)
        {
            this.health = health;
            this.speed = speed;
            healthAmount = health;
            Angle = 0.0d;
            Position = origin.ToVector2();
            Waypoints = waypoints;
            Damage = damage;
            State = new StartMovementState(0);
        }

        public override void Update(TimeSpan elapsed)
        {
            if (null == Waypoints)
            {
                return;
            }

            Age += elapsed;

            base.Update(elapsed);
        }

        public override void Draw(CanvasDrawingSession session)
        {
            var direction = new Point(Math.Cos(Angle), Math.Sin(Angle));
            var end = Position + direction.ToVector2() * 11.0f;

            session.DrawCircle(Position, 8.0f, DrawBrush);
            session.FillCircle(Position, 8.0f, FillBrush);
            session.DrawLine(Position, end, DrawBrush);

            DrawHealthBar(session);
        }

        public override void CreateResources(ICanvasResourceCreatorWithDpi creator, CanvasCreateResourcesReason reason)
        {
            DrawBrush = new CanvasSolidColorBrush(creator, Colors.White);
            FillBrush = new CanvasSolidColorBrush(creator, Colors.Gray);
            HealthBarBrush = new CanvasSolidColorBrush(creator, Colors.Chartreuse);
            GoodHealthBrush = new CanvasSolidColorBrush(creator, Colors.Chartreuse);
            PoorHealthBrush = new CanvasSolidColorBrush(creator, Colors.OrangeRed);
        }

        public void TakeDamage(double value)
        {
            Health -= value;

            if (false == IsAlive)
            {
                State = new KilledState();
            }
        }

        public void ReportDamage()
        {
            var emitter = (EnemyWaveEmitter) Parent;
            emitter.ReportEnemyReachedEnd(this);
        }

        public void Die()
        {
            var emitter = (EnemyWaveEmitter)Parent;
            emitter.ReportDie(this);
        }

        protected void DrawHealthBar(CanvasDrawingSession session)
        {
            var percentage = Math.Min(Math.Max(health / healthAmount, 0.0f), 1.0f);
            var width = HealthBarSize.Width * percentage;
            var point0 = new Point(Position.X - HealthBarSize.Width / 2.0d, Position.Y + 12.0d);

            session.DrawRectangle(new Rect(point0, HealthBarSize), HealthBarBrush);
            session.FillRectangle(
                new Rect(point0, new Size(width, HealthBarSize.Height)),
                percentage >= 0.4d ? GoodHealthBrush : PoorHealthBrush
            );
        }

        /// <summary>
        /// 
        /// </summary>
        private class StartMovementState : SceneNodeState
        {
            private readonly int index;
            private Enemy enemy;

            public StartMovementState(int index)
            {
                this.index = index;
            }

            public override void Leave(ISceneNode node)
            {
                enemy = null;
            }

            public override void Enter(ISceneNode node)
            {
                enemy = (Enemy)node;
            }

            public override void Update(TimeSpan elapsed)
            {
                if (0 == enemy.Waypoints.Count)
                {
                    enemy.State = Empty;
                    return;
                }

                if (index >= enemy.Waypoints.Count)
                {
                    enemy.State = new ReachedEndState();
                    return;
                }

                var point = GetWaypoint(enemy.Waypoints, index);

                enemy.State = new WaypointMovementState(point.ToVector2(), index);
            }

            private static Point GetWaypoint(IEnumerable<Point> waypoints, int number)
            {
                foreach (var waypoint in waypoints)
                {
                    if (0 < number--)
                    {
                        continue;
                    }

                    return waypoint;
                }

                throw new InvalidDataException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private class WaypointMovementState : SceneNodeState
        {
            private readonly int index;
            private readonly Vector2 destination;
            private Enemy enemy;

            public WaypointMovementState(Vector2 destination, int index)
            {
                this.destination = destination;
                this.index = index;
            }

            public override void Leave(ISceneNode node)
            {
                enemy = null;
            }

            public override void Enter(ISceneNode node)
            {
                enemy = (Enemy) node;
                enemy.Angle = Math.Atan2(destination.Y - enemy.Position.Y, destination.X - enemy.Position.X);
            }

            public override void Update(TimeSpan elapsed)
            {
                if (1.0f >= Vector2.Distance(enemy.Position, destination))
                {
                    enemy.State = new StartMovementState(index + 1);
                    return;
                }

                var direction = new Point(Math.Cos(enemy.Angle), Math.Sin(enemy.Angle));

                enemy.Position += direction.ToVector2() * (float) enemy.speed;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private class ReachedEndState : SceneNodeState
        {
            private Enemy enemy;

            public override void Leave(ISceneNode node)
            {
                enemy = null;
            }

            public override void Enter(ISceneNode node)
            {
                enemy = (Enemy) node;
            }

            public override void Update(TimeSpan elapsed)
            {
                enemy.ReportDamage();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private class KilledState : SceneNodeState
        {
            private Enemy enemy;

            public override void Leave(ISceneNode node)
            {
                enemy = null;
            }

            public override void Enter(ISceneNode node)
            {
                enemy = (Enemy)node;
            }

            public override void Update(TimeSpan elapsed)
            {
                enemy.Die();
            }
        }
    }
}