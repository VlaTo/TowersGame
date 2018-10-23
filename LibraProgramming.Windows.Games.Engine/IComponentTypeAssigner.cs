using System;
using System.Collections.Generic;

namespace LibraProgramming.Windows.Games.Engine
{
    public interface IComponentTypeAssigner
    {
        IReadOnlyDictionary<Type, int> GenerateComponentLookups();
    }
}