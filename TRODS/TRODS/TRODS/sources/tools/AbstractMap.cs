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
        protected List<AbstractMap.Element> _elements;
        protected Rectangle _windowSize;
        protected Vector2 _originalVuePosition;
        protected Vector2 _vuePosition;
        protected List<Rectangle> _originalVisitable;
        protected List<Rectangle> _visitable;
        protected bool _isDrawingForeground;

        public List<AbstractMap.Element> Elements
        {
            get
            {
                return this._elements;
            }
            set
            {
                this._elements = value;
            }
        }

        public Rectangle WindowSize
        {
            get
            {
                return this._windowSize;
            }
            set
            {
                this._windowSize = value;
            }
        }

        public Vector2 VuePosition
        {
            get
            {
                return this._vuePosition;
            }
            set
            {
                this._vuePosition = value;
                this._originalVuePosition = this._vuePosition;
            }
        }

        public List<Rectangle> Visitable
        {
            get
            {
                return this._visitable;
            }
            set
            {
                this._visitable = value;
            }
        }

        public bool IsDrawingForeground
        {
            get
            {
                return this._isDrawingForeground;
            }
            set
            {
                this._isDrawingForeground = value;
            }
        }

        public AbstractMap(Rectangle windowSize)
        {
            this._windowSize = windowSize;
            this._elements = new List<AbstractMap.Element>();
            this._visitable = new List<Rectangle>();
            this._isDrawingForeground = false;
            this._originalVisitable = new List<Rectangle>();
        }

        public AbstractMap(Rectangle windowSize, List<AbstractMap.Element> elements, Vector2 vuePosition, List<Rectangle> visitableArea)
        {
            this._windowSize = windowSize;
            this._elements = elements;
            this._vuePosition = vuePosition;
            this._visitable = visitableArea;
            this._isDrawingForeground = false;
            this._originalVisitable = visitableArea;
            this._originalVuePosition = vuePosition;
        }

        public virtual void Add(AnimatedSprite s, float horizontalSpeed = 0.0f, float verticalSpeed = 0.0f, bool repeating = false, bool isForeground = false)
        {
            this._elements.Add(new AbstractMap.Element(s, horizontalSpeed, verticalSpeed, repeating, isForeground, false, true, false));
        }

        public virtual void AddVisitable(Rectangle v)
        {
            this._visitable.Add(v);
            this._originalVisitable.Add(v);
        }

        public virtual void AddVisitable(int x, int y, int width, int height)
        {
            this._visitable.Add(new Rectangle(x, y, width, height));
            this._originalVisitable.Add(new Rectangle(x, y, width, height));
        }

        public virtual bool Moving(Vector2 destination, bool performMove = true)
        {
            Point point = new Point((int)((double)destination.X + (double)this.VuePosition.X), (int)((double)destination.Y + (double)this.VuePosition.Y));
            foreach (Rectangle rectangle1 in this.Visitable)
            {
                if (rectangle1.Contains(point))
                {
                    if (performMove)
                    {
                        foreach (AbstractMap.Element element in this._elements)
                            element.sprite.Position = new Rectangle(element.sprite.Position.X - (int)((double)destination.X * (double)element.speed), element.sprite.Position.Y - (int)((double)destination.Y * (double)element.verticalSpeed), element.sprite.Position.Width, element.sprite.Position.Height);
                        this._vuePosition.X += destination.X;
                        this._vuePosition.Y += destination.Y;
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
            }
            return false;
        }

        public override void LoadContent(ContentManager content)
        {
            foreach (AbstractMap.Element element in this._elements)
                ((AbstractScene)element.sprite).LoadContent(content);
        }

        public override void Update(float elapsedTime)
        {
            foreach (AbstractMap.Element element in this._elements)
                element.sprite.Update(elapsedTime);
        }

        public virtual void Draw(SpriteBatch s, bool foreground)
        {
            this._isDrawingForeground = foreground;
            Draw(s);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (AbstractMap.Element element in this._elements)
            {
                if (element.foreground == this._isDrawingForeground)
                {
                    if (element.repeating)
                    {
                        Rectangle position = element.sprite.Position;
                        if (position.X > this._windowSize.Width)
                            position.X = position.X - position.Width;
                        else if (position.X < -position.Width)
                            position.X = position.X + position.Width;
                        element.sprite.Position = position;
                        ((AbstractScene)element.sprite).Draw(spriteBatch);
                        if (position.X > 0)
                            element.sprite.Draw(spriteBatch, new Vector2((float)(position.X - position.Width), (float)position.Y));
                        if (position.X - position.Width < this._windowSize.Width)
                            element.sprite.Draw(spriteBatch, new Vector2((float)(position.X + position.Width), (float)position.Y));
                    }
                    else
                        ((AbstractScene)element.sprite).Draw(spriteBatch);
                }
            }
        }

        public virtual int GetTravelState(Rectangle pos)
        {
            foreach (AbstractMap.Element element in this._elements)
            {
                if (element.sprite.Position.Intersects(pos) && element.isPortal)
                    return element.isPortalNext ? 1 : -1;
            }
            return 0;
        }

        public override void WindowResized(Rectangle rect)
        {
            foreach (AbstractMap.Element element in this._elements)
                element.WindowResized(this._windowSize, rect);
            float x = (float)rect.Width / (float)this._windowSize.Width;
            float y = (float)rect.Height / (float)this._windowSize.Height;
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
            //foreach (Rectangle rectangle in this._visitable)
            //{
            //    rectangle.X = (int)((double)rectangle.X * (double)num1);
            //    rectangle.Y = (int)((double)rectangle.Y * (double)num2);
            //    rectangle.Width = (int)((double)rectangle.Width * (double)num1);
            //    rectangle.Height = (int)((double)rectangle.Height * (double)num2);
            //    list.Add(rectangle);
            //}
            this._visitable = n;
            this._vuePosition.X = this._vuePosition.X * x;
            this._vuePosition.Y = this._vuePosition.Y * y;
            this._windowSize = rect;
        }

        public override void Activation(Game1 parent = null)
        {
            foreach (AbstractMap.Element element in this._elements)
                element.Reset(this._windowSize);
            this._visitable = this._originalVisitable;
            this._vuePosition = this._originalVuePosition;
        }

        public struct Element
        {
            public AnimatedSprite sprite;
            public Rectangle originalPosition;
            public float speed;
            public float verticalSpeed;
            public bool repeating;
            public bool foreground;
            public bool isPortal;
            public bool isPortalNext;
            public bool isHeal;

            public Element(AnimatedSprite _s, float _speed = 0.0f, float _verticalSpeed = 0.0f, bool _repeating = false, bool _foreground = false, bool isportal = false, bool isportalnext = true, bool healing = false)
            {
                this.sprite = _s;
                this.speed = _speed;
                this.verticalSpeed = _verticalSpeed;
                this.repeating = _repeating;
                this.foreground = _foreground;
                this.originalPosition = _s.Position;
                this.isPortal = isportal;
                this.isPortalNext = isportalnext;
                this.isHeal = healing;
            }

            public void WindowResized(Rectangle oldWinSize, Rectangle winSize)
            {
                this.sprite.windowResized(winSize, oldWinSize);
                this.speed *= (float)winSize.Width / (float)oldWinSize.Width;
                this.verticalSpeed *= (float)winSize.Height / (float)oldWinSize.Height;
            }

            public void Reset(Rectangle winSize)
            {
                this.sprite.setRelatvePos(this.originalPosition, winSize.Width, winSize.Height);
            }
        }
    }
}
