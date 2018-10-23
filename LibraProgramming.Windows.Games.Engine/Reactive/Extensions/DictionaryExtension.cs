using System;
using System.Collections.Generic;

namespace LibraProgramming.Windows.Games.Engine.Reactive.Extensions
{
    public static class DictionaryExtension
    {
        public static void RemoveAndDispose<TKey>(this IDictionary<TKey, IDisposable> disposables, TKey key)
        {
            disposables[key].Dispose();
            disposables.Remove(key);
        }

        public static void RemoveAndDisposeAll<TKey>(this IDictionary<TKey, IDisposable> disposables)
        {
            foreach (var kvp in disposables)
            {
                kvp.Value.Dispose();
            }

            disposables.Clear();
        }
    }
}