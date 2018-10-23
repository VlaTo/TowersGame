using System;
using System.Numerics;
using LibraProgramming.Windows.Games.Engine;
using LibraProgramming.Windows.Games.Engine.Reactive;
using LibraProgramming.Windows.Games.Towers.Blueprints.Enemies;
using LibraProgramming.Windows.Games.Towers.Events;

namespace LibraProgramming.Windows.Games.Towers.Systems
{
    public class NewEnemyWaveEmitterSystem : EventReactionSystem<NewEnemyWaveEvent>
    {
        private readonly IEntityCollection collection;

        public NewEnemyWaveEmitterSystem(IEventSystem system, IEntityCollection collection)
            : base(system)
        {
            this.collection = collection;
        }

        public override void OnEventReceived(NewEnemyWaveEvent @event)
        {
            var receiver = @event.GameComponent;

            receiver.CurrentWave.Value = @event.WaveNumber;

            collection.CreateEntity(new MeleeBlueprint
            {
                WaveNumber = @event.WaveNumber,
                Position = new Vector2(20.0f, 24.0f),
                Speed = 0.2f,
                Angle = MathF.PI / 4.0f,
                Health = 20.0f,
                Damage = 12.0f
            });
        }
    }
}