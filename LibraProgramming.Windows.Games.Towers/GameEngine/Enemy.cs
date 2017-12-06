using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private readonly float speed;
        private readonly ICoordinatesTransformer coordinatesTransformer;
        private readonly IPathFinder pathFinder;
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

        public CellPosition Origin => coordinatesTransformer.GetPosition(Position);

        public Vector2 Position
        {
            get;
            set;
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

        public Enemy(
            CellPosition origin,
            ICoordinatesTransformer coordinatesTransformer,
            IPathFinder pathFinder,
            double health,
            float speed,
            double damage)
        {
            this.coordinatesTransformer = coordinatesTransformer;
            this.pathFinder = pathFinder;
            this.health = health;
            this.speed = speed;

            healthAmount = health;

            Angle = 0.0d;
            Damage = damage;
            Position = coordinatesTransformer.GetPoint(origin);
            State = new CalculateWaypointsState();
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

        /*public void TakeDamage(double value)
        {
            Health -= value;

            if (false == IsAlive)
            {
                State = new KilledState();
            }
        }*/

        public override void CreateResources(ICanvasResourceCreatorWithDpi creator, CanvasCreateResourcesReason reason)
        {
            DrawBrush = new CanvasSolidColorBrush(creator, Colors.White);
            FillBrush = new CanvasSolidColorBrush(creator, Colors.Gray);
            HealthBarBrush = new CanvasSolidColorBrush(creator, Colors.Chartreuse);
            GoodHealthBrush = new CanvasSolidColorBrush(creator, Colors.Chartreuse);
            PoorHealthBrush = new CanvasSolidColorBrush(creator, Colors.OrangeRed);
        }

        /*public void ReportDamage()
        {
            var emitter = (EnemyWaveEmitter) Parent;
            emitter.ReportEnemyReachedEnd(this);
        }*/

        /*public void Die()
        {
            var emitter = (EnemyWaveEmitter) Parent;
            emitter.ReportDie(this);
        }*/

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
        private class CalculateWaypointsState : SceneNodeState
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
                var origin = enemy.Origin;
                var waypoints = enemy.pathFinder.GetWaypoints(origin);

                enemy.State = new FindNextPositionState(waypoints, 0);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private class FindNextPositionState : SceneNodeState
        {
            private readonly CellPosition[] waypoints;
            private readonly int index;
            private Enemy enemy;

            public FindNextPositionState(CellPosition[] waypoints, int index)
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
                enemy.State = null == waypoints || index >= waypoints.Length
                    ? Empty
                    : new MoveToPositionState(waypoints, index);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private class MoveToPositionState : SceneNodeState
        {
            private readonly CellPosition[] waypoints;
            private readonly int index;
            private Enemy enemy;

            public MoveToPositionState(CellPosition[] waypoints, int index)
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
                var position = waypoints[index];
                var point = enemy.coordinatesTransformer.GetPoint(position);
                var angle = Math.Atan2(point.Y - enemy.Position.Y, point.X - enemy.Position.X);
//                var temp = Math.Atan2(Math.Abs(point.Y - enemy.Position.Y), Math.Abs(point.X - enemy.Position.X));

                var delta = enemy.Angle - angle;

                if (0.0d < delta || delta < 0.0d)
                {
                    enemy.State = new EnemyRotatingState(angle, waypoints, index);
                }
                else
                {
                    enemy.State = new EnemyMovingState(waypoints, index);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private class EnemyRotatingState : SceneNodeState
        {
            private static readonly double pi34 = Math.PI * 1.5d;
            private static readonly double pi2 = Math.PI * 0.5d;

            private readonly double angle;
            private readonly CellPosition[] waypoints;
            private readonly int index;
            private Enemy enemy;

            public EnemyRotatingState(double angle, CellPosition[] waypoints, int index)
            {
                this.angle = angle;
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
                const double delta = 0.01d;
                var distance = Math.Abs(angle - enemy.Angle);

                if (delta >= distance)
                {
                    enemy.Angle = angle;
                    enemy.State = new EnemyMovingState(waypoints, index);

                    return;
                }

                enemy.Angle += delta;
            }
        }

/*
        /// <summary>
        /// 
        /// </summary>
        private class AntiClockwiseRotatingState : SceneNodeState
        {
            private readonly double delta;
            private readonly CellPosition[] waypoints;
            private readonly int index;
            private Enemy enemy;

            public AntiClockwiseRotatingState(double delta, CellPosition[] waypoints, int index)
            {
                this.delta = delta;
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
                const double angle = 0.05d;

                if (Double.Epsilon > (delta - angle))
                {
                    enemy.Angle -= angle;
                }
                else
                {
                    enemy.State = Empty;
                }
            }
        }
*/

        /// <summary>
        /// 
        /// </summary>
        private class EnemyMovingState : SceneNodeState
        {
            private readonly CellPosition[] waypoints;
            private readonly int index;
            private Enemy enemy;

            public EnemyMovingState(CellPosition[] waypoints, int index)
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
                var position = waypoints[index];
                var destination = enemy.coordinatesTransformer.GetPoint(position);

                if (1.0f >= Vector2.Distance(enemy.Position, destination))
                {
                    enemy.State = new FindNextPositionState(waypoints, index + 1);
                }
                else
                {
                    var direction = new Point(Math.Cos(enemy.Angle), Math.Sin(enemy.Angle));
                    enemy.Position += direction.ToVector2() * enemy.speed;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /*private class MoveToNextWaypointState : SceneNodeState
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
        }*/


        /// <summary>
        /// 
        /// </summary>
        /*private class MoveToWaypointState : SceneNodeState
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
        }*/
    }
}