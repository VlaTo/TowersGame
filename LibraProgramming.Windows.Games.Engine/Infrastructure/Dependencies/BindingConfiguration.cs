using System;
using System.Collections.Generic;

namespace LibraProgramming.Windows.Games.Engine.Infrastructure.Dependencies
{
    public class BindingConfiguration
    {
        public bool AsSingleton
        {
            get;
            set;
        }

        public string WithName
        {
            get;
            set;
        }

        public object ToInstance
        {
            get;
            set;
        }

        public Func<IDependencyContainer, object> ToMethod
        {
            get;
            set;
        }

        public IDictionary<string, object> WithNamedCtorArgs
        {
            get;
        }

        public IDictionary<Type, object> WithTypedCtorArgs
        {
            get;
        }

        public BindingConfiguration()
        {
            AsSingleton = true;
            WithNamedCtorArgs = new Dictionary<string, object>();
            WithTypedCtorArgs = new Dictionary<Type, object>();
        }
    }
}