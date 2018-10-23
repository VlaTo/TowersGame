using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraProgramming.Windows.Games.Engine.Reactive.Extensions
{
    public static class GroupExtension
    {
        public static IGroup WithComponent<T>(this IGroup group)
            where T : class, IComponent
        {
            var requiredComponents = new List<Type>(group.RequiredComponents) {typeof(T)};
            return new Group(null, requiredComponents, group.ExcludedComponents);
        }

        public static IGroup WithComponents(this IGroup group, params Type[] requiredComponents)
        {
            var newComponents = new List<Type>(group.RequiredComponents);

            newComponents.AddRange(requiredComponents);

            return new Group(null, newComponents, group.ExcludedComponents);
        }

        public static IGroup WithoutComponent<T>(this IGroup group)
            where T : class, IComponent
        {
            var excludedComponents = new List<Type>(group.ExcludedComponents) {typeof(T)};
            return new Group(null, group.RequiredComponents, excludedComponents);
        }

        public static IGroup WithoutComponent<T>(this IGroup group, params Type[] excludedComponents)
        {
            var newComponents = new List<Type>(group.ExcludedComponents);

            newComponents.AddRange(excludedComponents);

            return new Group(null, group.RequiredComponents, newComponents);
        }

        public static bool ContainsAllRequiredComponents(this IGroup group, IEnumerable<IComponent> components)
        {
            for (var index = group.RequiredComponents.Length - 1; index >= 0; index--)
            {
                if (components.Any(x => @group.RequiredComponents[index].IsInstanceOfType(x)))
                {
                    continue;
                }

                return false;
            }

            return true;
        }

        public static bool ContainsAllRequiredComponents(this IGroup group, params Type[] componentTypes)
        {
            for (var index = group.RequiredComponents.Length - 1; index >= 0; index--)
            {
                if (componentTypes.Contains(@group.RequiredComponents[index]))
                {
                    continue;
                }

                return false;
            }

            return true;
        }

        public static bool ContainsAllRequiredComponents(this IGroup group, IEntity entity) =>
            entity.HasAllComponents(group.RequiredComponents);

        public static bool ContainsAnyRequiredComponents(this IGroup group, IEnumerable<IComponent> components)
        {
            for (var index = group.RequiredComponents.Length - 1; index >= 0; index--)
            {
                if (components.Any(x => group.RequiredComponents[index] == x.GetType()))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool ContainsAnyRequiredComponents(this IGroup group, params Type[] componentTypes)
        {
            for (var index = @group.RequiredComponents.Length - 1; index >= 0; index--)
            {
                for (var j = componentTypes.Length - 1; j >= 0; j--)
                {
                    if (group.RequiredComponents[index] == componentTypes[j])
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static bool ContainsAnyRequiredComponents(this IGroup group, IEntity entity) =>
            entity.HasAnyComponents(group.RequiredComponents);

        public static bool ContainsAnyExcludedComponents(this IGroup group, IEnumerable<IComponent> components)
        {
            var castComponents = components.Select(x => x.GetType()).ToArray();
            return ContainsAnyExcludedComponents(group, castComponents);
        }

        public static bool ContainsAnyExcludedComponents(this IGroup group, params Type[] componentTypes) =>
            group.ExcludedComponents.Any(componentTypes.Contains);

        public static bool ContainsAnyExcludedComponents(this IGroup group, IEntity entity) =>
            entity.HasAnyComponents(group.ExcludedComponents);

        public static bool ContainsAny(this IGroup group, params IComponent[] components)
        {
            var requiredContains = group.ContainsAnyRequiredComponents(components);
            return requiredContains || @group.ContainsAnyExcludedComponents(components);
        }

        public static bool ContainsAny(this IGroup group, params Type[] componentTypes)
        {
            var requiredContains = group.RequiredComponents.Any(componentTypes.Contains);
            return requiredContains || @group.ExcludedComponents.Any(componentTypes.Contains);
        }

        public static bool ContainsAny(this IGroup group, IEntity entity)
        {
            var requiredContains = group.ContainsAnyRequiredComponents(entity);
            return requiredContains || @group.ContainsAnyExcludedComponents(entity);
        }

        public static bool Matches(this IGroup group, IEntity entity)
        {
            if (0 == group.ExcludedComponents.Length)
            {
                return ContainsAllRequiredComponents(group, entity);
            }

            return ContainsAllRequiredComponents(group, entity) && !ContainsAnyExcludedComponents(group, entity);
        }
    }
}