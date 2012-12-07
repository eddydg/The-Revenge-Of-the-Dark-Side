using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TRODS
{
    class AnimatedSprite
    {
        public Texture2D Texture { get; set; }

        public int Lignes { get; set; }
        public int Colonnes { get; set; }
        public Rectangle Position { get; set; }
        public int Speed { get; set; }

        public int ActualPicture { get; set; }
        public int FirstPicture { get; set; }
        public int LastPicture { get; set; }

        private int _elapsedTime;


        public AnimatedSprite(Texture2D texture, int nbColonnes = 1, int nbLignes = 1, int vitesse = 0)
        {
            Texture = texture;
            Lignes = nbLignes;
            Colonnes = nbColonnes;
            Speed = vitesse;
            SetPictureBounds(1, nbLignes * nbColonnes);
            _elapsedTime = 0;
            Position = new Rectangle(0, 0, texture.Width / Colonnes, texture.Height / Lignes);
        }

        /// <summary>
        /// constructeur avancée de la classe AnimatedSprite.
        /// </summary>
        /// <param name="texture">sprite contenant les différentes animations</param>
        /// <param name="nbColonnes">nombre de colonnes dans la sprite</param>
        /// <param name="nbLignes">nombre de lignes dans la sprite</param>
        /// <param name="vitesse">vitesse d'animation en images/secondes</param>
        /// <param name="first">numéro de la première image de l'animation dans la sprite</param>
        /// <param name="last">numéro de la dernière image de l'animation dans la sprite</param>
        public AnimatedSprite(Texture2D texture, int nbColonnes, int nbLignes, int vitesse, int first, int last)
        {
            Texture = texture;
            Lignes = nbLignes;
            Colonnes = nbColonnes;
            Speed = vitesse;
            SetPictureBounds(first, last);
            _elapsedTime = 0;
            Position = new Rectangle(0, 0, texture.Width / Colonnes, texture.Height / Lignes);
        }

        /// <summary>
        /// Affiche l'image suivante
        /// </summary>
        /// <param name="elapsedTime"></param>
        public void Update(int elapsedTime)
        {
            _elapsedTime += elapsedTime;
            if (Speed > 0 && _elapsedTime >= 1000 / Speed)
            {
                _elapsedTime = 0;
                if (ActualPicture >= LastPicture)
                    ActualPicture = FirstPicture;
                else
                    ActualPicture++;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position,
                            new Rectangle(ActualPicture % Colonnes * Texture.Width / Colonnes,
                                          ActualPicture / Colonnes * Texture.Height / Lignes,
                                          Texture.Width / Colonnes,
                                          Texture.Height / Lignes), Color.White);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 location)
        {
            spriteBatch.Draw(Texture,
                            new Rectangle((int)location.X, (int)location.Y, Position.Width, Position.Height),
                            new Rectangle(ActualPicture % Colonnes * Texture.Width / Colonnes,
                                          ActualPicture / Colonnes * Texture.Height / Lignes,
                                          Texture.Width / Colonnes,
                                          Texture.Height / Lignes), Color.White);
        }

        /// <summary>
        /// Modifie l'intervalle de lecture des images
        /// </summary>
        /// <param name="first">Premiere image</param>
        /// <param name="last">Derniere image</param>
        public void SetPictureBounds(int first, int last)
        {
            if (first > 0)
                FirstPicture = first;
            else
                FirstPicture = 1;

            if (last >= first && last <= Lignes * Colonnes)
                LastPicture = last;
            else
                LastPicture = Lignes * Colonnes;

            ActualPicture = FirstPicture - 1;
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
        /// Renvoie true si 
        /// </summary>
        /// <returns></returns>
        public bool IsEnd()
        {
            return ActualPicture == LastPicture;
        }

        /// <summary>
        /// Libere les textures
        /// </summary>
        public void Dispose()
        {
            Texture.Dispose();
        }
    }

}