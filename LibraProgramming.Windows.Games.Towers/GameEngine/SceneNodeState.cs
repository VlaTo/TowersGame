using System;

namespace LibraProgramming.Windows.Games.Towers.GameEngine
{
    public abstract class SceneNodeState : ISceneNodeState
    {
        public static readonly ISceneNodeState Empty;

        static SceneNodeState()
        {
            Empty = new EmptySceneState();
        }

        public abstract void Leave(ISceneNode node);

        public abstract void Enter(ISceneNode node);

        public abstract void Update(TimeSpan elapsed);

        /// <summary>
        /// 
        /// </summary>
        private class EmptySceneState : ISceneNodeState
        {
            public void Leave(ISceneNode node)
            {
            }

            public void Enter(ISceneNode node)
            {
            }

            public void Update(TimeSpan elapsed)
            {
            }
        }
    }
}