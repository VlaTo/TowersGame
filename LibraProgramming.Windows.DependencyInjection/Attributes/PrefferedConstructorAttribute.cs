using System;

namespace LibraProgramming.Windows.DependencyInjection.Attributes
{
    /// <summary>
    /// Marks preffered ctor for <see cref="ServiceLocator" /> class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Constructor)]
    public sealed class PrefferedConstructorAttribute : Attribute
    {
    }
}