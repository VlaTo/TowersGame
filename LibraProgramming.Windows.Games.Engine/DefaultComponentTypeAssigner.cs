using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraProgramming.Windows.Games.Engine
{
    public class DefaultComponentTypeAssigner : IComponentTypeAssigner
    {
        public IReadOnlyDictionary<Type, int> GenerateComponentLookups()
        {
            var id = 0;
            var types = GetAllComponentTypes();

            return types.ToDictionary(type => type, notused => id++);
        }

        private static IEnumerable<Type> GetAllComponentTypes()
        {
            var component = typeof(IComponent);
            var assemblies=AppDomain.CurrentDomain.GetAssemblies();

            return assemblies
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => component.IsAssignableFrom(type) && false == type.IsInterface && false == type.IsAbstract);
        }
    }
}