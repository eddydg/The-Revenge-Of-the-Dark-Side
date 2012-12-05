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
        //public
        public Texture2D sprite { get; set; }
        public int lignes { get; set; }
        public int colonnes { get; set; }
        public int actualPicture { get; set; }
        public int totalPictures { get; set; }
        public int XPos { get; set; }
        public int YPos { get; set; }
        /// <summary>
        /// The speed rate for displaying the sprite (in pictures/seconds)
        /// </summary>
        public int speed { get; set; }
        //private
        private double speedCount;

        /// <summary>
        /// AnimatedSprite
        /// </summary>
        /// <param name="texture">sprite contenant les différentes animations</param>
        /// <param name="nbLignes">nombre de lignes dans la sprite</param>
        /// <param name="nbColonnes">nombre de colonnes dans la sprite</param>
        /// <param name="vitesse">vitesse d'animation en images/secondes</param>
        public AnimatedSprite(Texture2D texture,int nbColonnes, int nbLignes,int vitesse)
        {
            sprite = texture;
            lignes = nbLignes;
            colonnes = nbColonnes;
            actualPicture = -1;
            totalPictures = lignes * colonnes;
            speed = vitesse;
            speedCount = 0;
            XPos = 0; YPos = 0;
        }

        public void SetPosition(int x, int y)
        {
            XPos = x;
            YPos = y;
        }

        public void Next(int elapsedTime)
        {
            speedCount++;            
            if (elapsedTime!= 0 && speedCount >= (1000 / elapsedTime) / (float)speed)
            {
                actualPicture = (actualPicture == totalPictures-1)? 0 : actualPicture+1;
                speedCount = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 location)
        {
            XPos = (int)location.X;
            YPos = (int)location.Y;
            int largeur = sprite.Width / colonnes;
            int hauteur = sprite.Height / lignes;
            int _ligne = (int)((float)actualPicture / (float)colonnes);
            int _colonne = actualPicture % colonnes;

            Rectangle pieceOfSprite = new Rectangle(largeur * _colonne, hauteur * _ligne, largeur, hauteur);
            Rectangle drawOfSprite = new Rectangle(XPos, YPos, largeur, hauteur);

            spriteBatch.Begin();
            spriteBatch.Draw(sprite, drawOfSprite, pieceOfSprite, Color.White);
            spriteBatch.End();
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            int largeur = sprite.Width / colonnes;
            int hauteur = sprite.Height / lignes;
            int _ligne = (int)((float)actualPicture / (float)colonnes);
            int _colonne = actualPicture % colonnes;

            Rectangle pieceOfSprite = new Rectangle(largeur * _colonne, hauteur * _ligne, largeur, hauteur);
            Rectangle drawOfSprite = new Rectangle(XPos, YPos, largeur, hauteur);

            spriteBatch.Begin();
            spriteBatch.Draw(sprite, drawOfSprite, pieceOfSprite, Color.White);
            spriteBatch.End();
        }
    }
}
