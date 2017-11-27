using System;

namespace LibraProgramming.Windows.Games.Towers.GameEngine
{
    public class StateAwareSceneNode : SceneNode
    {
        private ISceneNodeState state;

        public ISceneNodeState State
        {
            get => state;
            set
            {
                state?.Leave(this);
                state = value;
                state?.Enter(this);
            }
        }

        protected StateAwareSceneNode()
        {
            State = SceneNodeState.Empty;
        }

        public override void Update(TimeSpan elapsed)
        {
            state?.Update(elapsed);
            base.Update(elapsed);
        }
    }
}