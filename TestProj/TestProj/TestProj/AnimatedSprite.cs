using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TestProj
{
    class AnimatedSprite
    {
        //public variables
        private Texture2D sprite { get; set; }
        /// <summary>
        /// Obtient ou définit le nombre de lignes dans la sprite.
        /// </summary>
        public int lignes { get; set; }
        /// <summary>
        /// Obtient ou définit le nombre de colonnes dans la sprite.
        /// </summary>
        public int colonnes { get; set; }        
        /// <summary>
        /// Obtient ou définit la position suivant X de l'image.
        /// </summary>
        public int XPos { get; set; }
        /// <summary>
        /// Obtient ou définit la position suivant Y de l'image.
        /// </summary>
        public int YPos { get; set; }
        /// <summary>
        /// numéro de la première image de l'animation.
        /// </summary>
        public int firstPicture { get; set; }
        /// <summary>
        /// numéro de la dernière image de l'animation.
        /// </summary>
        public int lastPicture { get; set; }
        /// <summary>
        /// Vitesse d'animation de la Sprite (en images/secondes).
        /// </summary>
        public int speed { get; set; }

        //private variables
        private double speedCount;
        private int actualPicture { get; set; }

        /// <summary>
        /// constructeur de base de la classe AnimatedSprite.
        /// </summary>
        /// <param name="texture">sprite contenant les différentes animations</param>
        /// <param name="nbLignes">nombre de lignes dans la sprite</param>
        /// <param name="nbColonnes">nombre de colonnes dans la sprite</param>
        /// <param name="vitesse">vitesse d'animation en images/secondes</param>
        public AnimatedSprite(Texture2D texture,int nbColonnes, int nbLignes,int vitesse)
        {
            firstPicture = 0;
            lastPicture = nbLignes * nbColonnes - 1;
            sprite = texture;
            lignes = nbLignes;
            colonnes = nbColonnes;
            actualPicture = -1;
            speed = vitesse;
            speedCount = 0;
            XPos = 0; YPos = 0;
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
        public AnimatedSprite(Texture2D texture, int nbColonnes, int nbLignes, int vitesse,int first, int last)
        {
            firstPicture = first - 1;
            lastPicture = last - 1;
            sprite = texture;
            lignes = nbLignes;
            colonnes = nbColonnes;
            actualPicture = firstPicture - 1;
            speed = vitesse;
            speedCount = 0;
            XPos = 0; YPos = 0;            
        }

        /// <summary>
        /// Met à jour la position de l'image.
        /// </summary>
        /// <param name="x">Abscisse en pixel</param>
        /// <param name="y">Ordonnée en pixel</param>
        public void SetPosition(int x, int y)
        {
            XPos = x;
            YPos = y;
        }

        /// <summary>
        /// Passe à l'image suivante de la sprite en gerant la vitesse de l'animation
        /// Cette fonction doit donc etre appelé à chaque Update() dans le main.
        /// </summary>
        /// <param name="elapsedTime">Temps écoulé depuis le dernier appel à la fonction.
        ///  Utiliser gameTime.ElapsedGameTime.Milliseconds</param>
        public void Next(int elapsedTime)
        {
            speedCount++;            
            if (elapsedTime!= 0 && speedCount >= (1000 / elapsedTime) / (float)speed)
            {
                actualPicture = (actualPicture == lastPicture)? firstPicture : actualPicture + 1;
                speedCount = 0;
            }
        }

        /// <summary>
        /// Affiche l'image à sa position actuelle.
        /// </summary>
        /// <param name="spriteBatch">spriteBatch</param>
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

        /// <summary>
        /// Affiche l'image à une position donnée.
        /// </summary>
        /// <param name="spriteBatch">spriteBatch</param>
        /// <param name="location">position du coin supérieur gauche de l'image</param>
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
    }
}
