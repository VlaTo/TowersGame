using System;
using LibraProgramming.Windows.Games.Engine;
using LibraProgramming.Windows.Games.Engine.Reactive;

namespace LibraProgramming.Windows.Games.Towers.Components
{
    public class HealthComponent : IComponent, IDisposable
    {
        public float MaxHealth
        {
            get;
        }

        public ObservableProperty<float> Health
        {
            get;
        }

        public HealthComponent(float maxHealth)
        {
            MaxHealth = maxHealth;
            Health = new ObservableProperty<float>(maxHealth);
        }

        public void Dispose()
        {
            Health.Dispose();
        }
    }
}