using System;

namespace LibraProgramming.Windows.Games.Engine.Infrastructure
{
    internal static class Throw
    {
        public static readonly Action<Exception> Error = exception => throw exception;
    }
}