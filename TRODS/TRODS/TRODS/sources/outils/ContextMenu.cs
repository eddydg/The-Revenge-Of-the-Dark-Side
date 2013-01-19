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
    /// <summary>
    /// Menu popup
    /// </summary>
    class ContextMenu : AbstractScene
    {
        private List<AnimatedSprite> _elements;
        internal List<AnimatedSprite> Elements
        {
            get { return _elements; }
            set { _elements = value; }
        }
        private AnimatedSprite _container;
        internal AnimatedSprite Container
        {
            get { return _container; }
            set { _container = value; }
        }
        private AnimatedSprite _title;
        internal AnimatedSprite Title
        {
            get { return _title; }
            set
            {
                _title = value;
                _title.setRelatvePos(new Rectangle(
                    _title.Position.X + Position.X,
                    _title.Position.Y + Position.Y,
                    _title.Position.Width,
                    _title.Position.Height),
                    _windowSize.Width,
                    _windowSize.Height);
            }
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
        private int _choise;
        public int Choise
        {
            get { return _choise; }
            set { _choise = value; }
        }
        private bool _isMoving;
        private byte _backOpacity;
        public byte BackOpacity
        {
            get { return _backOpacity; }
            set { _backOpacity = value; }
        }

        public const int NONE = -1;
        public const int HIDE_MENU = -2;

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="windowSize">Taille courante de la fenetre</param>
        /// <param name="mainContainer">Fond du menu</param>
        /// <param name="backOpacity">Opacite de l'arriere-plan du menu (entre 0 et 255)</param>
        public ContextMenu(Rectangle windowSize, AnimatedSprite mainContainer, byte backOpacity = 255)
        {
            _windowSize = windowSize;
            _container = mainContainer;
            _elements = new List<AnimatedSprite>();
            _isMoving = false;
            _backOpacity = backOpacity;
        }

        /// <summary>
        /// Ajout d'un element de menu, l'origine des positions est la position du container principal (fond).
        /// </summary>
        /// <param name="s">Element de menu a ajouter</param>
        public void Add(AnimatedSprite s)
        {
            s.Position = new Rectangle(s.Position.X + Position.X, s.Position.Y + Position.Y, s.Position.Width, s.Position.Height);
            s.setRelatvePos(s.Position, _windowSize.Width, _windowSize.Height);
            _elements.Add(s);
        }
        /// <summary>
        /// Deplace le menu de la valeur indiquee.
        /// </summary>
        /// <param name="x">Abcisse du deplacement</param>
        /// <param name="y">Ordonee du deplacement</param>
        public void MoveBy(int x, int y)
        {
            _container.setRelatvePos(new Rectangle(Position.X + x, Position.Y + y, Position.Width, Position.Height), _windowSize.Width, _windowSize.Height);
            _title.setRelatvePos(new Rectangle(_title.Position.X + x, _title.Position.Y + y, _title.Position.Width, _title.Position.Height), _windowSize.Width, _windowSize.Height);
            foreach (AnimatedSprite s in _elements)
                s.setRelatvePos(new Rectangle(s.Position.X + x, s.Position.Y + y, s.Position.Width, s.Position.Height), _windowSize.Width, _windowSize.Height);

        }
        /// <summary>
        /// Place tous les elements du menu par remplissage de lignes. Ils auront tous la meme taille.
        /// Pour placeer soi-meme certains elements, leur positions deveront etre modifies posterieurement ou ajoutes apres l'appel de la fonction.
        /// L'origine des positions etant la position du container principal.
        /// </summary>
        /// <param name="elementSize">Taille de tous les elements du menu (x et y n'importent pas).</param>
        /// <param name="fromTop">Hauteur du premier element.</param>
        /// <param name="fromSides">abcisse minimale du premier element.</param>
        /// <param name="hSpace">Espace horizontal entre les elements.</param>
        /// <param name="vSpace">Espace entre les lignes d'elements.</param>
        /// <param name="adaptContainerHeight">Si la variable vaut true, la hauteur du container principal sera adaptee.</param>
        public void CuadricPositionning(Rectangle elementSize, int fromTop, int fromSides, int hSpace, int vSpace, bool adaptContainerHeight)
        {
            int defaultX = Position.X + fromSides;
            int maxX = defaultX + Position.Width - 2 * fromSides;
            defaultX += (maxX - defaultX - ((maxX - defaultX) / (elementSize.Width + hSpace)) * (elementSize.Width + hSpace) + hSpace) / 2;
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
            if (adaptContainerHeight && _elements.Count > 0)
            {
                _container.setRelatvePos(
                    new Rectangle(Position.X, Position.Y, Position.Width, _elements.Last<AnimatedSprite>().Position.Y + _elements.Last<AnimatedSprite>().Position.Height + fromSides - Position.Y),
                    _windowSize.Width,
                    _windowSize.Height);
            }
        }

        public override void HandleInput(KeyboardState newKeyboardState, MouseState newMouseState, Game1 parent)
        {
            if (_visible)
            {
                if (newMouseState != _mouseState)
                {
                    if (newMouseState.LeftButton == ButtonState.Pressed && _mouseState.LeftButton == ButtonState.Released)
                    {
                        Rectangle click = new Rectangle(newMouseState.X, newMouseState.Y, 1, 1);
                        _choise = ContextMenu.NONE;
                        if (_title != null && click.Intersects(_title.Position))
                        {
                            _visible = false;
                            _choise = ContextMenu.HIDE_MENU;
                        }
                        foreach (AnimatedSprite s in _elements)
                        {
                            if (click.Intersects(s.Position))
                                _choise = _elements.IndexOf(s);
                        }
                        if (_choise != ContextMenu.NONE)
                            _isMoving = false;
                        else if (click.Intersects(Position))
                            _isMoving = true;
                    }
                    else if (newMouseState.LeftButton == ButtonState.Pressed && _mouseState.LeftButton == ButtonState.Pressed)
                    {
                        if (_isMoving)
                            MoveBy(newMouseState.X - _mouseState.X, newMouseState.Y - _mouseState.Y);
                    }
                    else
                        _isMoving = false;
                    _mouseState = newMouseState;
                }
            }
        }
        public override void LoadContent(ContentManager content)
        {
            _container.LoadContent(content);
            foreach (AnimatedSprite s in _elements)
                s.LoadContent(content);
            if (_title != null)
                _title.LoadContent(content);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_visible)
            {
                _container.Draw(spriteBatch, _backOpacity);
                if (_title != null)
                    _title.Draw(spriteBatch);
                foreach (AnimatedSprite s in _elements)
                    s.Draw(spriteBatch);
            }
        }
        public override void Update(float elapsedTime)
        {
            if (_visible)
            {
                _container.Update(elapsedTime);
                if (_title != null)
                    _title.Update(elapsedTime);
                foreach (AnimatedSprite s in _elements)
                    s.Update(elapsedTime);
            }
        }
        public override void WindowResized(Rectangle rect)
        {
            _container.windowResized(rect);
            if (_title != null)
                _title.windowResized(rect);
            foreach (AnimatedSprite s in _elements)
                s.windowResized(rect);
            _windowSize = rect;
        }
        public override void Activation(Game1 parent)
        {
            _visible = false;
            _choise = ContextMenu.NONE;
        }
    }
}
