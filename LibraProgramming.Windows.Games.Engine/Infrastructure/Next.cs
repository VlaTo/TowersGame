using System;

namespace LibraProgramming.Windows.Games.Engine.Infrastructure
{
    internal static class Next
    {
        public static readonly Action Empty = () => { };
    }

    internal static class Next<TSource>
    {
        public static readonly Action<TSource> Ignore = source => { };
    }
}