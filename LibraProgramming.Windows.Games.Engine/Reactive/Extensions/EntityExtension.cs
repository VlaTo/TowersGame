using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraProgramming.Windows.Games.Engine.Reactive.Extensions
{
    public static class EntityExtension
    {
        public static async Task<IEntity> WaitForPredicateMet(this IEntity entity, Predicate<IEntity> predicate)
        {
            while (false == predicate(entity))
            {
                await Task.Delay(1000);
            }

            return entity;
        }

        public static bool MatchesGroup(this IEntity entity, IGroup group) => group.Matches(entity);

        public static IEntity ApplyBlueprint(this IEntity entity, IBlueprint blueprint)
        {
            blueprint.Apply(entity);
            return entity;
        }

        public static IEntity ApplyBlueprints(this IEntity entity, params IBlueprint[] blueprints)
        {
            foreach (var blueprint in blueprints)
            {
                blueprint.Apply(entity);
            }

            return entity;
        }

        public static IEntity ApplyBlueprints(this IEntity entity, IEnumerable<IBlueprint> blueprints)
        {
            foreach (var blueprint in blueprints)
            {
                blueprint.Apply(entity);
            }

            return entity;
        }

        public static T AddComponent<T>(this IEntity entity)
            where T : class, IComponent, new()
        {
            var component = new T();

            entity.AddComponents(component);

            return component;
        }

        /// <summary>
        /// Removes a component from the entity by its type
        /// </summary>
        /// <typeparam name="T">The type of entity to remove</typeparam>
        /// <param name="entity">entity to use</param>
        /// <remarks>
        /// It is recommended if you can to batch component removals together
        /// </remarks>
        public static void RemoveComponent<T>(this IEntity entity)
            where T : class, IComponent => entity.RemoveComponents(typeof(T));

        /// <summary>
        /// Checks to see if the entity contains a given component
        /// </summary>
        /// <typeparam name="T">Type of component to check for</typeparam>
        /// <param name="entity">entity to use</param>
        /// <returns>true if the component was found, false if it was not</returns>
        public static bool HasComponent<T>(this IEntity entity)
            where T : class, IComponent => entity.HasComponent(typeof(T));

        /// <summary>
        /// Removes many component instances from the entity
        /// </summary>
        /// <param name="entity">entity to use</param>
        /// <param name="components">The components to remove</param>
        public static void RemoveComponents(this IEntity entity, params IComponent[] components)
        {
            var componentTypes = components.Select(x => x.GetType()).ToArray();
            entity.RemoveComponents(componentTypes);
        }

        /// <summary>
        /// Gets a component from the entity based upon its type or null if one cannot be found
        /// </summary>
        /// <typeparam name="T">The type of component to retrieve</typeparam>
        /// <param name="entity">entity to use</param>
        /// <returns>The component instance if found, or null if not</returns>
        public static T GetComponent<T>(this IEntity entity)
            where T : class, IComponent
        {
            var componentType = typeof(T);
            return (T) entity.GetComponent(componentType);
        }

        /// <summary>
        /// Checks to see if the entity contains all of the given components
        /// </summary>
        /// <param name="entity">entity to use</param>
        /// <param name="componentTypes">Types of components to check for</param>
        /// <returns>true if all the component was found, false if one or more is missing</returns>
        public static bool HasAllComponents(this IEntity entity, params Type[] componentTypes)
        {
            for (var index = componentTypes.Length - 1; index >= 0; index--)
            {
                if (entity.HasComponent(componentTypes[index]))
                {
                    continue;
                }

                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks to see if the entity contains all of the given components
        /// </summary>
        /// <param name="entity">entity to use</param>
        /// <param name="componentTypeIds">Type ids of components to check for</param>
        /// <returns>true if all the component was found, false if one or more is missing</returns>
        public static bool HasAllComponents(this IEntity entity, params int[] componentTypeIds)
        {
            for (var index = componentTypeIds.Length - 1; index >= 0; index--)
            {
                if (entity.HasComponent(componentTypeIds[index]))
                {
                    continue;
                }

                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks to see if the entity contains any of the given components
        /// </summary>
        /// <param name="entity">entity to use</param>
        /// <param name="componentTypes">Types of the components to check for</param>
        /// <returns>true if any components were found, false if no matching components were found</returns>
        public static bool HasAnyComponents(this IEntity entity, params Type[] componentTypes)
        {
            for (var index = componentTypes.Length - 1; index >= 0; index--)
            {
                if (entity.HasComponent(componentTypes[index]))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks to see if the entity contains any of the given components
        /// </summary>
        /// <param name="entity">entity to use</param>
        /// <param name="componentTypeIds">Type ids of the components to check for</param>
        /// <returns>true if any components were found, false if no matching components were found</returns>
        public static bool HasAnyComponents(this IEntity entity, params int[] componentTypeIds)
        {
            for (var index = componentTypeIds.Length - 1; index >= 0; index--)
            {
                if (entity.HasComponent(componentTypeIds[index]))
                {
                    return true;
                }
            }

            return false;
        }
    }
}