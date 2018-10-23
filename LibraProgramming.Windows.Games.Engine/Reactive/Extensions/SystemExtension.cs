using System;
using System.Linq;

namespace LibraProgramming.Windows.Games.Engine.Reactive.Extensions
{
    public static class SystemExtension
    {
        public static IGroup GroupFor(this ISystem system, params Type[] requiredTypes) => new Group(requiredTypes);

        public static bool IsSystemReactive(this ISystem system) =>
            system is IReactToEntitySystem || system is IReactToGroupSystem || system.IsReactiveDataSystem();

        public static bool IsReactiveDataSystem(this ISystem system) => system.GetType()
            .GetInterfaces()
            .Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IReactToDataSystem<>));

        public static Type GetGenericDataType(this ISystem system) => system.GetType()
            .GetInterfaces()
            .Single(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IReactToDataSystem<>))
            .GetGenericArguments()[0];

        public static Type GetGenericInterfaceType(this ISystem system) => system.GetType()
            .GetInterfaces()
            .Single(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IReactToDataSystem<>));
    }
}