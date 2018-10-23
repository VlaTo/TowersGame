using System;

namespace LibraProgramming.Windows.Games.Engine.Infrastructure.Dependencies
{
    public class BindingBuilder : IBuilder<BindingConfiguration>
    {
        protected BindingConfiguration configuration;

        public BindingBuilder()
        {
            configuration = new BindingConfiguration();
        }

        public BindingBuilder AsSingleton()
        {
            configuration.AsSingleton = true;
            return this;
        }

        public BindingBuilder AsTransient()
        {
            configuration.AsSingleton = false;
            return this;
        }

        public BindingBuilder WithName(string name)
        {
            configuration.WithName = name;
            return this;
        }

        public BindingBuilder WithCtorArg(string name, object value)
        {
            configuration.WithNamedCtorArgs.Add(name, value);
            return this;
        }

        public BindingBuilder WithCtorArg<TValue>(string value)
        {
            configuration.WithTypedCtorArgs.Add(typeof(TValue), value);
            return this;
        }

        public BindingBuilder WithCtorArg(Type type, object value)
        {
            configuration.WithTypedCtorArgs.Add(type, value);
            return this;
        }

        public BindingConfiguration Build()
        {
            return configuration;
        }
    }

    public class BindingBuilder<TFrom> : BindingBuilder
    {
        public BindingBuilder<TFrom> ToInstance<TTo>(TTo instance)
            where TTo : TFrom
        {
            if (null != configuration.ToMethod)
            {
                throw new Exception();
            }

            configuration.ToInstance = instance;

            return this;
        }

        public BindingBuilder<TFrom> ToMethod<TTo>(Func<IDependencyContainer, object> method)
            where TTo : TFrom
        {
            if (null != configuration.ToInstance)
            {
                throw new Exception();
            }

            configuration.ToMethod = container => method.Invoke(container);

            return this;
        }
    }
}