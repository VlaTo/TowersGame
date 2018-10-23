using System.Collections.Generic;
using LibraProgramming.Windows.Games.Engine.Reactive.Collections;

namespace LibraProgramming.Windows.Games.Engine.Reactive.Extensions
{
    public static class ListExtension
    {
        public static ReactiveCollection<T> ToReactiveCollection<T>(this IEnumerable<T> source)
        {
            return new ReactiveCollection<T>(source);
        }
    }
}