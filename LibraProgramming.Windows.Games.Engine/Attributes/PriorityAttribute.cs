using System;

namespace LibraProgramming.Windows.Games.Engine.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class PriorityAttribute : Attribute
    {
        public int Priority
        {
            get;
        }

        public PriorityAttribute(int priority)
        {
            Priority = priority;
        }
    }
}