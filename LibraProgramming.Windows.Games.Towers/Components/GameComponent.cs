using System;
using LibraProgramming.Windows.Games.Engine;
using LibraProgramming.Windows.Games.Engine.Reactive;

namespace LibraProgramming.Windows.Games.Towers.Components
{
    public class GameComponent : IComponent
    {
        public ObservableProperty<TimeSpan> Elapsed
        {
            get;
        }

        public ObservableProperty<int> CurrentWave
        {
            get;
        }

        public GameComponent()
        {
            Elapsed = new ObservableProperty<TimeSpan>(TimeSpan.Zero);
            CurrentWave = new ObservableProperty<int>();
        }
    }
}