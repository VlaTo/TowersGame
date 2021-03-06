﻿using System;
using LibraProgramming.Windows.Games.Engine;
using LibraProgramming.Windows.Games.Engine.Infrastructure.Dependencies;
using LibraProgramming.Windows.Games.Engine.Infrastructure.Extensions;
using LibraProgramming.Windows.Games.Engine.Reactive.Extensions;
using LibraProgramming.Windows.Games.Towers.Components;
using LibraProgramming.Windows.Games.Towers.Components.Drawable;
using LibraProgramming.Windows.Games.Towers.Core.DependencyInjection;
using LibraProgramming.Windows.Games.Towers.Events;
using LibraProgramming.Windows.Games.Towers.Systems;
using Microsoft.Graphics.Canvas;

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

        public void Draw(CanvasDrawingSession drawingSession)
        {
            EventSystem.Publish(new SceneDrawEvent(drawingSession));
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
            var temp = EntityCollectionManager.GetObservableGroup(new Group(typeof(DrawEnemyComponent)), enemiesCollectionName);

            Executor.AddSystem(new GameUpdateSystem(EventSystem));
            Executor.AddSystem(new NewEnemyWaveWarningSystem(EventSystem, TimeSpan.FromSeconds(20.0d)));
            Executor.AddSystem(new DisplayWaveWarningSystem(EventSystem));
            Executor.AddSystem(new NewEnemyWaveEmitterSystem(EventSystem, enemiesCollection));
            Executor.AddSystem(new EnemyMoveSystem(enemiesGroup));
            Executor.AddSystem(new EnemyDrawSystem(EventSystem, temp));

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