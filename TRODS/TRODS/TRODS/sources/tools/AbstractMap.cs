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
        /// <summary>
        /// Element constitutif de la mep.
        /// </summary>
        public struct Element
        {
            public Element(AnimatedSprite _s, float _speed = 0, float _verticalSpeed = 0, bool _repeating = false, bool _foreground = false, bool isportal = false, bool isportalnext = true)
            {
                sprite = _s;
                speed = _speed;
                verticalSpeed = _verticalSpeed;
                repeating = _repeating;
                foreground = _foreground;
                originalPosition = _s.Position;
                isPortal = isportal;
                isPortalNext = isportalnext;
            }
            public AnimatedSprite sprite;
            public Rectangle originalPosition;
            public float speed;
            public float verticalSpeed;
            public bool repeating;
            public bool foreground;
            public bool isPortal;
            public bool isPortalNext;
            public void WindowResized(Rectangle oldWinSize, Rectangle winSize)
            {
                sprite.windowResized(winSize, oldWinSize);
                speed *= (float)winSize.Width / (float)oldWinSize.Width;
                verticalSpeed *= (float)winSize.Height / (float)oldWinSize.Height;
            }
            public void Reset(Rectangle winSize)
            {
                sprite.setRelatvePos(originalPosition, winSize.Width, winSize.Height);
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
        protected Vector2 _originalVuePosition;
        protected Vector2 _vuePosition;
        public Vector2 VuePosition
        {
            get { return _vuePosition; }
            set
            {
                _vuePosition = value;
                //if (_originalVuePosition == null)
                _originalVuePosition = _vuePosition;
            }
        }
        protected List<Rectangle> _originalVisitable;
        protected List<Rectangle> _visitable;
        public List<Rectangle> Visitable
        {
            get { return _visitable; }
            set { _visitable = value; }
        }
        protected bool _isDrawingForeground;
        public bool IsDrawingForeground
        {
            get { return _isDrawingForeground; }
            set { _isDrawingForeground = value; }
        }

        /// <summary>
        /// Constructeur par defaut.
        /// </summary>
        /// <param name="windowSize">Taille actuelle de la fenetre</param>
        public AbstractMap(Rectangle windowSize)
        {
            _windowSize = windowSize;
            _elements = new List<Element>();
            _visitable = new List<Rectangle>();
            _isDrawingForeground = false;
            _originalVisitable = new List<Rectangle>();
        }
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="windowSize">Taille actuelle de la fenetre</param>
        /// <param name="elements">Liste d'Element de la map</param>
        /// <param name="vuePosition">Position de la "camera" sur la map</param>
        /// <param name="visitableArea">Liste de regions autorisees</param>
        public AbstractMap(Rectangle windowSize, List<Element> elements, Vector2 vuePosition, List<Rectangle> visitableArea)
        {
            _windowSize = windowSize;
            _elements = elements;
            _vuePosition = vuePosition;
            _visitable = visitableArea;
            _isDrawingForeground = false;
            _originalVisitable = visitableArea;
            _originalVuePosition = vuePosition;
        }

        /// <summary>
        /// Ajoute un element a la map.
        /// </summary>
        /// <param name="s">AnimatedSprite representant l'element</param>
        /// <param name="horizontalSpeed">Vitesse horizontale</param>
        /// <param name="verticalSpeed">Vitesse verticale</param>
        /// <param name="repeating">Si la valeur est true, l'element se repete. Attention, ne fonctionne pas avec des elements trop petits.</param>
        /// <param name="isForeground">Si la valeur est true, l'element sera dessine devant le personnage.</param>
        public virtual void Add(AnimatedSprite s, float horizontalSpeed = 0, float verticalSpeed = 0, bool repeating = false, bool isForeground = false)
        {
            _elements.Add(new Element(s, horizontalSpeed, verticalSpeed, repeating, isForeground));
        }
        /// <summary>
        /// Ajoute une zone autorisee
        /// </summary>
        /// <param name="v">Zone a ajouter</param>
        public virtual void AddVisitable(Rectangle v)
        {
            _visitable.Add(v);
            _originalVisitable.Add(v);
        }
        /// <summary>
        /// Ajoute une zone autorisee
        /// </summary>
        /// <param name="x">Abcisse</param>
        /// <param name="y">Ordonee</param>
        /// <param name="width">Largeur</param>
        /// <param name="height">Hauteur</param>
        public virtual void AddVisitable(int x, int y, int width, int height)
        {
            _visitable.Add(new Rectangle(x, y, width, height));
            _originalVisitable.Add(new Rectangle(x, y, width, height));
        }
        /// <summary>
        /// Effectue un mouvement sur la map.
        /// </summary>
        /// <param name="destination">Vecteur representant le mouvement voulu.</param>
        /// <param name="performMove">Si la valeur est false, le mouvement n'est pas effectue.</param>
        /// <returns>Renvoie true si le mouvement est autorise.</returns>
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
        public virtual void Draw(SpriteBatch s, bool foreground)
        {
            _isDrawingForeground = foreground;
            Draw(s);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle p;
            foreach (Element e in _elements)
            {
                if (e.foreground == _isDrawingForeground)
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
        }

        public virtual int GetTravelState(Rectangle pos)
        {
            foreach (Element e in _elements)
            {
                if (e.sprite.Position.Intersects(pos) && e.isPortal)
                    return e.isPortalNext ? 1 : -1;
            }
            return 0;
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
        public override void Activation(Game1 parent = null)
        {
            foreach (Element e in _elements)
                e.Reset(_windowSize);
            _visitable = _originalVisitable;
            _vuePosition = _originalVuePosition;
        }
    }
}
