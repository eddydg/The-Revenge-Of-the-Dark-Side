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
    class AbstractMap : AbstractScene
    {
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
            public void WindowResized(Rectangle oldWinSize, Rectangle winSize)
            {
                sprite.windowResized(winSize, oldWinSize);
                speed *= (float)winSize.Width / (float)oldWinSize.Width;
                verticalSpeed *= (float)winSize.Height / (float)oldWinSize.Height;
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
        public AbstractMap(Rectangle windowSize, List<Element> elements, Vector2 vuePosition, List<Rectangle> visitableArea)
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
            Rectangle p;
            foreach (Element e in _elements)
            {
                if (e.repeating)
                {

                    p = e.sprite.Position;

                    if (p.X > _windowSize.Width)
                        p.X = p.X - p.Width;
                    else if (p.X < -p.Width)
                        p.X = p.X + p.Width;

                    e.sprite.Position = p;

                    e.sprite.Draw(spriteBatch);
                    if (p.X > 0)
                        e.sprite.Draw(spriteBatch, new Vector2(p.X - p.Width, p.Y));
                    if (p.X - p.Width < _windowSize.Width)
                        e.sprite.Draw(spriteBatch, new Vector2(p.X + p.Width, p.Y));
                }
                else
                    e.sprite.Draw(spriteBatch);
            }
        }
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

                        for (int i = _visitable.Count - 1; i >= 0; i--)
                        {
                            if (_elements.ElementAt(i).foreground)
                            {
                                i = -1;
                                float s = _elements.ElementAt(i).speed;
                                float vs = _elements.ElementAt(i).verticalSpeed;
                                List<Rectangle> n = new List<Rectangle>();
                                Rectangle rip;
                                foreach (Rectangle v in _visitable)
                                {
                                    rip = v;
                                    rip.X += (int)((float)destination.X * s);
                                    rip.Y += (int)((float)destination.Y * vs);
                                    n.Add(rip);
                                }
                                _visitable = n;
                            }
                        }
                    }
                    return true;
                }
            return false;
        }
        public override void WindowResized(Rectangle rect)
        {
            foreach (Element e in _elements)
                e.WindowResized(_windowSize, rect);

            float x = (float)rect.Width / (float)_windowSize.Width;
            float y = (float)rect.Height / (float)_windowSize.Height;
            List<Rectangle> n = new List<Rectangle>();
            Rectangle r;
            foreach (Rectangle rectal in _visitable)
            {
                r = rectal;
                r.X = (int)((float)r.X * x);
                r.Y = (int)((float)r.Y * y);
                r.Width = (int)((float)r.Width * x);
                r.Height = (int)((float)r.Height * y);
                n.Add(r);
            }
            _visitable = n;

            _vuePosition.X = _vuePosition.X * x;
            _vuePosition.Y = _vuePosition.Y * y;

            _windowSize = rect;
        }
    }
}
