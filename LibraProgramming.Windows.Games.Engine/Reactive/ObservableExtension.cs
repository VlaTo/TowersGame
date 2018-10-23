using System;
using LibraProgramming.Windows.Games.Engine.Infrastructure;

namespace LibraProgramming.Windows.Games.Engine.Reactive
{
    public static class ObservableExtension
    {
        /*public static IDisposable Subscribe<TSource>(this IObservable<TSource> observable, Action<TSource> next)
        {
            return observable.Subscribe(CreateSubscribedObserver(next, Throw.Error, Next.Empty));
        }*/

        internal static IObserver<TSource> CreateSubscribedObserver<TSource>(Action<TSource> next,
            Action<Exception> error, Action completed)
        {
            if (Next<TSource>.Ignore == next)
            {
                return new EmptySubscribeObserver<TSource>(error, completed);
            }

            return new SubscribeObserver<TSource>(next, error, completed);
        }
    }
}