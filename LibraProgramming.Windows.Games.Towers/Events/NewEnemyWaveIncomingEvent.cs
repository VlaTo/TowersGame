using System;
using LibraProgramming.Windows.Games.Towers.Components;

namespace LibraProgramming.Windows.Games.Towers.Events
{
    public class NewEnemyWaveIncomingEvent
    {
        public GameComponent GameComponent
        {
            get;
        }

        public TimeSpan Countdown
        {
            get;
        }

        public NewEnemyWaveIncomingEvent(GameComponent gameComponent, TimeSpan countdown)
        {
            GameComponent = gameComponent;
            Countdown = countdown;
        }
    }
}