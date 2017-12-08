using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Numerics;
using Windows.Foundation;
using Windows.System;
using Windows.UI;
using Windows.UI.Input;
using Windows.UI.Xaml.Controls;
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
//        private Seeker seeker;

        [PrefferedConstructor]
        public GameplayController(IScene scene, EnemyWaveFactory waveFactory)
        {
            this.waveFactory = waveFactory;
            game = new Game(scene)
            {
                MapSize = new MapSize(40, 30)
            };
            enemyProvider = new EnemyProvider(game.Enemies);
            scene.SetController(this);
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

            /*seeker = new Seeker(new Vector2(100.0f, 100.0f), 0.85f, Colors.White, Colors.DarkSlateGray);
            userPointer = new UserPointer(Colors.White);

            scene.Children.Add(seeker);
            scene.Children.Add(userPointer);*/

            var transformer = new MapCoordinatesTranslator();
            var pathFinder = new MapPathFinder(game);
            var origin = new Position(3, 3);
            var enemy = new Enemy(origin, transformer, pathFinder, 250.0f, 0.56f, 1.0f);

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
            /*userPointer.Position = pointer.Position;
            seeker.LookAt(pointer.Position.ToVector2());*/
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

            seeker.MoveTo(position.ToVector2());*/

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
        private class MapCoordinatesTranslator : ICoordinatesTranslator
        {
            private const float CellHeight = 10.0f;
            private const float CellWidth = 10.0f;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="position"></param>
            /// <returns></returns>
            public Vector2 GetPoint(Position position)
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
            public Position GetPosition(Vector2 point)
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

                return new Position(column, row);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private class MapPathFinder : IPathFinder
        {
            private readonly Game game;
            private long [,] heatMap;

            public MapPathFinder(Game game)
            {
                this.game = game;
            }

            public IReadOnlyCollection<Position> GetWaypoints(Position position)
            {
                var map = GetGameHeatMap();

                TraceMap(map, position, new Position(20, 30));

                var waypoints = new List<Position>
                {
                    new Position(position.Column + 10, position.Row),
                    new Position(position.Column + 10, position.Row + 10),
                    new Position(position.Column, position.Row + 10),
                    new Position(position.Column, position.Row)
                };
                
                return waypoints.ToArray();
            }

            private long[,] GetGameHeatMap()
            {
                if (null != heatMap)
                {
                    return heatMap;
                }

                var size = game.MapSize;

                heatMap = new long[size.Rows, size.Columns];

                for (var row = 0; row < size.Rows; row++)
                {
                    for (var column = 0; column < size.Columns; column++)
                    {
                        heatMap[row, column] = size.Columns * row + column + 1;
                    }
                }
                
                return heatMap;
            }

            private static void TraceMap(long[,] map, Position origin, Position target)
            {
                var queue = new Queue<Position>();
                var number = 0L;

                queue.Enqueue(origin);

                //var rows = map.GetLength(0);
                var columns = map.GetLength(1);

                var getRow = new Func<int, int>(index => index / columns);
                var getColumn = new Func<int, int>(index => index % columns);

                while (0 < queue.Count)
                {
                    var neighbors = GeNeighbors(map, queue.Dequeue());

                    for (var index = 0; index < neighbors.Length; index++)
                    {
                        if (0 >= neighbors[index])
                        {
                            continue;
                        }

                        var position = new Position(getRow(index), getColumn(index));

                        map[position.Row, position.Column] = number;

                        if (target == position)
                        {
                            return;
                        }

                        queue.Enqueue(position);
                    }

                    number++;
                }
            }

            private static long[] GeNeighbors(long[,] map, Position position)
            {
                var neighbors = new long[8];

                neighbors[0] = GetNeighbor(map, position.Row - 1, position.Column);
                neighbors[1] = GetNeighbor(map, position.Row - 1, position.Column + 1);
                neighbors[2] = GetNeighbor(map, position.Row, position.Column + 1);
                neighbors[3] = GetNeighbor(map, position.Row + 1, position.Column + 1);
                neighbors[4] = GetNeighbor(map, position.Row + 1, position.Column);
                neighbors[5] = GetNeighbor(map, position.Row + 1, position.Column - 1);
                neighbors[6] = GetNeighbor(map, position.Row, position.Column - 1);
                neighbors[7] = GetNeighbor(map, position.Row - 1, position.Column - 1);

                return neighbors;
            }

            private static long GetNeighbor(long[,] map, int row, int column)
            {
                var rows = map.GetLength(0);
                var columns = map.GetLength(1);

                if (0 <= column && column < columns)
                {
                    if (0 <= row && row < rows)
                    {
                        return map[row, column];
                    }
                }

                return -1;
            }
        }
    }
}