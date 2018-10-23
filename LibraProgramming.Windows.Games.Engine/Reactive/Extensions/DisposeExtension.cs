using System;
using System.Collections.Generic;

namespace LibraProgramming.Windows.Games.Engine.Reactive.Extensions
{
    public static class DisposeExtension
    {
        public static void DisposeAll(this IEnumerable<IDisposable> disposables)
        {
            foreach (var disposable in disposables)
            {
                disposable.Dispose();
            }
        }

        public static void DisposeAll<TKey>(this IDictionary<TKey, IDisposable> disposables)
        {
            foreach (var disposable in disposables.Values)
            {
                disposable.Dispose();
            }
        }
    }
}