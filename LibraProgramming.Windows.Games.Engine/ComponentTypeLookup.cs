using System;
using System.Collections.Generic;
using System.Linq;
using LibraProgramming.Windows.Games.Engine.Infrastructure.Extensions;

namespace LibraProgramming.Windows.Games.Engine
{
    public class ComponentTypeLookup : IComponentTypeLookup
    {
        public IReadOnlyDictionary<Type, int> ComponentsByType
        {
            get;
        }

        public IReadOnlyDictionary<int, Type> ComponentsById
        {
            get;
        }

        public ComponentTypeLookup(IReadOnlyDictionary<Type, int> lookup)
        {
            ComponentsByType = lookup;
            ComponentsById = lookup
                .ToDictionary(kvp => kvp.Value, kvp => kvp.Key)
                .AsReadOnly();
        }

        public IReadOnlyDictionary<Type, int> GetAllComponentTypes() => ComponentsByType;

        public int GetComponentType<TComponent>()
            where TComponent : IComponent
        {
            return GetComponentType(typeof(TComponent));
        }

        public int GetComponentType(Type componenType)
        {
            if (null == componenType)
            {
                throw new ArgumentNullException(nameof(componenType));
            }

            return ComponentsByType[componenType];
        }

        public int[] GetComponentTypes(params Type[] componenTypes)
        {
            return componenTypes.Select(GetComponentType).ToArray();
        }

        public Type[] GetComponentTypes(params int[] typeIds)
        {
            return typeIds.Select(id => ComponentsById[id]).ToArray();
        }
    }
}