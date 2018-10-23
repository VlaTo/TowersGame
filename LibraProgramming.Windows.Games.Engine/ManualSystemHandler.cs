using LibraProgramming.Windows.Games.Engine.Reactive;

namespace LibraProgramming.Windows.Games.Engine
{
    public class ManualSystemHandler : IConventionalSystemHandler
    {
        public IEntityCollectionManager EntityCollectionManager
        {
            get;
        }

        public ManualSystemHandler(IEntityCollectionManager entityCollectionManager)
        {
            EntityCollectionManager = entityCollectionManager;
        }

        public bool CanHandleSystem(ISystem system) => system is IManualSystem;

        public void SetupSystem(ISystem system)
        {
            var observableGroup = EntityCollectionManager.GetObservableGroup(system.Group);
            var castSystem = (IManualSystem) system;

            castSystem.Start(observableGroup);
        }

        public void TeardownSystem(ISystem system)
        {
            var observableGroup = EntityCollectionManager.GetObservableGroup(system.Group);
            var castSystem = (IManualSystem) system;

            castSystem.Stop(observableGroup);
        }

        public void Dispose()
        {
        }
    }
}