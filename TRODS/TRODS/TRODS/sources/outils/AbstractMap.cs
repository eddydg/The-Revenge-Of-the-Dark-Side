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
        public struct Element
        {
            public Element(Sprite _s, float _speed = 0, bool _foreground = false)
            {
                sprite = _s;
                speed = _speed;
                foreground = _foreground;
            }
            public Sprite sprite;
            public float speed;
            public bool foreground;
        }
        protected List<Element> _elements;
        public List<Element> Elements
        {
            get { return _elements; }
            set { _elements = value; }
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
            _elements = new List<Element>();
            _visitable = new List<Rectangle>();
        }

        public AbstractMap(Rectangle windowSize, List<Element> elements,
                            Vector2 vuePosition, List<Rectangle> visitableArea)
        {
            _windowSize = windowSize;
            _elements = elements;
            _vuePosition = vuePosition;
            _visitable = visitableArea;
        }

        public override void LoadContent(ContentManager content)
        {
            foreach (Element s in _elements)
                s.sprite.LoadContent(content);
        }

        public override void Update(float elapsedTime)
        {
            foreach (Element s in _elements)
                s.sprite.Update(elapsedTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (Element e in _elements)
                e.sprite.Draw(spriteBatch);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="repeating">Repetition des textures lorsqu'elles depassent de l'ecran</param>
        public void Draw(SpriteBatch spriteBatch, bool repeating = false)
        {
            if (repeating)
            {
                foreach (Element e in _elements)
                {
                    e.sprite.Draw(spriteBatch);
                    if (e.sprite.Position.X > _windowSize.Width)
                        e.sprite.Position = new Rectangle(-e.sprite.Position.Width + _windowSize.Width, e.sprite.Position.Y, e.sprite.Position.Width, e.sprite.Position.Height);
                    else if (e.sprite.Position.X < -e.sprite.Position.Width)
                        e.sprite.Position = new Rectangle(0, e.sprite.Position.Y, e.sprite.Position.Width, e.sprite.Position.Height);
                    if (e.sprite.Position.X > 0)
                        e.sprite.Draw(spriteBatch, new Vector2(e.sprite.Position.X - e.sprite.Position.Width, e.sprite.Position.Y));
                    else if (e.sprite.Position.X + e.sprite.Position.Width < _windowSize.Width)
                        e.sprite.Draw(spriteBatch, new Vector2(e.sprite.Position.X + e.sprite.Position.Width, e.sprite.Position.Y));
                }
            }
            else
                foreach (Element e in _elements)
                    e.sprite.Draw(spriteBatch);
        }

        /// <summary>
        /// Mouvement sur la map
        /// </summary>
        /// <param name="destination">Vecteur deplacement voulu</param>
        /// <param name="performMove">Si vrai, le mouvement sera realise</param>
        /// <returns>Vrai si le mouvement est autorise</returns>
        public virtual bool Moving(Vector2 destination, bool performMove = true)
        {
            Point p = new Point((int)(destination.X + VuePosition.X), (int)(destination.Y + VuePosition.Y));
            foreach (Rectangle r in Visitable)
                if (r.Contains(p))
                {
                    if (performMove)
                    {
                        float x = destination.X;
                        foreach (Element s in _elements)
                            s.sprite.Position = new Rectangle(s.sprite.Position.X + (int)x, s.sprite.Position.Y, s.sprite.Position.Width, s.sprite.Position.Height);
                    }
                    return true;
                }
            return false;
        }

        public override void WindowResized(Rectangle rect)
        {
            foreach (Element s in _elements)
                s.sprite.windowResized(rect);
        }
    }
}
