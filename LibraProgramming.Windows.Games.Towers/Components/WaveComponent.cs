using LibraProgramming.Windows.Games.Engine;

namespace LibraProgramming.Windows.Games.Towers.Components
{
    public class WaveComponent : IComponent
    {
        public int WaveNumber
        {
            get;
        }

        public WaveComponent(int waveNumber)
        {
            WaveNumber = waveNumber;
        }
    }
}