using System;
using System.Collections.Generic;
using Microsoft.Graphics.Canvas;

namespace LibraProgramming.Windows.Games.Towers.GameEngine
{
    public partial class SceneNode : ISceneNode
    {
        private ISceneNode parent;
        
        public ISceneNode Parent
        {
            get => parent;
            set
            {
                if (value == parent)
                {
                    return;
                }

                if (null != parent)
                {
                    DoParentRemoved();
                }

                parent = value;

                if (null != parent)
                {
                    DoParentAdded();
                }
            }
        }

        ICollection<ISceneNode> ISceneNode.Children => Children;

        protected SceneNodeCollection Children
        {
            get;
        }

        protected SceneNode()
        {
            Children = new SceneNodeCollection(this);
        }

        public virtual void Draw(CanvasDrawingSession session)
        {
            foreach (ISceneNode child in Children)
            {
                child.Draw(session);
            }
        }

        public virtual void Update(TimeSpan elapsed)
        {
            foreach (ISceneNode child in Children)
            {
                child.Update(elapsed);
            }
        }

        protected virtual void DoParentAdded()
        {
        }

        protected virtual void DoParentRemoved()
        {
        }

        protected virtual void DoChildAdded(ISceneNode child)
        {
        }

        protected virtual void DoChildRemoved(ISceneNode child)
        {
        }
    }
}