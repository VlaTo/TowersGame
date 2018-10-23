using System;
using System.Reactive.Linq;
using LibraProgramming.Windows.Games.Engine;
using LibraProgramming.Windows.Games.Engine.Reactive;
using LibraProgramming.Windows.Games.Engine.Reactive.Extensions;
using LibraProgramming.Windows.Games.Towers.Extensions;
using LibraProgramming.Windows.Games.Towers.Components;
using LibraProgramming.Windows.Games.Towers.Events;

namespace LibraProgramming.Windows.Games.Towers.Systems
{
    public class NewEnemyWaveWarningSystem : IReactToDataSystem<TimeSpan>
    {
        private static readonly TimeSpan warningTimeout = TimeSpan.FromSeconds(10.0d);

        private readonly IEventSystem eventSystem;
        private TimeSpan countdown;
        private int waveNumber;

        public IGroup Group
        {
            get;
        }

        public TimeSpan Timeout
        {
            get;
        }

        public ObservableProperty<TimeSpan> LastWaveTime
        {
            get;
        }

        public NewEnemyWaveWarningSystem(IEventSystem eventSystem, TimeSpan timeout)
        {
            this.eventSystem = eventSystem;
            countdown = timeout;
            waveNumber = 1;

            Group = new Group(typeof(GameComponent));
            LastWaveTime = new ObservableProperty<TimeSpan>();
            Timeout = timeout;
        }

        public IObservable<TimeSpan> ReactToData(IEntity entity)
        {
            var game = entity.GetComponent<GameComponent>();
            return game.Elapsed.WithValueChanges().Select(GetElapsed);
        }

        public void Process(IEntity entity, TimeSpan value)
        {
            var game = entity.GetComponent<GameComponent>();
            var newValue = countdown - value;

            if (TimeSpan.Zero < newValue)
            {
                countdown = newValue;

                if (warningTimeout >= countdown)
                {
                    eventSystem.Publish(new NewEnemyWaveIncomingEvent(game, countdown));
                }

                return;
            }

            LastWaveTime.Value = game.Elapsed.Value;
            countdown = Timeout;

            eventSystem.Publish(new NewEnemyWaveEvent(game, waveNumber++, LastWaveTime.Value));
        }

        private static TimeSpan GetElapsed(ValueChanges<TimeSpan> data)
        {
            if (TimeSpan.Zero == data.CurrentValue)
            {
                return TimeSpan.Zero;
            }

            return data.CurrentValue - data.PreviousValue;
        }
    }
}