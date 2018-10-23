using System.Collections.Generic;

namespace LibraProgramming.Windows.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public abstract partial class InstanceLifetime
    {
        protected Factory Factory
        {
            get;
        }

        public abstract object ResolveInstance(Stack<ServiceTypeReference> queue);

        protected InstanceLifetime(Factory factory)
        {
            Factory = factory;
        }
    }
}