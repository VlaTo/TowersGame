using System;
using LibraProgramming.Windows.Games.Engine;
using LibraProgramming.Windows.Games.Engine.Infrastructure.Dependencies;
using LibraProgramming.Windows.Games.Engine.Infrastructure.Extensions;
using LibraProgramming.Windows.Games.Engine.Reactive.Extensions;
using LibraProgramming.Windows.Games.Towers.Blueprints;
using LibraProgramming.Windows.Games.Towers.Blueprints.Enemies;
using LibraProgramming.Windows.Games.Towers.Components;
using LibraProgramming.Windows.Games.Towers.Core.DependencyInjection;
using LibraProgramming.Windows.Games.Towers.Events;
using LibraProgramming.Windows.Games.Towers.Systems;

namespace LibraProgramming.Windows.Games.Towers.GameEngine
{
    public class TowersGame : WindowsUniversalPlatformApplication
    {
        public override IDependencyContainer Container
        {
            get;
        }

        protected GameComponent GameComponent
        {
            get;
            private set;
        }

        public TowersGame()
        {
            Container = new NinjectDependencyContainer();
        }

        public void Update(TimeSpan elapsed)
        {
            EventSystem.Publish(new GameUpdateEvent(GameComponent, elapsed));
        }

        protected override void ApplicationStarted()
        {
            const string enemiesCollectionName = "enemies";

            var enemiesCollection = EntityCollectionManager.CreateCollection(enemiesCollectionName);
            var enemiesGroup = EntityCollectionManager.GetObservableGroup(new Group(typeof(MoveComponent)), enemiesCollectionName);

            Executor.AddSystem(new GameUpdateSystem(EventSystem));
            Executor.AddSystem(new NewEnemyWaveWarningSystem(EventSystem, TimeSpan.FromSeconds(20.0d)));
            Executor.AddSystem(new DisplayWaveWarningSystem(EventSystem));
            Executor.AddSystem(new NewEnemyWaveEmitterSystem(EventSystem, enemiesCollection));
            Executor.AddSystem(new EnemyMoveSystem(enemiesGroup));

            this.StartAllBoundSystems();

            var manager = EntityCollectionManager.GetCollection();
            var game = manager.CreateEntity();

            GameComponent = game.AddComponent<GameComponent>();
        }

        protected override void Dispose(bool dispose)
        {
        }
    }
}