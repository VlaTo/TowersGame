using System;

namespace LibraProgramming.Windows.Games.Engine
{
    public interface IGroup
    {
        Type[] RequiredComponents
        {
            get;
        }

        Type[] ExcludedComponents
        {
            get;
        }
    }
}