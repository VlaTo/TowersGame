using System;
using System.Collections;
using System.Linq;
using LibraProgramming.Windows.Games.Engine.Infrastructure.Dependencies;
using Ninject;

namespace LibraProgramming.Windows.Games.Towers.Core.DependencyInjection
{
    public class NinjectDependencyContainer : IDependencyContainer
    {
        private readonly IKernel kernel;

        public NinjectDependencyContainer(IKernel kernel = null)
        {
            this.kernel = kernel ?? new StandardKernel();
        }

        public void Bind(Type fromType, Type toType, BindingConfiguration configuration = null)
        {
            var bindingSetup = kernel.Bind(fromType);

            if (null == configuration)
            {
                bindingSetup.To(toType).InSingletonScope();
                return;
            }

            if (null != configuration.ToInstance)
            {
                var instanceBinding = bindingSetup.ToConstant(configuration.ToInstance);

                if (configuration.AsSingleton)
                {
                    instanceBinding.InSingletonScope();
                }

                return;
            }

            if (null != configuration.ToMethod)
            {
                var methodBinding = bindingSetup.ToMethod(x => configuration.ToMethod(this));

                if (configuration.AsSingleton)
                {
                    methodBinding.InSingletonScope();
                }

                return;
            }

            var binding = bindingSetup.To(toType);

            if (configuration.AsSingleton)
            {
                binding.InSingletonScope();
            }

            if (false == string.IsNullOrEmpty(configuration.WithName))
            {
                binding.Named(configuration.WithName);
            }

            if (0 == configuration.WithNamedCtorArgs.Count)
            {
                return;
            }

            foreach (var constructorArg in configuration.WithNamedCtorArgs)
            {
                binding.WithConstructorArgument(constructorArg.Key, constructorArg.Value);
            }

            foreach (var constructorArg in configuration.WithTypedCtorArgs)
            {
                binding.WithConstructorArgument(constructorArg.Key, constructorArg.Value);
            }
        }

        public void Bind(Type toType, BindingConfiguration configuration = null)
        {
            Bind(toType, toType, configuration);
        }

        public bool HasBinding(Type type, string name = null)
        {
            var bindings = kernel.GetBindings(type);

            if (String.IsNullOrEmpty(name))
            {
                return bindings.Any();
            }

            return bindings.Any(x => x.Metadata.Name == name);
        }

        public object Resolve(Type type, string name = null)
        {
            if (String.IsNullOrEmpty(name))
            {
                return kernel.Get(type);
            }

            return kernel.Get(type, name);
        }

        public void Unbind(Type type)
        {
            kernel.Unbind(type);
        }

        public IEnumerable ResolveAll(Type type)
        {
            return kernel.GetAll(type);
        }

        public void LoadModule(IDependencyModule module)
        {
            module.Setup(this);
        }
    }
}