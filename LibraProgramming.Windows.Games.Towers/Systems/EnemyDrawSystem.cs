using LibraProgramming.Windows.Games.Engine;
using LibraProgramming.Windows.Games.Engine.Reactive.Extensions;
using LibraProgramming.Windows.Games.Towers.Components;
using LibraProgramming.Windows.Games.Towers.Components.Drawable;
using LibraProgramming.Windows.Games.Towers.Events;

namespace LibraProgramming.Windows.Games.Towers.Systems
{
    public class EnemyDrawSystem : EventReactionSystem<SceneDrawEvent>
    {
        private readonly IObservableGroup drawables;

        public EnemyDrawSystem(IEventSystem system, IObservableGroup drawables)
            : base(system)
        {
            this.drawables = drawables;
        }

        public override void OnEventReceived(SceneDrawEvent @event)
        {
            foreach (var entity in drawables)
            {
                var component = entity.GetComponent<DrawEnemyComponent>();
                var position = entity.GetComponent<MoveComponent>();

                component.Draw(@event.DrawingSession, position.Position.Value);
            }
        }
    }
}