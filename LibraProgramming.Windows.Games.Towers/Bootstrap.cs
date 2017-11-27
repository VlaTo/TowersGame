using LibraProgramming.Windows.Games.Towers.Core.ServiceContainer;
using LibraProgramming.Windows.Games.Towers.GameEngine;

namespace LibraProgramming.Windows.Games.Towers
{
    internal static class Bootstrap
    {
        internal static void Register(IServiceRegistry services)
        {
            services.Register<EnemyWaveFactory>(InstanceLifetime.CreateNew);
            services.Register<IScene, Scene>(InstanceLifetime.CreateNew);
            services.Register<GameplayController>(InstanceLifetime.CreateNew);
        }
    }
}