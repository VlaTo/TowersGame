namespace LibraProgramming.Windows.Games.Towers.GameEngine
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TResource"></typeparam>
    public interface IResourceProvider<out TResource>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        TResource this[string key]
        {
            get;
        }
    }
}