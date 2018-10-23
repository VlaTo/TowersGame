using LibraProgramming.Windows.Games.Engine;
using LibraProgramming.Windows.Games.Towers.Events;

namespace LibraProgramming.Windows.Games.Towers.Systems
{
    public class GameUpdateSystem : EventReactionSystem<GameUpdateEvent>
    {
        public GameUpdateSystem(IEventSystem system)
            : base(system)
        {
        }

        public override void OnEventReceived(GameUpdateEvent @event)
        {
            @event.GameComponent.Elapsed.Value += @event.Elapsed;
        }
    }
}