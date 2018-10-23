using System.Diagnostics;
using LibraProgramming.Windows.Games.Engine;
using LibraProgramming.Windows.Games.Towers.Events;

namespace LibraProgramming.Windows.Games.Towers.Systems
{
    public class DisplayWaveWarningSystem : EventReactionSystem<NewEnemyWaveIncomingEvent>
    {
        public DisplayWaveWarningSystem(IEventSystem system)
            : base(system)
        {
        }

        public override void OnEventReceived(NewEnemyWaveIncomingEvent @event)
        {
            Debug.WriteLine($"Warning! Wave incomig timer: {@event.Countdown:g}");
        }
    }
}