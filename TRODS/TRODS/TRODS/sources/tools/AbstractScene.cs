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
    /// Classe dont heriteront toutes les scenes du jeu
    /// </summary>
    abstract class AbstractScene
    {
        /// <summary>
        /// Charge le contenu des textures
        /// </summary>
        /// <param name="content">Instance du gestionnaire de contenu de XNA</param>
        public virtual void LoadContent(ContentManager content)
        {
        }
        public virtual void LoadContent(Texture2D texture)
        {
        }
        /// <summary>
        /// Gere les entrees de clavier, souris et evenements
        /// </summary>
        /// <param name="newKeyboardState">Nouvel etat du clavier</param>
        /// <param name="newMouseState">Nouvel etat de la souris</param>
        /// <param name="parent">Reference de la classe parent</param>
        public virtual void HandleInput(KeyboardState newKeyboardState, MouseState newMouseState, Game1 parent)
        {
        }
        /// <summary>
        /// Met a jour la partie dynamique des variables
        /// elapsedTime represente le temps ecoule depuis
        /// le dernier appel a la fonction
        /// </summary>
        /// <param name="elapsedTime">Temps ecoule depuis le dernier appel de la fonction</param>
        public virtual void Update(float elapsedTime)
        {
        }
        /// <summary>
        /// Dessine le contenu graphique
        /// </summary>
        /// <param name="spriteBatch">Instance du gestionnaire de dessin de XNA</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
        }
        /// <summary>
        /// Fonction a appeler avant d'utiliser la scene
        /// </summary>
        /// <param name="parent">Reference de la classe parent</param>
        public virtual void Activation(Game1 parent = null)
        {
        }
        /// <summary>
        /// Fonction a appeler avant de changer de scene
        /// </summary>
        /// <param name="parent">Reference de la classe parent</param>
        public virtual void EndScene(Game1 parent = null)
        {
        }
        /// <summary>
        /// Redimensionnement de la fenetre
        /// </summary>
        /// <param name="rect">Taille de la nouvelle fenetre</param>
        public virtual void WindowResized(Rectangle rect)
        {
        }
    }
}