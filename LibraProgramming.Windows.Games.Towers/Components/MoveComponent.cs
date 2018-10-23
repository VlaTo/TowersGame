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

        public MoveComponent(Vector2 position, float angle, float speed)
        {
            Speed = speed;
            Position = new ObservableProperty<Vector2>(position);
            Angle = new ObservableProperty<float>(angle);
        }

        public void Dispose()
        {
        }
    }
}