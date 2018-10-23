using System;
using System.Numerics;
using System.Reactive.Linq;
using Windows.Foundation;
using LibraProgramming.Windows.Games.Engine;
using LibraProgramming.Windows.Games.Engine.Reactive.Extensions;
using LibraProgramming.Windows.Games.Towers.Components;
using LibraProgramming.Windows.Games.Towers.Extensions;

namespace LibraProgramming.Windows.Games.Towers.Systems
{
    public class EnemyMoveSystem : IReactToDataSystem<TimeSpan>
    {
        private readonly IObservableGroup enemies;

        public IGroup Group
        {
            get;
        }

        public EnemyMoveSystem(IObservableGroup enemies)
        {
            this.enemies = enemies;

            Group = new Group(typeof(GameComponent));
        }

        public IObservable<TimeSpan> ReactToData(IEntity entity)
        {
            var game = entity.GetComponent<GameComponent>();
            return game.Elapsed.WithValueChanges().Select(OnTimeElapsed);
        }

        public void Process(IEntity entity, TimeSpan data)
        {
            var game = entity.GetComponent<GameComponent>();

            foreach (var enemy in enemies)
            {
                var component = enemy.GetComponent<MoveComponent>();
                var direction = GetDirection(component.Angle.Value);

                component.Position.Value += direction * component.Speed;
            }
        }

        private static TimeSpan OnTimeElapsed(ValueChanges<TimeSpan> arg)
        {
            if (TimeSpan.Zero == arg.PreviousValue)
            {
                return TimeSpan.Zero;
            }

            return arg.CurrentValue - arg.PreviousValue;
        }

        private static Vector2 GetDirection(float angle)
        {
            return new Point(Math.Cos(angle), Math.Sin(angle)).ToVector2();
        }
    }
}