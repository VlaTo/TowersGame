using System;
using System.Collections.Generic;

namespace LibraProgramming.Windows.Games.Engine
{
    public interface IComponentTypeLookup
    {
        IReadOnlyDictionary<Type, int> GetAllComponentTypes();

        int GetComponentType<TComponent>()
            where TComponent : IComponent;

        int GetComponentType(Type componenType);

        int[] GetComponentTypes(params Type[] componenTypes);

        Type[] GetComponentTypes(params int[] typeIds);
    }
}