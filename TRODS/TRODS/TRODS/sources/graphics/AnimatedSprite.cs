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

        public int ActualPicture { get; set; }
        private int FirstPicture;
        public int First
        {
            get { return FirstPicture; }
            private set { FirstPicture = value; }
        }
        private int LastPicture;
        public int Last
        {
            get { return LastPicture; }
            private set { LastPicture = value; }
        }
        public bool _repeating { get; private set; }
        private int _elapsedTime;

        public AnimatedSprite(Rectangle position, Rectangle windowSize, int nbColonnes = 1, int nbLignes = 1, int vitesse = 30, int first = 1, int last = -1, int beginning = -1, bool repeating = false) :
            base(position, windowSize)
        {
            Lignes = nbLignes;
            Colonnes = nbColonnes;
            Speed = vitesse;
            SetPictureBounds(first, last, beginning);
            _elapsedTime = 0;
            _repeating = repeating;
            Position = position;
        }
        public AnimatedSprite(Rectangle position, Rectangle windowSize, string assetName, int nbColonnes = 1, int nbLignes = 1, int vitesse = 30, int first = 1, int last = -1, int beginning = -1, bool repeating = false) :
            base(position, windowSize, assetName)
        {
            Lignes = nbLignes;
            Colonnes = nbColonnes;
            Speed = vitesse;
            SetPictureBounds(first, last, beginning);
            _elapsedTime = 0;
            _repeating = repeating;
            Position = position;
        }
        public AnimatedSprite(AnimatedSprite s = null) :
            base((Sprite)s)
        {
            Lignes = s.Lignes;
            Colonnes = s.Colonnes;
            ActualPicture = s.ActualPicture;
            FirstPicture = s.FirstPicture;
            LastPicture = s.LastPicture;
            _repeating = s._repeating;
            _elapsedTime = s._elapsedTime;
        }

        /// <summary>
        /// Modifie l'intervalle de lecture des images
        /// </summary>
        /// <param name="first">Premiere image</param>
        /// <param name="last">Derniere image</param>
        /// <param name="beginning">Image de debut de l'annimation</param>
        /// <param name="repeating">Repetition de l'animation entre first et last</param>
        public void SetPictureBounds(int first, int last, int beginning = -1, bool repeating = false)
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
            _repeating = repeating;
        }
        /// <summary>
        /// Change la position de l'image
        /// </summary>
        /// <param name="x">Abcisse</param>
        /// <param name="y">Ordonee</param>
        public void SetPosition(int x, int y)
        {
            Position = new Rectangle(x, y, Position.Width, Position.Height);
        }
        /// <summary>
        /// Renvoie true si l'animation est a la derniere image
        /// </summary>
        /// <returns></returns>
        public bool IsEnd()
        {
            if (_repeating)
                return ActualPicture == LastPicture || ActualPicture == Lignes * Colonnes;
            else
                return ActualPicture == Lignes * Colonnes;
        }
        /// <summary>
        /// Libere les textures
        /// </summary>
        public override void Dispose()
        {
            Texture.Dispose();
        }

        public override void Update(float elapsedTime)
        {
            base.Update(elapsedTime);
            _elapsedTime += (int)elapsedTime;
            if (Speed > 0 && _elapsedTime >= 1000 / Speed)
            {
                _elapsedTime = 0;
                if (_repeating && ActualPicture >= LastPicture)
                    ActualPicture = FirstPicture;
                else if (ActualPicture >= Lignes * Colonnes)
                    ActualPicture = FirstPicture;
                else
                    ActualPicture++;
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position,
                            new Rectangle((ActualPicture - 1) % Colonnes * Texture.Width / Colonnes,
                                          (ActualPicture - 1) / Colonnes * Texture.Height / Lignes,
                                          Texture.Width / Colonnes,
                                          Texture.Height / Lignes), Color.White);
        }
        public override void Draw(SpriteBatch spriteBatch, Vector2 location)
        {
            spriteBatch.Draw(Texture,
                            new Rectangle((int)location.X, (int)location.Y, Position.Width, Position.Height),
                            new Rectangle((ActualPicture - 1) % Colonnes * Texture.Width / Colonnes,
                                          (ActualPicture - 1) / Colonnes * Texture.Height / Lignes,
                                          Texture.Width / Colonnes,
                                          Texture.Height / Lignes), Color.White);
        }
        public override void Draw(SpriteBatch spriteBatch, Rectangle position)
        {
            spriteBatch.Draw(Texture, position,
                            new Rectangle((ActualPicture - 1) % Colonnes * Texture.Width / Colonnes,
                                          (ActualPicture - 1) / Colonnes * Texture.Height / Lignes,
                                          Texture.Width / Colonnes,
                                          Texture.Height / Lignes), Color.White);
        }
    }
}