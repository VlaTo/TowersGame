using System;
using System.Linq;
using System.Reflection;
using LibraProgramming.Windows.Games.Engine.Infrastructure.Dependencies;

namespace LibraProgramming.Windows.Games.Engine.Infrastructure.Extensions
{
    public static class ApplicationExtension
    {
        public static void BindAllSystemsInAssemblies(this IApplication application, params Assembly[] assemblies)
        {
            if (null == application)
            {
                throw new ArgumentNullException(nameof(application));
            }

            var systemType = typeof(ISystem);
            var applicableSystems = assemblies
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type =>
                {
                    if (type.IsInterface || type.IsAbstract)
                    {
                        return false;
                    }

                    return systemType.IsAssignableFrom(type);
                })
                .ToList();

            if (false == applicableSystems.Any())
            {
                return;
            }

            var container = application.Container;

            foreach (var system in applicableSystems)
            {
                var configuration = new BindingBuilder().AsSingleton().WithName(system.Name).Build();
                container.Bind(systemType, system, configuration);
            }
        }

        public static void BindAllSystemsInNamespaces(this IApplication application, params string[] namespaces)
        {
            if (null == application)
            {
                throw new ArgumentNullException(nameof(application));
            }

            var systemType = typeof(ISystem);
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var applicableSystems = assemblies
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type =>
                {
                    if (type.IsInterface || type.IsAbstract)
                    {
                        return false;
                    }

                    if (String.IsNullOrEmpty(type.Namespace))
                    {
                        return false;
                    }

                    if (false == systemType.IsAssignableFrom(type))
                    {
                        return false;
                    }

                    var @namespace = type.Namespace;

                    return namespaces.Any(ns => @namespace.StartsWith(ns));
                })
                .ToList();

            if (false == applicableSystems.Any())
            {
                return;
            }

            var container = application.Container;

            foreach (var system in applicableSystems)
            {
                var configuration = new BindingBuilder().AsSingleton().WithName(system.Name).Build();
                container.Bind(systemType, system, configuration);
            }
        }

        public static void BindAllSystemsWithinApplicationScope(this IApplication application)
        {
            if (null == application)
            {
                throw new ArgumentNullException(nameof(application));
            }

            var ns = application.GetType().Namespace;
            var namespaces = new[]
            {
                $"{ns}.Systems",
                $"{ns}.ViewResolvers"
            };

            application.BindAllSystemsInNamespaces(namespaces);
        }

        public static void BindAndStartSystem<TSystem>(this IApplication application)
            where TSystem : ISystem
        {
            if (null == application)
            {
                throw new ArgumentNullException(nameof(application));
            }

            application.Container.Bind<ISystem, TSystem>(new BindingConfiguration {WithName = typeof(TSystem).Name});

            StartSystem<TSystem>(application);
        }

        public static void StartSystem<TSystem>(this IApplication application)
            where TSystem : ISystem
        {
            var container = application.Container;
            var name = typeof(TSystem).Name;
            var system = container.HasBinding<ISystem>(name)
                ? container.Resolve<ISystem>(name)
                : container.Resolve<TSystem>();

            application.Executor.AddSystem(system);
        }

        public static void StartAllBoundSystems(this IApplication application)
        {
            var allSystems = application.Container.ResolveAll<ISystem>();
            var orderedSystems = allSystems
                .OrderByDescending(x => x is ViewResolverSystem)
                .ThenByDescending(x => x is ISetupSystem);

            foreach (var system in orderedSystems)
            {
                application.Executor.AddSystem(system);
            }
        }
    }
}