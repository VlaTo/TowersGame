using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace LibraProgramming.Windows.Games.Engine.Infrastructure.Extensions
{
    public static class DictionaryExtension
    {
        public static IReadOnlyDictionary<TKey, TValue> AsReadOnly<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary)
        {
            if (null == dictionary)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            return new ReadOnlyDictionary<TKey, TValue>(dictionary);
        }
    }
}