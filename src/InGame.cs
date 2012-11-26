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
    /// Classe de test temporaire
	/// qui contiendra le moteur de jeu
	/// ( La plus bg des classes les plus importantes )
    /// </summary>
    class InGame : AbstractScene
    {
        private KeyboardState kb;
        private Rectangle windowSize;

        public InGame(Rectangle wnewWindowSize)
        {
            windowSize = wnewWindowSize;
        }

        public override void LoadContent(ContentManager content)
        {
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();


            spriteBatch.End();
        }
        public override void Update(float elapsedTime)
        {
        }
        public override void HandleInput(KeyboardState newKeyboardState, MouseState newMouseState, Game1 parent)
        {
            if (parent.Window.ClientBounds != windowSize)
            {
                windowSize = parent.Window.ClientBounds;
                windowResized(windowSize);
            }
            if (!newKeyboardState.IsKeyDown(Keys.Escape) && kb.IsKeyDown(Keys.Escape))
                parent.SwitchScene(Scene.MainMenu);
            kb = newKeyboardState;
        }
        /// <summary>
        /// Fonction adaptant les textures au
        /// redimensionnement de la fenetre
        /// </summary>
        /// <param name="rect">Nouvelle dimension de la fenetre obtenue par *Game1*.Window.ClientBounds()</param>
        private void windowResized(Rectangle rect)
        {
            
        }
    }
}