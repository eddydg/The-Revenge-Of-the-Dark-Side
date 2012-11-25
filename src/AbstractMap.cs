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
        private List<Sprite> _elements;
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
        private Vector2 _position;
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
        /// Fonction gerant le ouvement du point de vue sur la map
        /// </summary>
        /// <param name="destination">Destination voulue</param>
        /// <returns>Destination possible</returns>
        public virtual Vector2 Moving(Vector2 destination)
        {
            return destination;
        }
    }
}
