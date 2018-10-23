using System;
using System.Numerics;
using LibraProgramming.Windows.Games.Engine;
using LibraProgramming.Windows.Games.Engine.Reactive;

namespace LibraProgramming.Windows.Games.Towers.Components
{
    public class MoveComponent : IComponent, IDisposable
    {
        public float Speed
        {
            get;
        }

        public ObservableProperty<Vector2> Position
        {
            get;
            set;
        }

        public ObservableProperty<float> Angle
        {
            get;
            set;
        }

        public MoveComponent(float speed)
        {
            Speed = speed;
            Position = new ObservableProperty<Vector2>(new Vector2());
            Angle = new ObservableProperty<float>(0.0f);
        }

        public void Dispose()
        {
        }
    }
}