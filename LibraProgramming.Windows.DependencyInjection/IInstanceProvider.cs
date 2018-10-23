using System;
using System.Collections.Generic;

namespace LibraProgramming.Windows.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public interface IInstanceProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        object GetInstance(Stack<ServiceTypeReference> queue, Type serviceType, string key);
    }
}