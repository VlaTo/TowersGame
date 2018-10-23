using System;

namespace LibraProgramming.Windows.Games.Engine.Infrastructure.Extensions
{
    public static class GroupExtension
    {
        public static IGroup GroupFor(ISystem system, params Type[] requiredTypes)
        {
            if (null == system)
            {
                throw new ArgumentNullException(nameof(system));
            }

            return new Group(requiredTypes);
        }
    }
}