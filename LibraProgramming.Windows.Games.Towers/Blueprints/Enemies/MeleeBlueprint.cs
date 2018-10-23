using LibraProgramming.Windows.Games.Engine;
using LibraProgramming.Windows.Games.Towers.Components;

namespace LibraProgramming.Windows.Games.Towers.Blueprints.Enemies
{
    public class MeleeBlueprint : IBlueprint
    {
        public float Speed
        {
            get;
            set;
        }

        public float Health
        {
            get;
            set;
        }

        public float Damage
        {
            get;
            set;
        }

        public int WaveNumber
        {
            get;
            set;
        }

        public void Apply(IEntity entity)
        {
            entity.AddComponents(
                new HealthComponent(Health),
                new MoveComponent(Speed),
                new WaveComponent(WaveNumber)
            );
        }
    }
}