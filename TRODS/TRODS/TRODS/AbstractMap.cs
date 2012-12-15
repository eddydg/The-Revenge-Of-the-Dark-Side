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
        protected List<Vector2> _upperVisitableLimit;
        public List<Vector2> UpperVisitableLimit
        {
            get { return _upperVisitableLimit; }
            set { _upperVisitableLimit = value; }
        }
        protected List<Vector2> _lowerVisitableLimit;
        public List<Vector2> LowerVisitableLimit
        {
            get { return _lowerVisitableLimit; }
            set { _lowerVisitableLimit = value; }
        }
        protected int _leftVisitableLimit;
        public int LeftVisitableLimit
        {
            get { return _leftVisitableLimit; }
            set { _leftVisitableLimit = value; }
        }
        protected int _rightVisitableLimit;
        public int RightVisitableLimit
        {
            get { return _rightVisitableLimit; }
            set { _rightVisitableLimit = value; }
        }


        public AbstractMap(Rectangle windowSize)
        {
            _windowSize = windowSize;
            _elementsBackground = new List<Sprite>();
            _elementsForeground = new List<Sprite>();
            _elementsMainground = new List<Sprite>();
            _upperVisitableLimit = new List<Vector2>();
            _lowerVisitableLimit = new List<Vector2>();
        }

        /// <summary>
        /// Constructeur de test
        /// </summary>
        /// <param name="windowSize">Dimensions de la fenetre</param>
        /// <param name="elementsBackground">Textures d'arriere plan</param>
        /// <param name="elementsMainground">Textures de la lane</param>
        /// <param name="elementsForeground">Textures de premier plan</param>
        /// <param name="mapDimensions">Dimension de la map</param>
        /// <param name="vuePosition">Position du point de vue sur la map</param>
        /// <param name="upperVisitableLimit">Limite visitable superieure</param>
        /// <param name="lowerVisitableLimit">Limite visitable inferieure</param>
        /// <param name="leftVisitableLimit">Limite visitable gauche</param>
        /// <param name="rightVisitableLimit">Limite visitable droite</param>
        public AbstractMap(Rectangle windowSize,
                            List<Sprite> elementsBackground, List<Sprite> elementsMainground, List<Sprite> elementsForeground,
                            Vector2 vuePosition,
                            List<Vector2> upperVisitableLimit, List<Vector2> lowerVisitableLimit,
                            int leftVisitableLimit, int rightVisitableLimit)
        {
            _windowSize = windowSize;
            _elementsBackground = elementsBackground;
            _elementsForeground = elementsForeground;
            _elementsMainground = elementsMainground;
            _vuePosition = vuePosition;
            _upperVisitableLimit = upperVisitableLimit;
            _lowerVisitableLimit = lowerVisitableLimit;
            _leftVisitableLimit = leftVisitableLimit;
            _rightVisitableLimit = rightVisitableLimit;
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

        public void Draw(SpriteBatch spriteBatch, bool repeating = false)
        {
            DrawForeground(spriteBatch, repeating);
            DrawMainground(spriteBatch, repeating);
            DrawBackground(spriteBatch, repeating);
        }

        /// <summary>
        /// Dessine le contenu graphique de l'arriere plan
        /// spriteBatch.Begin et spriteBatch.End non declares
        /// </summary>
        /// <param name="spriteBatch">Instance du gestionnaire de dessin de XNA</param>
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

        /// <summary>
        /// Dessine le contenu graphique de la lane
        /// spriteBatch.Begin et spriteBatch.End non declares
        /// </summary>
        /// <param name="spriteBatch">Instance du gestionnaire de dessin de XNA</param>
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

        /// <summary>
        /// Dessine le contenu graphique du premier plan
        /// spriteBatch.Begin et spriteBatch.End non declares
        /// </summary>
        /// <param name="spriteBatch">Instance du gestionnaire de dessin de XNA</param>
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
            destination = VuePosition + destination;

            if (destination.X < LeftVisitableLimit)
                destination.X = LeftVisitableLimit;
            else if (destination.X > RightVisitableLimit)
                destination.X = RightVisitableLimit;

            Vector2 currentPos = new Vector2();
            foreach (Vector2 v in LowerVisitableLimit)
            {
                if (destination.X >= currentPos.X && destination.X <= currentPos.X + v.X)
                {
                    float k = v.Y / v.X;
                    if (destination.Y - currentPos.Y > (destination.X - currentPos.X) * k)
                        destination.Y = currentPos.Y + (destination.X - currentPos.X) * k;
                }
                else
                    currentPos += v;
            }
            currentPos = new Vector2();
            foreach (Vector2 v in UpperVisitableLimit)
            {
                if (destination.X >= currentPos.X && destination.X <= currentPos.X + v.X)
                {
                    float k = v.Y / v.X;
                    if (destination.Y - currentPos.Y < (destination.X - currentPos.X) * k)
                        destination.Y = currentPos.Y + (destination.X - currentPos.X) * k;
                }
                else
                    currentPos += v;
            }

            destination -= VuePosition;
            VuePosition += destination;
            return destination;
        }
    }
}
