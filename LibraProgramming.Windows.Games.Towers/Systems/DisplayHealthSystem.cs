using System;
using System.Reactive.Linq;
using LibraProgramming.Windows.Games.Engine;
using LibraProgramming.Windows.Games.Engine.Reactive.Extensions;
using LibraProgramming.Windows.Games.Towers.Components;
using LibraProgramming.Windows.Games.Towers.Extensions;

namespace LibraProgramming.Windows.Games.Towers.Systems
{
    public class DisplayHealthSystem : IReactToDataSystem<float>
    {
        public IGroup Group { get; } = new Group(typeof(HealthComponent));

        public IObservable<float> ReactToData(IEntity entity)
        {
            var healthComponent = entity.GetComponent<HealthComponent>();
            return ObservableExtension.WithValueChanges(healthComponent.Health).Select(CalculateDamageTaken);
            //return healthComponent.Health.WithValueChanges<float>().Select(CalculateDamageTaken);
        }

        public void Process(IEntity entity, float data)
        {
            var component = entity.GetComponent<HealthComponent>();

            if (component.Health.Value <= 0.0f)
            {
                entity.RemoveComponents(component);
            }
        }

        private static float CalculateDamageTaken(ValueChanges<float> changes)
        {
            if (0.0f == changes.PreviousValue)
            {
                return 0.0f;
            }

            return changes.PreviousValue - changes.CurrentValue;
        }
    }
}