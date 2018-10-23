using System;
using System.Collections.Generic;
using System.Linq;
using LibraProgramming.Windows.Games.Engine.Infrastructure.Dependencies;
using LibraProgramming.Windows.Games.Engine.Reactive;

namespace LibraProgramming.Windows.Games.Engine.Infrastructure.Extensions
{
    public static class DependencyContainerExtension
    {
        public static void Bind<TFrom, TTo>(this IDependencyContainer container,
            BindingConfiguration configuration = null)
            where TTo : TFrom
        {
            if (null == container)
            {
                throw new ArgumentNullException(nameof(container));
            }

            container.Bind(typeof(TFrom), typeof(TTo), configuration);
        }

        public static void Bind<TType>(this IDependencyContainer container, BindingConfiguration configuration = null)
        {
            if (null == container)
            {
                throw new ArgumentNullException(nameof(container));
            }

            container.Bind(typeof(TType), configuration);
        }

        public static void Bind<TType>(this IDependencyContainer container, Action<BindingBuilder<TType>> configurator)
        {
            if (null == container)
            {
                throw new ArgumentNullException(nameof(container));
            }

            var builder = new BindingBuilder<TType>();

            configurator.Invoke(builder);
            container.Bind<TType>(builder.Build());
        }

        public static void Bind<TFrom, TTo>(this IDependencyContainer container, Action<BindingBuilder> configurator)
            where TTo : TFrom
        {
            if (null == container)
            {
                throw new ArgumentNullException(nameof(container));
            }

            var builder = new BindingBuilder();

            configurator.Invoke(builder);
            container.Bind<TFrom, TTo>(builder.Build());
        }

        public static bool HasBinding<TType>(this IDependencyContainer container, string name = null) =>
            container.HasBinding(typeof(TType), name);

        public static TType Resolve<TType>(this IDependencyContainer container, string name = null) =>
            (TType) container.Resolve(typeof(TType), name);

        public static void Unbind<TType>(this IDependencyContainer container) =>
            container.Unbind(typeof(TType));

        public static IEnumerable<TType> ResolveAll<TType>(this IDependencyContainer container) =>
            container.ResolveAll(typeof(TType)).Cast<TType>();

        public static void LoadModule<TType>(this IDependencyContainer container)
            where TType : IDependencyModule, new() =>
            container.LoadModule(new TType());

        public static IObservableGroup ResolveObservableGroup(this IDependencyContainer container, IGroup group)
        {
            var collectionManager = container.Resolve<IEntityCollectionManager>();
            return collectionManager.GetObservableGroup(group);
        }

        public static IObservableGroup ResolveObservableGroup(this IDependencyContainer container, params Type[] componentTypes)
        {
            var collectionManager = container.Resolve<IEntityCollectionManager>();
            var group = new Group(componentTypes);

            return collectionManager.GetObservableGroup(group);
        }
    }
}