using System;
using LibraProgramming.Windows.Games.Engine;
using LibraProgramming.Windows.Games.Towers.Components;

namespace LibraProgramming.Windows.Games.Towers.Groups
{
    public class MovingEnemiesGroup : ComputedGroup, IMovingEnemiesGroup
    {
        public override IObservable<bool> RefreshWhen
        {
            get;
        }

        public MovingEnemiesGroup(IObservableGroup observableGroup)
            : base(observableGroup)
        {
        }

        public override bool IsEntityApplicable(IEntity entity) => entity.HasComponent(typeof(MoveComponent));
    }
}