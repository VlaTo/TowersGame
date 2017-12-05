﻿using System;
using System.Collections.Generic;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI;

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

        protected SceneNode.SceneNodeCollection Children
        {
            get;
        }

        protected SceneNode()
        {
            Children = new SceneNode.SceneNodeCollection(this);
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

        public virtual void CreateResources(ICanvasResourceCreatorWithDpi creator, CanvasCreateResourcesReason reason)
        {
            foreach (ISceneNode child in Children)
            {
                child.CreateResources(creator, reason);
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