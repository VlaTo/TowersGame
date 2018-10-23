using System.Numerics;
using Windows.UI;
using LibraProgramming.Windows.Games.Engine;
using LibraProgramming.Windows.Games.Towers.Components;
using LibraProgramming.Windows.Games.Towers.Components.Drawable;

namespace LibraProgramming.Windows.Games.Towers.Blueprints.Enemies
{
    public class MeleeBlueprint : IBlueprint
    {
        public float Speed
        {
            get;
            set;
        }

        public Vector2 Position
        {
            get;
            set;
        }

        public float Angle
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
                new MoveComponent(Position, Angle, Speed),
                new WaveComponent(WaveNumber),
                new DrawEnemyComponent(Colors.DarkOliveGreen)
            );
        }
    }
}