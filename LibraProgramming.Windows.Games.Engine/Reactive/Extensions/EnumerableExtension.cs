using System.Collections.Generic;
using System.Linq;
using LibraProgramming.Windows.Games.Engine.Attributes;

namespace LibraProgramming.Windows.Games.Engine.Reactive.Extensions
{
    public static class EnumerableExtension
    {
        public static IEnumerable<IEntity> MatchingGroup(this IEnumerable<IEntity> entities, IGroup group) =>
            entities.Where(group.Matches);

        public static IEnumerable<IEntity> MatchingGroup(this IEnumerable<IEntity> entities, ILookupGroup group) =>
            entities.Where(group.Matches);

        public static IEnumerable<ISystem> GetApplicableSystems(this IEnumerable<ISystem> systems, IEntity entity) =>
            systems.Where(x => entity.MatchesGroup(x.Group));

        public static IEnumerable<ISystem> GetApplicableSystems(this IEnumerable<ISystem> systems, IEnumerable<IComponent> components)
        {
            var componentTypes = components.Select(x => x.GetType());
            return systems.Where(x => x.Group.RequiredComponents.All(y => componentTypes.Contains(y)));
        }

        public static IEnumerable<T> OrderByPriority<T>(this IEnumerable<T> listToPrioritize)
        {
            return listToPrioritize.OrderBy(x =>
            {
                var finalOrder = 0;
                var priorityAttributes = x.GetType().GetCustomAttributes(typeof(PriorityAttribute), true);

                if (priorityAttributes.Length == 0)
                {
                    return finalOrder;
                }

                var priorityAttribute = priorityAttributes.FirstOrDefault() as PriorityAttribute;
                var priority = priorityAttribute.Priority;

                if (priority >= 0)
                {
                    finalOrder = int.MinValue + priority;
                }
                else
                {
                    finalOrder -= priority;
                }

                return finalOrder;
            });
        }
    }
}