namespace LibraProgramming.Windows.Games.Engine.Infrastructure
{
    public interface IPool<T>
    {
        int IncrementSize
        {
            get;
        }

        /// <summary>
        /// Allocates an instance in the pool for use
        /// </summary>
        /// <returns>An instance to use</returns>
        T AllocateInstance();

        /// <summary>
        /// Frees up the pooled item for re-allocation
        /// </summary>
        /// <param name="obj">The instance to put back in the pool</param>
        void ReleaseInstance(T obj);
    }
}