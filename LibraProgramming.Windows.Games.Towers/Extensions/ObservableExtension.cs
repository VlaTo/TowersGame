using System;
using System.Reactive.Linq;

namespace LibraProgramming.Windows.Games.Towers.Extensions
{
    public class ValueChanges<TValue>
    {
        public TValue PreviousValue
        {
            get;
        }

        public TValue CurrentValue
        {
            get;
        }

        public ValueChanges(TValue previousValue, TValue currentValue)
        {
            PreviousValue = previousValue;
            CurrentValue = currentValue;
        }
    }

    public static class ObservableExtension
    {
        public static IObservable<ValueChanges<TValue>> WithValueChanges<TValue>(this IObservable<TValue> source)
        {
            var value = default(TValue);
            return source.Scan(
                new ValueChanges<TValue>(value, value),
                (accumulator, current) => new ValueChanges<TValue>(accumulator.CurrentValue, current)
            );
        }
    }
}