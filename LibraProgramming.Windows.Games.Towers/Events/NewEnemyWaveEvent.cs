using System;
using LibraProgramming.Windows.Games.Towers.Components;

namespace LibraProgramming.Windows.Games.Towers.Events
{
    public class NewEnemyWaveEvent
    {
        public GameComponent GameComponent
        {
            get;
        }

        public int WaveNumber
        {
            get;
        }

        public TimeSpan Time
        {
            get;
        }

        public NewEnemyWaveEvent(GameComponent gameComponent, int waveNumber, TimeSpan time)
        {
            GameComponent = gameComponent;
            WaveNumber = waveNumber;
            Time = time;
        }
    }
}