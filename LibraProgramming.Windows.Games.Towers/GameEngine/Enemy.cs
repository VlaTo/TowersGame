using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private readonly ITargetProvider targetProvider;
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

        public Enemy(Vector2 origin, ITargetProvider targetProvider, double health, double speed, double damage)
        {
            this.targetProvider = targetProvider;
            this.health = health;
            this.speed = speed;
            healthAmount = health;
            Angle = 0.0d;
            Position = origin;
            Damage = damage;
            State = new CalculatePathState();
        }

        public override void Update(TimeSpan elapsed)
        {
            if (false == IsAlive)
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
//                State = new KilledState();
            }
        }

        public void ReportDamage()
        {
            var emitter = (EnemyWaveEmitter) Parent;
            emitter.ReportEnemyReachedEnd(this);
        }

        public void Die()
        {
            var emitter = (EnemyWaveEmitter) Parent;
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
        private class CalculatePathState : SceneNodeState
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
                var targetProvider = enemy.targetProvider;
                var waypoints = targetProvider.GetWaypoints(enemy.Position);
                enemy.State = new StartMovingState(waypoints);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private class StartMovingState : SceneNodeState
        {
            private readonly IList<Vector2> waypoints;
            private Enemy enemy;

            public StartMovingState(IList<Vector2> waypoints)
            {
                this.waypoints = waypoints;
            }

            public override void Leave(ISceneNode node)
            {
                enemy = null;
            }

            public override void Enter(ISceneNode node)
            {
                enemy = (Enemy) node;
                //enemy.Angle = Math.Atan2(destination.Y - enemy.Position.Y, destination.X - enemy.Position.X);
            }

            public override void Update(TimeSpan elapsed)
            {
                /*if (1.0f >= Vector2.Distance(enemy.Position, destination))
                {
                    enemy.State = new CalculatePathState(index + 1);
                    return;
                }

                var direction = new Point(Math.Cos(enemy.Angle), Math.Sin(enemy.Angle));

                enemy.Position += direction.ToVector2() * (float) enemy.speed;*/

                if (false == waypoints.Any())
                {
                    enemy.State = Empty;
                    return;
                }

                enemy.State = new MoveToNextWaypointState(waypoints, 0);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private class MoveToNextWaypointState : SceneNodeState
        {
            private readonly IList<Vector2> waypoints;
            private readonly int index;
            private Enemy enemy;

            public MoveToNextWaypointState(IList<Vector2> waypoints, int index)
            {
                this.waypoints = waypoints;
                this.index = index;
            }

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
                if (index >= waypoints.Count)
                {
                    return;
                }

                var waypoint = waypoints[index];
                var angle = Math.Atan2(waypoint.Y - enemy.Position.Y, waypoint.X - enemy.Position.X);

                enemy.State = new RotatingState(angle, waypoints, index + 1);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private class RotatingState : SceneNodeState
        {
            private readonly double targetAngle;
            private readonly IList<Vector2> waypoints;
            private readonly int nextIndex;
            private Enemy enemy;
            private double delta;
            private double angle;

            public RotatingState(double targetAngle, IList<Vector2> waypoints, int nextIndex)
            {
                this.targetAngle = targetAngle;
                this.waypoints = waypoints;
                this.nextIndex = nextIndex;
            }

            public override void Leave(ISceneNode node)
            {
                enemy = null;
            }

            public override void Enter(ISceneNode node)
            {
                enemy = (Enemy) node;

                if (Single.Epsilon >= (enemy.Angle - targetAngle))
                {
                    enemy.State = new MoveToWaypointState(waypoints, nextIndex);
                }
                else
                {
                    delta = enemy.Angle > targetAngle ? 0.05d : -0.05d;
                }
            }

            public override void Update(TimeSpan elapsed)
            {
                enemy.Angle += delta;

                if (Double.Epsilon >= (enemy.Angle - targetAngle))
                {
                    enemy.State = new MoveToWaypointState(waypoints, nextIndex);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private class MoveToWaypointState : SceneNodeState
        {
            private readonly IList<Vector2> waypoints;
            private readonly int index;
            private Enemy enemy;
            private Vector2 destination;

            public MoveToWaypointState(IList<Vector2> waypoints, int index)
            {
                this.waypoints = waypoints;
                this.index = index;
            }

            public override void Leave(ISceneNode node)
            {
                enemy = null;
            }

            public override void Enter(ISceneNode node)
            {
                enemy = (Enemy) node;
                destination = waypoints[index];
            }

            public override void Update(TimeSpan elapsed)
            {
                if (1.0f >= Vector2.Distance(enemy.Position, destination))
                {
                    enemy.State = new MoveToNextWaypointState(waypoints, index + 1);
                    return;
                }

                var direction = new Point(Math.Cos(enemy.Angle), Math.Sin(enemy.Angle));

                enemy.Position += direction.ToVector2() * (float) enemy.speed;
            }
        }
    }
}