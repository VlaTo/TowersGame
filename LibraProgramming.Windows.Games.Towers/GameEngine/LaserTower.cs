using System;
using System.Linq;
using System.Numerics;
using Windows.Foundation;
using Windows.UI;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.UI;

namespace LibraProgramming.Windows.Games.Towers.GameEngine
{
    public class LaserTower : StateAwareSceneNode
    {
        private readonly IEnemyProvider enemyProvider;
        private readonly double damage;
        private ICanvasBrush drawBrush;

        public Color Color
        {
            get;
        }

        public Vector2 Position
        {
            get;
        }

        public double Distance
        {
            get;
        }

        public ICanvasResourceCreatorWithDpi Creator
        {
            get;
            private set;
        }

        public LaserTower(IEnemyProvider enemyProvider, Vector2 position, Color color, double damage, double distance)
        {
            this.enemyProvider = enemyProvider;
            this.damage = damage;
            Position = position;
            Color = color;
            Distance = distance;
            State = new EnemySeekState();
        }

        public override void CreateResources(ICanvasResourceCreatorWithDpi creator, CanvasCreateResourcesReason reason)
        {
            Creator = creator;
            drawBrush = new CanvasSolidColorBrush(creator, Color);
        }

        public override void Draw(CanvasDrawingSession session)
        {
            var leftTop = new Point(Position.X - 8.0d, Position.Y - 8.0d);
            var rightBottom = new Point(Position.X + 8.0d, Position.Y + 8.0d);

            session.DrawRectangle(new Rect(leftTop, rightBottom), drawBrush);
            session.DrawLine(leftTop.ToVector2(), rightBottom.ToVector2(), drawBrush);

            var rightTop = new Point(rightBottom.X, leftTop.Y);
            var leftBottom = new Point(leftTop.X, rightBottom.Y);

            session.DrawLine(rightTop.ToVector2(), leftBottom.ToVector2(), drawBrush);

            base.Draw(session);
        }

        /// <summary>
        /// 
        /// </summary>
        private class EnemySeekState : SceneNodeState
        {
            private LaserTower tower;

            public override void Leave(ISceneNode node)
            {
                tower = null;
            }

            public override void Enter(ISceneNode node)
            {
                tower = (LaserTower) node;
            }

            public override void Update(TimeSpan elapsed)
            {
                var enemies = tower.enemyProvider.GetEnemies();

                foreach (var enemy in enemies.Where(entity => entity.IsAlive))
                {
                    var distance = Vector2.Distance(tower.Position, enemy.Position);

                    if (distance > tower.Distance)
                    {
                        continue;
                    }

                    tower.State = new FireState(enemy, TimeSpan.FromMilliseconds(280.0d));

                    break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private class FireState : SceneNodeState
        {
            private readonly Enemy enemy;
            private TimeSpan duration;
            private LaserTower tower;
            private LaserBeam beam;

            public FireState(Enemy enemy, TimeSpan duration)
            {
                this.enemy = enemy;
                this.duration = duration;
            }

            public override void Leave(ISceneNode node)
            {
                tower.Children.Remove(beam);
                tower = null;
            }

            public override void Enter(ISceneNode node)
            {
                beam = new LaserBeam(enemy, Colors.Red);

                tower = (LaserTower) node;
                tower.Children.Add(beam);

                beam.CreateResources(tower.Creator, CanvasCreateResourcesReason.FirstTime);
            }

            public override void Update(TimeSpan elapsed)
            {
                duration -= elapsed;

                if (TimeSpan.Zero <= duration)
                {
                    //enemy.TakeDamage(tower.damage);

                    if (false == enemy.IsAlive)
                    {
                        tower.State = new EnemySeekState();
                    }

                    return;
                }

                tower.State = new FireCooldownState(TimeSpan.FromMilliseconds(500.0d));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private class FireCooldownState : SceneNodeState
        {
            private LaserTower tower;
            private TimeSpan timeout;

            public FireCooldownState(TimeSpan timeout)
            {
                this.timeout = timeout;
            }

            public override void Leave(ISceneNode node)
            {
                tower = null;
            }

            public override void Enter(ISceneNode node)
            {
                tower = (LaserTower)node;
            }

            public override void Update(TimeSpan elapsed)
            {
                timeout -= elapsed;

                if (TimeSpan.Zero >= timeout)
                {
                    tower.State = new EnemySeekState();
                }
            }
        }
    }
}