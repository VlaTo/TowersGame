using System;
using System.Collections.Generic;

namespace LibraProgramming.Windows.Games.Engine.Infrastructure.Extensions
{
    public static class ListExtension
    {
        public static void ExpandBy<T>(this IList<T> list, int count)
        {
            if (null == list)
            {
                throw new ArgumentNullException(nameof(list));
            }

            while (0 < count--)
            {
                list.Add(default(T));
            }
        }
    }
}