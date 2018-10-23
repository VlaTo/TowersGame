using LibraProgramming.Windows.Games.Engine;
using LibraProgramming.Windows.Games.Towers.Events;

namespace LibraProgramming.Windows.Games.Towers.Systems
{
    public class DamageTakenSystem : EventReactionSystem<EntityDamageEvent>
    {
        public DamageTakenSystem(IEventSystem system)
            : base(system)
        {
        }

        public override void OnEventReceived(EntityDamageEvent @event)
        {
            @event.Component.Health.Value -= @event.DamageApplied;
        }
    }
}