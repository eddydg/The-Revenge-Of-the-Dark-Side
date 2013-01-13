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
        /// <summary>
        /// Element constitutif de la map
        /// </summary>
        public struct Element
        {
            public Element(AnimatedSprite _s, float _speed = 0, float _verticalSpeed = 0, bool _repeating = false, bool _foreground = false)
            {
                sprite = _s;
                speed = _speed;
                verticalSpeed = _verticalSpeed;
                repeating = _repeating;
                foreground = _foreground;
            }
            public AnimatedSprite sprite;
            public float speed;
            public float verticalSpeed;
            public bool repeating;
            public bool foreground;
            public void windowResized(Rectangle rect, double w, double h)
            {
                sprite.windowResized(rect);
                speed = (float)((double)speed * w);
                verticalSpeed = (float)((double)verticalSpeed * h);
            }
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="repeating">Repetition des textures lorsqu'elles depassent de l'ecran</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle p;
            foreach (Element e in _elements)
            {
                if (e.repeating)
                {

                    p = e.sprite.Position;
                    if (p.X > _windowSize.Width)
                    {
                        p.X = p.X - p.Width;
                        e.sprite.Position = p;
                    }
                    else if (p.X < -p.Width)
                    {
                        p.X = p.X + p.Width;
                        e.sprite.Position = p;
                    }
                    e.sprite.Draw(spriteBatch);
                    if (p.X > 0)
                        e.sprite.Draw(spriteBatch, new Vector2(p.X - p.Width, p.Y));
                    if (p.X < p.Width - _windowSize.Width)
                        e.sprite.Draw(spriteBatch, new Vector2(p.X + p.Width, p.Y));
                }
                else
                    e.sprite.Draw(spriteBatch);
            }
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
                        foreach (Element s in _elements)
                            s.sprite.Position = new Rectangle(
                                s.sprite.Position.X - (int)(destination.X * s.speed),
                                s.sprite.Position.Y - (int)(destination.Y * s.verticalSpeed),
                                s.sprite.Position.Width,
                                s.sprite.Position.Height);
                        _vuePosition.X += destination.X;
                        _vuePosition.Y += destination.Y;
                    }
                    return true;
                }
            return false;
        }

        public override void WindowResized(Rectangle rect)
        {
            double w = (double)rect.Width / (double)_windowSize.Width;
            double h = (double)rect.Height / (double)_windowSize.Height;
            
            foreach (Element s in _elements)
                s.windowResized(rect, w, h);

            _vuePosition.X = (float)((double)_vuePosition.X * w);
            _vuePosition.Y = (float)((double)_vuePosition.Y * h);

            List<Rectangle> l = new List<Rectangle>();
            foreach (Rectangle r in _visitable)
                l.Add(new Rectangle((int)((double)r.X * w), (int)((double)r.Y * h), (int)((double)r.Width * w), (int)((double)r.Height * h)));
            _visitable = l;

            _windowSize = rect;
        }
    }
}
