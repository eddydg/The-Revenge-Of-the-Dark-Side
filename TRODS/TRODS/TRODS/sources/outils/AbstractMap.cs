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
    /// Classe dont Heriteront toutes les maps du jeu
    /// </summary>
    class AbstractMap : AbstractScene
    {
        protected List<Sprite> _elementsBackground;
        public List<Sprite> ElementsBackground
        {
            get { return _elementsBackground; }
            set { _elementsBackground = value; }
        }
        protected List<Sprite> _elementsMainground;
        public List<Sprite> ElementsMainground
        {
            get { return _elementsMainground; }
            set { _elementsMainground = value; }
        }
        protected List<Sprite> _elementsForeground;
        public List<Sprite> ElementsForeground
        {
            get { return _elementsForeground; }
            set { _elementsForeground = value; }
        }
        protected Rectangle _windowSize;
        public Rectangle WindowSize
        {
            get { return _windowSize; }
            set { _windowSize = value; }
        }
        protected Vector2 _vuePosition;
        public Vector2 VuePosition
        {
            get { return _vuePosition; }
            set { _vuePosition = value; }
        }
        protected List<Rectangle> _visitable;
        public List<Rectangle> Visitable
        {
            get { return _visitable; }
            set { _visitable = value; }
        }

        
        public AbstractMap(Rectangle windowSize)
        {
            _windowSize = windowSize;
            _elementsBackground = new List<Sprite>();
            _elementsForeground = new List<Sprite>();
            _elementsMainground = new List<Sprite>();
            _visitable = new List<Rectangle>();
        }

        public AbstractMap(Rectangle windowSize,
                            List<Sprite> elementsBackground, List<Sprite> elementsMainground, List<Sprite> elementsForeground,
                            Vector2 vuePosition, List<Rectangle> visitableArea)
        {
            _windowSize = windowSize;
            _elementsBackground = elementsBackground;
            _elementsForeground = elementsForeground;
            _elementsMainground = elementsMainground;
            _vuePosition = vuePosition;
            _visitable = visitableArea;
        }

        public override void LoadContent(ContentManager content)
        {
            foreach (Sprite s in _elementsBackground)
                s.LoadContent(content);
            foreach (Sprite s in _elementsForeground)
                s.LoadContent(content);
            foreach (Sprite s in _elementsMainground)
                s.LoadContent(content);
        }

        public override void Update(float elapsedTime)
        {
            foreach (AnimatedSprite s in _elementsForeground)
                s.Update(elapsedTime);
            foreach (AnimatedSprite s in _elementsMainground)
                s.Update(elapsedTime);
            foreach (AnimatedSprite s in _elementsBackground)
                s.Update(elapsedTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            DrawForeground(spriteBatch);
            DrawMainground(spriteBatch);
            DrawBackground(spriteBatch);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="repeating">Repetition des textures lorsqu'elles depassent de l'ecran</param>
        public void Draw(SpriteBatch spriteBatch, bool repeating = false)
        {
            DrawForeground(spriteBatch, repeating);
            DrawMainground(spriteBatch, repeating);
            DrawBackground(spriteBatch, repeating);
        }

        public virtual void DrawBackground(SpriteBatch spriteBatch, bool repeating = false)
        {
            if (repeating)
            {
                foreach (Sprite e in ElementsBackground)
                {
                    e.Draw(spriteBatch);
                    if (e.Position.X > _windowSize.Width)
                        e.Position = new Rectangle(-e.Position.Width + _windowSize.Width, e.Position.Y, e.Position.Width, e.Position.Height);
                    else if (e.Position.X < -e.Position.Width)
                        e.Position = new Rectangle(0, e.Position.Y, e.Position.Width, e.Position.Height);
                    if (e.Position.X > 0)
                        e.Draw(spriteBatch, new Vector2(e.Position.X - e.Position.Width, e.Position.Y));
                    else if (e.Position.X + e.Position.Width < _windowSize.Width)
                        e.Draw(spriteBatch, new Vector2(e.Position.X + e.Position.Width, e.Position.Y));
                }
            }
            else
                foreach (Sprite e in ElementsBackground)
                    e.Draw(spriteBatch);
        }

        public virtual void DrawMainground(SpriteBatch spriteBatch, bool repeating = false)
        {
            if (repeating)
            {
                foreach (Sprite e in ElementsBackground)
                {
                    e.Draw(spriteBatch);
                    if (e.Position.X > _windowSize.Width)
                        e.Position = new Rectangle(-e.Position.Width + _windowSize.Width, e.Position.Y, e.Position.Width, e.Position.Height);
                    else if (e.Position.X < -e.Position.Width)
                        e.Position = new Rectangle(0, e.Position.Y, e.Position.Width, e.Position.Height);
                    if (e.Position.X > 0)
                        e.Draw(spriteBatch, new Vector2(e.Position.X - e.Position.Width, e.Position.Y));
                    else if (e.Position.X + e.Position.Width < _windowSize.Width)
                        e.Draw(spriteBatch, new Vector2(e.Position.X + e.Position.Width, e.Position.Y));
                }
            }
            else
                foreach (Sprite e in ElementsMainground)
                    e.Draw(spriteBatch);
        }

        public virtual void DrawForeground(SpriteBatch spriteBatch, bool repeating = false)
        {
            if (repeating)
            {
                foreach (Sprite e in ElementsBackground)
                {
                    e.Draw(spriteBatch);
                    if (e.Position.X > _windowSize.Width)
                        e.Position = new Rectangle(-e.Position.Width + _windowSize.Width, e.Position.Y, e.Position.Width, e.Position.Height);
                    else if (e.Position.X < -e.Position.Width)
                        e.Position = new Rectangle(0, e.Position.Y, e.Position.Width, e.Position.Height);
                    if (e.Position.X > 0)
                        e.Draw(spriteBatch, new Vector2(e.Position.X - e.Position.Width, e.Position.Y));
                    else if (e.Position.X + e.Position.Width < _windowSize.Width)
                        e.Draw(spriteBatch, new Vector2(e.Position.X + e.Position.Width, e.Position.Y));
                }
            }
            else
                foreach (Sprite e in ElementsForeground)
                    e.Draw(spriteBatch);
        }

        /// <summary>
        /// Fonction gerant le mouvement du point de vue sur la map
        /// </summary>
        /// <param name="destination">Vecteur representant le deplacement voulu</param>
        /// <returns>Destination possible</returns>
        public virtual Vector2 Moving(Vector2 destination)
        {
            Point p = new Point((int)(destination.X + VuePosition.X), (int)(destination.Y + VuePosition.Y));
            foreach (Rectangle r in Visitable)
                if (r.Contains(p))
                    return destination;
            return Vector2.Zero;
        }
    }
}
