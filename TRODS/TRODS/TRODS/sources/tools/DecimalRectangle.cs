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
    class DecimalRectangle
    {
        public float X, Y, Width, Height;

        /// <summary>
        /// Constructeur initialisant toutes les valeurs
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <param name="w">Width</param>
        /// <param name="h">Height</param>
        public DecimalRectangle(float x = 0, float y = 0, float w = 0, float h = 0)
        {
            X = x;
            Y = y;
            Width = w;
            Height = h;
        }
        /// <summary>
        /// Assignation de toutes les valeurs
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <param name="w">Width</param>
        /// <param name="h">Height</param>
        public void SetValues(float x = 0, float y = 0, float w = 0, float h = 0)
        {
            X = x;
            Y = y;
            Width = w;
            Height = h;
        }
        /// <summary>
        /// Retourne la position du coin haut gauche
        /// </summary>
        /// <returns></returns>
        public Vector2 Position()
        {
            return new Vector2(X, Y);
        }
        /// <summary>
        /// Definit la position du rectangle.
        /// </summary>
        /// <param name="v">Position.</param>
        public void SetPosition(Vector2 v)
        {
            X = v.X;
            Y = v.Y;
        }
        /// <summary>
        /// Retourne le rectangle currespondant aux valeurs.
        /// </summary>
        /// <returns></returns>
        public Rectangle ToRectangle()
        {
            return new Rectangle((int)X, (int)Y, (int)Width, (int)Height);
        }
        /// <summary>
        /// Deplace le rectangle.
        /// </summary>
        /// <param name="speed">Vecteur mouvement.</param>
        public void Move(Vector2 speed)
        {
            X += speed.X;
            Y += speed.Y;
        }
    }
}
