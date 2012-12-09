using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TRODS
{
    class AnimatedSprite : Sprite
    {
        public int Lignes { get; set; }
        public int Colonnes { get; set; }
        public int Speed { get; set; }

        private int ActualPicture;
        private int FirstPicture;
        private int LastPicture;

        private int _elapsedTime;

        /// <summary>
        /// constructeur avancée de la classe AnimatedSprite.
        /// </summary>
        /// <param name="texture">sprite contenant les différentes animations</param>
        /// <param name="nbColonnes">nombre de colonnes dans la sprite</param>
        /// <param name="nbLignes">nombre de lignes dans la sprite</param>
        /// <param name="vitesse">vitesse d'animation en images/secondes</param>
        /// <param name="first">numéro de la première image de l'animation dans la sprite</param>
        /// <param name="last">numéro de la dernière image de l'animation dans la sprite</param>
        public AnimatedSprite(Rectangle position, Rectangle windowSize, int nbColonnes, int nbLignes, int vitesse, int first=1, int last = -1, int beginning = -1) :
            base(position, windowSize)
        {
            Lignes = nbLignes;
            Colonnes = nbColonnes;
            Speed = vitesse;
            SetPictureBounds(first, last, beginning);
            _elapsedTime = 0;
            Position = position;
        }

        public override void Update(float elapsedTime)
        {
            _elapsedTime += (int)elapsedTime;
            if (Speed > 0 && _elapsedTime >= 1000 / Speed)
            {
                _elapsedTime = 0;
                if (ActualPicture >= LastPicture)
                    ActualPicture = FirstPicture;
                else
                    ActualPicture++;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position,
                            new Rectangle((ActualPicture-1) % Colonnes * Texture.Width / Colonnes,
                                          (ActualPicture-1) / Colonnes * Texture.Height / Lignes,
                                          Texture.Width / Colonnes,
                                          Texture.Height / Lignes), Color.White);
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 location)
        {
            spriteBatch.Draw(Texture,
                            new Rectangle((int)location.X, (int)location.Y, Position.Width, Position.Height),
                            new Rectangle((ActualPicture-1) % Colonnes * Texture.Width / Colonnes,
                                          (ActualPicture-1) / Colonnes * Texture.Height / Lignes,
                                          Texture.Width / Colonnes,
                                          Texture.Height / Lignes), Color.White);
        }

        /// <summary>
        /// Modifie l'intervalle de lecture des images
        /// </summary>
        /// <param name="first">Premiere image</param>
        /// <param name="last">Derniere image</param>
        /// <param name="beginning">Image de debut de l'annimation</param>
        public void SetPictureBounds(int first, int last, int beginning=-1)
        {
            if (last > 0 && last <= Lignes * Colonnes)
                LastPicture = last;
            else
                LastPicture = Lignes * Colonnes;

            if (first > 0 && first < LastPicture)
                FirstPicture = first;
            else
                FirstPicture = LastPicture;

            if (beginning > 0 && beginning < LastPicture)
                ActualPicture = beginning;
            else
                ActualPicture = FirstPicture;

            _elapsedTime = 0;
        }

        /// <summary>
        /// Change la position de l'image
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetPosition(int x, int y)
        {
            Position = new Rectangle(x, y, Position.Width, Position.Height);
        }

        /// <summary>
        /// Renvoie true si l'animation est a la derniere image
        /// </summary>
        /// <returns>bool</returns>
        public bool IsEnd()
        {
            return ActualPicture == LastPicture;
        }

        /// <summary>
        /// Libere les textures
        /// </summary>
        public override void Dispose()
        {
            Texture.Dispose();
        }
    }

}