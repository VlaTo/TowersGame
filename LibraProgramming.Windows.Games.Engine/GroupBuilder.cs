using System;
using System.Collections.Generic;

namespace LibraProgramming.Windows.Games.Engine
{
    public class GroupBuilder : IBuilder<IGroup>
    {
        private readonly IList<Type> withComponents;
        private readonly IList<Type> excludeComponents;
        private Predicate<IEntity> predicate;

        public GroupBuilder()
        {
            withComponents = new List<Type>();
            excludeComponents = new List<Type>();
        }

        public GroupBuilder WithComponent<TComponent>()
            where TComponent : class, IComponent
        {
            withComponents.Add(typeof(TComponent));
            return this;
        }

        public GroupBuilder ExcludeComponent<TComponent>()
            where TComponent : class, IComponent
        {
            excludeComponents.Add(typeof(TComponent));
            return this;
        }

        public GroupBuilder WithPredicate(Predicate<IEntity> value)
        {
            predicate = value;
            return this;
        }

        public IGroup Build()
        {
            return new Group(predicate, withComponents, excludeComponents);
        }
    }
}