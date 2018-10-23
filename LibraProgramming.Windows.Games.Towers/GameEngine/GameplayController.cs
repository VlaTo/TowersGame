using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Windows.Foundation;
using Windows.System;
using Windows.UI.Input;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
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

        public GameplayController(IScene scene, EnemyWaveFactory waveFactory)
        {
            this.waveFactory = waveFactory;
            game = new Game(scene, new MapSize(24, 24));
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

            var pathFinder = new AStarPathFinder(game);
            var enemy = new Enemy(scene.Resources, game.CoordinatesSystem, new Position(0, 1), pathFinder, 250.0f, 0.56f, 1.0f);
            var landscape = new Landscape(scene.Resources, game.Map, size);
            
            scene.Children.Add(landscape);
            scene.Children.Add(new Grid(scene.Resources, new Size(25.0d, 25.0d), size));
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
        private class AStarPathFinder : IPathFinder
        {
            private readonly Game game;
            private long [,] heatMap;

            public AStarPathFinder(Game game)
            {
                this.game = game;
            }

            public IReadOnlyCollection<Position> GetWaypoints(Position position)
            {
                var map = GetGameHeatMap();
                var destination = new Position(23, 23);

                EmitWaveTo(map, position, destination);

                var waypoints = TraceBackFrom(map, destination, position);

                waypoints = OptimizeWaypoints(waypoints);
                
                return waypoints.ToArray();
            }

            private long[,] GetGameHeatMap()
            {
                if (null != heatMap)
                {
                    return heatMap;
                }

                heatMap = new long[,]
                {
                    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, -1, -1, -1, -1, -1, -1 },
                    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, -1, -1, -1, -1, -1, -1 },
                    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, -1, -1, 0, 0, 0, 0 },
                    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, -1, -1, 0, 0, 0, 0 },
                    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, 0 },
                    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, 0 },
                    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, 0 },
                    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, -1, -1, 0 },
                    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
                };
                
                return heatMap;
            }

            private static void EmitWaveTo(long[,] map, Position origin, Position target)
            {
                var queue = new Queue<Position>();

                queue.Enqueue(origin);

                while (0 < queue.Count)
                {
                    var location = queue.Dequeue();
                    var step = map[location.Row, location.Column] + 1;
                    var neighbors = GetNeighbors(map, location);

                    for (var index = 0; index < neighbors.Length; index++)
                    {
                        if (0 > neighbors[index])
                        {
                            continue;
                        }

                        var position = location + new Position(GetColumnFromIndex(index), GetRowFromIndex(index));

                        if (0 < map[position.Row, position.Column])
                        {
                            continue;
                        }

                        map[position.Row, position.Column] = step;

                        if (target == position)
                        {
                            return;
                        }

                        queue.Enqueue(position);
                    }
                }
            }

            private static List<Position> TraceBackFrom(long[,] map, Position origin, Position destination)
            {
                var queue = new Stack<Position>();
                var step = map[origin.Row, origin.Column];
                var position = origin;

                while (destination != position)
                {
                    var neighbors = GetNeighbors(map, position);

                    for (var index = 0; index < neighbors.Length; index++)
                    {
                        var target = position + new Position(GetColumnFromIndex(index), GetRowFromIndex(index));

                        if (destination == target)
                        {
                            position = target;
                            break;
                        }

                        if (step - neighbors[index] != 1)
                        {
                            continue;
                        }

                        position = target;

                        step = map[position.Row, position.Column];
                        queue.Push(position);

                        break;
                    }
                }
                
                return queue.ToList();
            }

            private static List<Position> OptimizeWaypoints(List<Position> waypoints)
            {
                if (null == waypoints)
                {
                    return new List<Position>();
                }

                var path = new List<Position>();
                Position? delta = null;

                foreach (var waypoint in waypoints)
                {
                    if (false == delta.HasValue)
                    {
                        path.Add(waypoint);
                        delta = new Position();

                        continue;
                    }

                    var index = path.Count - 1;
                    var direction = waypoint - path[index];

                    if (delta == direction)
                    {
                        path[index] = path[index] + direction;

                        continue;
                    }

                    path.Add(waypoint);
                    delta = direction;
                }

                return path;
            }

            private static int GetColumnFromIndex(int index)
            {
                switch (index)
                {
                    case 0:
                    case 2:
                    {
                        return 0;
                    }

                    case 1:
                    {
                        return 1;
                    }

                    case 3:
                    {
                        return -1;
                    }

                    default:
                    {
                        throw new Exception();
                    }
                }
            }

            private static int GetRowFromIndex(int index)
            {
                switch (index)
                {
                    case 0:
                    {
                        return -1;
                    }

                    case 1:
                    case 3:
                    {
                        return 0;
                    }

                    case 2:
                    {
                        return 1;
                    }

                    default:
                    {
                        throw new Exception();
                    }
                }
            }

            private static long[] GetNeighbors(long[,] map, Position position)
            {
                var neighbors = new long[4];

                neighbors[0] = GetNeighbor(map, position.Row - 1, position.Column);
                neighbors[1] = GetNeighbor(map, position.Row, position.Column + 1);
                neighbors[2] = GetNeighbor(map, position.Row + 1, position.Column);
                neighbors[3] = GetNeighbor(map, position.Row, position.Column - 1);

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