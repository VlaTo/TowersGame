using System;

namespace LibraProgramming.Windows.DependencyInjection.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class ServiceAttribute : Attribute
    {
        public string Name
        {
            get;
            set;
        }
    }
}