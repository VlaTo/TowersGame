using System;

namespace LibraProgramming.Windows.Games.Engine
{
    public sealed class EmptyGroup : IGroup
    {
        public Type[] RequiredComponents
        {
            get;
        } = new Type[0];

        public Type[] ExcludedComponents
        {
            get;
        } = new Type[0];

        public Predicate<IEntity> TargettedEntities => null;
    }
}