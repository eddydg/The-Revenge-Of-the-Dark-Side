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
    /**
     * Classe dont heriteront toutes les scenes 
     * du jeu 
     * */
    abstract class AbstractScene
    {
        /**
         * Charge le contenu graphique etc...
         * */
        public virtual void LoadContent(ContentManager content)
        {
        }
        /**
         * Gere les entrees de clavier ou de souris
         * */
        public virtual void HandleInput(KeyboardState newKeyboardState, MouseState newMouseState, Game1 parent)
        {
        }
        /**
         * Met a jour la partie dynamique des variables
         * elapsedTime represente le temps ecoule depuis
         * le dernier appel a la fonction
         * */
        public virtual void Update(float elapsedTime)
        {
        }
        /**
         * Dessine le contenu graphique
         * */
        public virtual void Draw(SpriteBatch spriteBatch)
        {
        }
    }
}
