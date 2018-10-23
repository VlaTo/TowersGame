namespace LibraProgramming.Windows.Games.Towers.GameEngine
{
    /// <summary>
    /// 
    /// </summary>
    public class Scene : SceneNode, IScene
    {
        public ResourcesLoader Resources
        {
            get;
        }

        public override GameplayController Controller => controller;

        public void SetController(GameplayController value)
        {
            return Resources.CreateAsync(creator).AsAsyncAction();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
        }
    }
}