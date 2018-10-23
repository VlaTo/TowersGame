using System.Linq;

namespace LibraProgramming.Windows.Games.Engine.Reactive.Extensions
{
    public static class LookupGroupExtension
    {
        public static bool ContainsAllRequiredComponents(this ILookupGroup group, params int[] componentTypeIds)
        {
            for (var index = group.RequiredComponents.Length - 1; index >= 0; index--)
            {
                if (componentTypeIds.Contains(@group.RequiredComponents[index]))
                {
                    continue;
                }

                return false;
            }

            return true;
        }

        public static bool ContainsAllRequiredComponents(this ILookupGroup group, IEntity entity) =>
            entity.HasAllComponents(group.RequiredComponents);

        public static bool ContainsAnyRequiredComponents(this ILookupGroup group, params int[] componentTypes)
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

        public static bool ContainsAnyRequiredComponents(this ILookupGroup group, IEntity entity) =>
            entity.HasAnyComponents(group.RequiredComponents);

        public static bool ContainsAnyExcludedComponents(this ILookupGroup group, params int[] componentTypes) =>
            group.ExcludedComponents.Any(componentTypes.Contains);

        public static bool ContainsAnyExcludedComponents(this ILookupGroup group, IEntity entity) =>
            entity.HasAnyComponents(group.ExcludedComponents);

        public static bool ContainsAny(this ILookupGroup group, params int[] components)
        {
            var requiredContains = group.ContainsAnyRequiredComponents(components);
            return requiredContains || @group.ContainsAnyExcludedComponents(components);
        }

        public static bool Matches(this ILookupGroup lookupGroup, IEntity entity)
        {
            if (lookupGroup.ExcludedComponents.Length == 0)
            {
                return ContainsAllRequiredComponents(lookupGroup, entity);
            }

            return ContainsAllRequiredComponents(lookupGroup, entity) &&
                   !ContainsAnyExcludedComponents(lookupGroup, entity);
        }
    }
}