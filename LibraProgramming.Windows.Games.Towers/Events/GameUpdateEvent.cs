using System;
using LibraProgramming.Windows.Games.Towers.Components;

namespace LibraProgramming.Windows.Games.Towers.Events
{
    public class GameUpdateEvent
    {
        public GameComponent GameComponent
        {
            get;
        }

        public TimeSpan Elapsed
        {
            get;
        }

        public GameUpdateEvent(GameComponent component, TimeSpan elapsed)
        {
            GameComponent = component;
            Elapsed = elapsed;
        }
    }
}