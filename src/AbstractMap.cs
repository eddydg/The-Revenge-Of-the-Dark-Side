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
        private List<Sprite> _elementsBackground;

        /// <summary>
        /// Liste des elements de la lane
        /// </summary>
        private List<Sprite> _elementsMainground;

        /// <summary>
        /// Liste des elements de premier plan
        /// </summary>
        private List<Sprite> _elementsForeground;

        /// <summary>
        /// Taille de la fenetre sur l'ecran
        /// </summary>
        private Rectangle _windowSize;

        /// <summary>
        /// Taille et position de la map
        /// </summary>
        private Rectangle _mapDimension;

        /// <summary>
        /// Position actulle du point de vue
        /// </summary>
        private Vector2 _vuePosition;

        /// <summary>
        /// Liste de Vecteurs delimitant la hauteur
        /// maximale visitable de la map
        /// </summary>
        private List<Vector2> _upperVisitableLimit;

        /// <summary>
        /// Liste de Vecteurs delimitant la hauteur
        /// minimale visitable de la map
        /// </summary>
        private List<Vector2> _lowerVisitableLimit;

        /// <summary>
        /// Abcisse minimale visitable de la map
        /// </summary>
        private int _leftVisitableLimit;

        /// <summary>
        /// Abcisse maximale visitable de la map
        /// </summary>
        private int _rightVisitableLimit;

        /// <summary>
        /// Constructeur Par defaut
        /// </summary>
        /// <param name="windowSize">Taille de la fenetre sur l'ecran
        /// Obtenue pat *Game*.Window.ClientBounds()</param>
        public AbstractMap(Rectangle windowSize)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="windowSize"></param>
        /// <param name="elementsBackground"></param>
        /// <param name="elementsMainground"></param>
        /// <param name="elementsForeground"></param>
        /// <param name="mapDimensions"></param>
        /// <param name="vuePosition"></param>
        /// <param name="upperVisitableLimit"></param>
        /// <param name="lowerVisitableLimit"></param>
        /// <param name="leftVisitableLimit"></param>
        /// <param name="rightVisitableLimit"></param>
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
            // Mouvements independants du joueur : Moteur a particules
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            DrawForeground(spriteBatch);
            DrawBackground(spriteBatch);
            spriteBatch.End();
        }

        /// <summary>
        /// Dessine le contenu graphique de l'arriere plan et de la lane
        /// </summary>
        /// <param name="spriteBatch">Instance du gestionnaire de dessin de XNA</param>
        /// <param name="personnage">Reference du personnage</param>
        public virtual void Draw(SpriteBatch spriteBatch,byte personnage/* PERSONNAGE*/)
        {
            spriteBatch.Begin();
            DrawForeground(spriteBatch);
            // PERSONNAGE.DRAW
            DrawBackground(spriteBatch);
            spriteBatch.End();
        }

        /// <summary>
        /// Dessine le contenu graphique de l'arriere plan et de la lane
        /// spriteBatch.Begin et spriteBatch.End non declares
        /// </summary>
        /// <param name="spriteBatch">Instance du gestionnaire de dessin de XNA</param>
        public virtual void DrawBackground(SpriteBatch spriteBatch)
        {
            foreach (Sprite e in _elementsMainground)
                e.Draw(spriteBatch);
            foreach (Sprite e in _elementsBackground)
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
        /// Fonction gerant le ouvement du point de vue sur la map
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
