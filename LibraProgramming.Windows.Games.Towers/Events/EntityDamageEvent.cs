using LibraProgramming.Windows.Games.Towers.Components;

namespace LibraProgramming.Windows.Games.Towers.Events
{
    public class EntityDamageEvent
    {
        public HealthComponent Component
        {
            get;
        }

        public float DamageApplied
        {
            get;
        }

        public EntityDamageEvent(HealthComponent component, float damageApplied)
        {
            Component = component;
            DamageApplied = damageApplied;
        }
    }
}