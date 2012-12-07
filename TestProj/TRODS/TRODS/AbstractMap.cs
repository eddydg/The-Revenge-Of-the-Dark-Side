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
        /// Liste des elements de decor
        /// </summary>
        protected List<Sprite> _elementsBackground;

        /// <summary>
        /// Liste des elements de la lane
        /// </summary>
        protected List<Sprite> _elementsMainground;

        /// <summary>
        /// Liste des elements de premier plan
        /// </summary>
        protected List<Sprite> _elementsForeground;

        /// <summary>
        /// Taille de la fenetre sur l'ecran
        /// </summary>
        protected Rectangle _windowSize;

        /// <summary>
        /// Taille et position de la map
        /// </summary>
        protected Rectangle _mapDimension;

        /// <summary>
        /// Position actulle du point de vue
        /// </summary>
        protected Vector2 _vuePosition;

        /// <summary>
        /// Liste de Vecteurs delimitant la hauteur
        /// maximale visitable de la map
        /// </summary>
        protected List<Vector2> _upperVisitableLimit;

        /// <summary>
        /// Liste de Vecteurs delimitant la hauteur
        /// minimale visitable de la map
        /// </summary>
        protected List<Vector2> _lowerVisitableLimit;

        /// <summary>
        /// Abcisse minimale visitable de la map
        /// </summary>
        protected int _leftVisitableLimit;

        /// <summary>
        /// Abcisse maximale visitable de la map
        /// </summary>
        protected int _rightVisitableLimit;

        /// <summary>
        /// Constructeur Par defaut
        /// </summary>
        /// <param name="windowSize">Taille de la fenetre sur l'ecran
        /// Obtenue pat *Game*.Window.ClientBounds()</param>
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
                            Rectangle mapDimensions, Vector2 vuePosition,
                            List<Vector2> upperVisitableLimit, List<Vector2> lowerVisitableLimit,
                            int leftVisitableLimit, int rightVisitableLimit)
        {
            _windowSize = windowSize;
            _elementsBackground = elementsBackground;
            _elementsForeground = elementsForeground;
            _elementsMainground = elementsMainground;
            _mapDimension = mapDimensions;
            _vuePosition = vuePosition;
            _upperVisitableLimit = upperVisitableLimit;
            _lowerVisitableLimit = lowerVisitableLimit;
            _leftVisitableLimit = leftVisitableLimit;
            _rightVisitableLimit = rightVisitableLimit;
        }

        public override void Update(float elapsedTime)
        {
            // A implementer :
            // Mouvements independants du joueur : Moteur a particules, environnement
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            DrawForeground(spriteBatch);
            DrawMainground(spriteBatch);
            DrawBackground(spriteBatch);
        }

        /// <summary>
        /// Dessine le contenu graphique de l'arriere plan,de la lane et du personnage
        /// </summary>
        /// <param name="spriteBatch">Instance du gestionnaire de dessin de XNA</param>
        /// <param name="personnage">Reference du personnage</param>
        public virtual void Draw(SpriteBatch spriteBatch, byte personnage/* PERSONNAGE*/)
        {
            DrawForeground(spriteBatch);
            // PERSONNAGE.DRAW
            DrawMainground(spriteBatch);
            DrawBackground(spriteBatch);
        }

        /// <summary>
        /// Dessine le contenu graphique de l'arriere plan
        /// spriteBatch.Begin et spriteBatch.End non declares
        /// </summary>
        /// <param name="spriteBatch">Instance du gestionnaire de dessin de XNA</param>
        public virtual void DrawBackground(SpriteBatch spriteBatch)
        {
            foreach (Sprite e in _elementsBackground)
                e.Draw(spriteBatch);
        }

        /// <summary>
        /// Dessine le contenu graphique de la lane
        /// spriteBatch.Begin et spriteBatch.End non declares
        /// </summary>
        /// <param name="spriteBatch">Instance du gestionnaire de dessin de XNA</param>
        public virtual void DrawMainground(SpriteBatch spriteBatch)
        {
            foreach (Sprite e in _elementsMainground)
                e.Draw(spriteBatch);
        }

        /// <summary>
        /// Dessine le contenu graphique du premier plan
        /// spriteBatch.Begin et spriteBatch.End non declares
        /// </summary>
        /// <param name="spriteBatch">Instance du gestionnaire de dessin de XNA</param>
        public virtual void DrawForeground(SpriteBatch spriteBatch)
        {
            foreach (Sprite e in _elementsForeground)
                e.Draw(spriteBatch);
        }

        /// <summary>
        /// Fonction gerant le mouvement du point de vue sur la map
        /// </summary>
        /// <param name="destination">Vecteur representant le deplacement voulu</param>
        /// <returns>Destination possible</returns>
        public virtual Vector2 Moving(Vector2 destination)
        {
            destination = _vuePosition + destination;

            if (destination.X < _leftVisitableLimit)
                destination.X = _leftVisitableLimit;
            else if (destination.X > _rightVisitableLimit)
                destination.X = _rightVisitableLimit;

            Vector2 currentPos = new Vector2();
            foreach (Vector2 v in _upperVisitableLimit)
            {
                if (destination.X >= currentPos.X && destination.X <= currentPos.X + v.X)
                {
                    float k = v.Y / v.X;
                    if (destination.Y - currentPos.Y > currentPos.X * k)
                        destination.Y = currentPos.Y + v.X * k;
                    break;
                }
                else
                    currentPos += v;
            }
            foreach (Vector2 v in _lowerVisitableLimit)
            {
                if (destination.X >= currentPos.X && destination.X <= currentPos.X + v.X)
                {
                    float k = v.Y / v.X;
                    if (destination.Y - currentPos.Y < currentPos.X * k)
                        destination.Y = currentPos.Y + v.X * k;
                    break;
                }
                else
                    currentPos += v;
            }

            destination -= _vuePosition;
            _vuePosition = _vuePosition + destination;
            return destination;
        }
    }
}
