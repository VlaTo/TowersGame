using System;
using System.Collections;

namespace LibraProgramming.Windows.Games.Engine.Infrastructure.Dependencies
{
    public interface IDependencyContainer
    {
        void Bind(Type fromType, Type toType, BindingConfiguration configuration = null);

        void Bind(Type toType, BindingConfiguration configuration = null);

        bool HasBinding(Type type, string name = null);

        object Resolve(Type type, string name = null);

        void Unbind(Type type);

        IEnumerable ResolveAll(Type type);

        void LoadModule(IDependencyModule module);
    }
}