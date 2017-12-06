using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Numerics;
using Windows.Foundation;
using Windows.System;
using Windows.UI;
using Windows.UI.Input;
using LibraProgramming.Windows.Games.Towers.Core.ServiceContainer;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI;

namespace LibraProgramming.Windows.Games.Towers.GameEngine
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GameplayController
    {
        private readonly EnemyWaveFactory waveFactory;
        private readonly Game game;
//        private readonly IScene scene;
        private readonly EnemyProvider enemyProvider;
//        private MyHomeBase homeBase;
//        private UserPointer userPointer;

        [PrefferedConstructor]
        public GameplayController(IScene scene, EnemyWaveFactory waveFactory)
        {
            this.waveFactory = waveFactory;
            game = new Game(scene);
            enemyProvider = new EnemyProvider(game.Enemies);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Initialize()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public IScene ConfigureScene(Size size)
        {
            /*var waypoints = new Collection<Point>
            {
                new Point(15.0d, 120.0d),
                new Point(500.0d, 120.0d),
                new Point(500.0d, 380.0d),
                new Point(250.0d, 380.0d),
                new Point(250.0d, 585.0d)
            };
            var emitter = new EnemyWaveEmitter(
                waveFactory,
                new Rect(new Point(10.0d, 100.0d), new Point(20.0d, 140.0d)),
                Colors.Black,
                waypoints
            );

            userPointer = new UserPointer(Colors.White);
            homeBase = new MyHomeBase(
                new Rect(new Point(225.0d, 590.0d), new Size(50.0d, 20.0d)),
                Colors.Black,
                2500.0d
            );

            emitter.EnemyCreated += OnEnemyCreated;
            emitter.EnemyKilled += OnEnemyKilled;
            emitter.EnemyReachedEnd += OnEnemyReachedEnd;
            
            scene.Children.Add(new LandscapeMap(size, Colors.DarkBlue, Colors.CadetBlue));
            scene.Children.Add(new WayPath(waypoints, Colors.BurlyWood, Colors.Brown));
            scene.Children.Add(emitter);
            scene.Children.Add(homeBase);
            scene.Children.Add(new MyHomeBaseHealthIndicator(
                homeBase,
                new Rect(new Point(200.0d, 10.0d), new Size(200.0d, 10.0d)),
                Colors.Chartreuse,
                Colors.Chartreuse,
                Colors.Red)
            );
            scene.Children.Add(userPointer);*/

            var scene = game.Scene;
            var transformer = new MapCoordinatesTransformer();
            var pathFinder = new MapPathFinder();
            var origin = new CellPosition(3, 3);
            var enemy = new Enemy(origin, transformer, pathFinder, 250.0d, 0.56f, 1.0d);

            scene.Children.Add(enemy);

            return scene;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Shutdown()
        {
            game.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timing"></param>
        public void Update(CanvasTimingInformation timing)
        {
            game.Scene.Update(timing.ElapsedTime);
        }

        public void DrawScene(CanvasDrawingSession session)
        {
            game.Scene.Draw(session);
        }

        public void PointerMoved(PointerPoint pointer)
        {
//            userPointer.Position = pointer.Position;
        }

        public void PointerEntered()
        {
//            userPointer.IsVisible = true;
        }

        public void PointerExited()
        {
//            userPointer.IsVisible = false;
        }

        public bool PointerPressed(ICanvasResourceCreatorWithDpi creator, VirtualKeyModifiers keyModifiers, PointerPoint pointerPoint)
        {
            /*var position = pointerPoint.Position;
            var tower = new LaserTower(enemyProvider, position.ToVector2(), Colors.Yellow, 0.7d, 150.0d);

            tower.CreateResources(creator, CanvasCreateResourcesReason.FirstTime);
            scene.Children.Add(tower);*/

            return true;
        }

        /*private void OnEnemyCreated(object sender, EnemyEventArgs e)
        {
            enemies.Add(e.Enemy);
        }*/

        /*private void OnEnemyKilled(object sender, EnemyEventArgs e)
        {
            enemies.Remove(e.Enemy);
        }*/

        /*private void OnEnemyReachedEnd(object sender, EnemyEventArgs e)
        {
            homeBase.TakeDamage(e.Enemy.Damage);
            enemies.Remove(e.Enemy);
        }*/

        /// <summary>
        /// 
        /// </summary>
        private class EnemyProvider : IEnemyProvider
        {
            private readonly ICollection<Enemy> enemies;

            public EnemyProvider(ICollection<Enemy> enemies)
            {
                this.enemies = enemies;
            }

            public ICollection<Enemy> GetEnemies()
            {
                return enemies;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private class MapCoordinatesTransformer : ICoordinatesTransformer
        {
            private const float CellHeight = 10.0f;
            private const float CellWidth = 10.0f;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="position"></param>
            /// <returns></returns>
            public Vector2 GetPoint(CellPosition position)
            {
                var point = new Vector2(CellWidth / 2.0f, CellHeight / 2.0f);

                if (position.Column > 0)
                {
                    point.X += CellWidth * (position.Column - 1);
                }

                if (position.Row > 0)
                {
                    point.Y += CellHeight * (position.Row - 1);
                }

                return point;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="point"></param>
            /// <returns></returns>
            public CellPosition GetPosition(Vector2 point)
            {
                var column = (int) (point.X / CellWidth);
                var row = (int) (point.Y / CellHeight);

                if (Math.IEEERemainder(point.X, CellWidth) >= Single.Epsilon)
                {
                    column++;
                }

                if (Math.IEEERemainder(point.Y, CellHeight) >= Single.Epsilon)
                {
                    row++;
                }

                return new CellPosition(column, row);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private class MapPathFinder : IPathFinder
        {
            public CellPosition[] GetWaypoints(CellPosition position)
            {
                var waypoints = new List<CellPosition>
                {
                    new CellPosition(position.Column + 10, position.Row),
                    new CellPosition(position.Column + 10, position.Row + 10),
                    new CellPosition(position.Column, position.Row + 10),
                    new CellPosition(position.Column, position.Row)
                };
                
                return waypoints.ToArray();
            }
        }
    }
}