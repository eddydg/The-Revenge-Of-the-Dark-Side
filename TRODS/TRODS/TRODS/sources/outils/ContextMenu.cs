using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TRODS
{
    class ContextMenu : AbstractScene
    {
        private List<AnimatedSprite> _elements;
        private AnimatedSprite _container;
        internal AnimatedSprite Container
        {
            get { return _container; }
            set { _container = value; }
        }
        public Rectangle Position
        {
            set { _container.Position = value; }
            get { return _container.Position; }
        }
        private MouseState _mouseState;
        private Rectangle _windowSize;
        private bool _visible;
        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }

        public ContextMenu(Rectangle windowSize, AnimatedSprite mainContainer)
        {
            _windowSize = windowSize;
            _container = mainContainer;
            _elements = new List<AnimatedSprite>();
        }
        /// <summary>
        /// Ajout d'un element de menu.
        /// </summary>
        /// <param name="s">L'origine des positions est la position du container principal.</param>
        public void Add(AnimatedSprite s)
        {
            s.Position = new Rectangle(s.Position.X + Position.X, s.Position.Y + Position.Y, s.Position.Width, s.Position.Height);
            s.setRelatvePos(s.Position, _windowSize.Width, _windowSize.Height);
            _elements.Add(s);
        }
        public override void LoadContent(ContentManager content)
        {
            _container.LoadContent(content);
            foreach (AnimatedSprite s in _elements)
                s.LoadContent(content);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_visible)
            {
                _container.Draw(spriteBatch);
                foreach (AnimatedSprite s in _elements)
                    s.Draw(spriteBatch);
            }
        }
        public override void Update(float elapsedTime)
        {
            if (_visible)
            {
                _container.Update(elapsedTime);
                foreach (AnimatedSprite s in _elements)
                    s.Update(elapsedTime);
            }
        }
        public override void WindowResized(Rectangle rect)
        {
            _container.windowResized(rect);
            foreach (AnimatedSprite s in _elements)
                s.windowResized(rect);
            _windowSize = rect;
        }
        public void CuadricPositionning(Rectangle elementSize, int fromTop, int fromSides, int hSpace, int vSpace, bool adaptContainerHeight)
        {
            int defaultX = Position.X + fromSides;
            int maxX = defaultX + Position.Width - 2 * fromSides;
            elementSize.X = defaultX;
            elementSize.Y = Position.Y + fromTop;
            foreach (AnimatedSprite s in _elements)
            {
                s.Position = elementSize;
                s.setRelatvePos(s.Position, _windowSize.Width, _windowSize.Height);
                elementSize.X += hSpace + elementSize.Width;
                if (elementSize.X + elementSize.Width > maxX)
                {
                    elementSize.X = defaultX;
                    elementSize.Y += vSpace + elementSize.Height;
                }
            }
            if (adaptContainerHeight)
                _container.Position = new Rectangle(Position.X, Position.Y, Position.Width, _elements.Last<AnimatedSprite>().Position.Y + _elements.Last<AnimatedSprite>().Position.Height + fromSides);
        }
    }
}
