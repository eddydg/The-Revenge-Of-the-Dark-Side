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

        private Map1 map;
        private float vitesse;

        public InGame(Rectangle wnewWindowSize)
        {
            windowSize = wnewWindowSize;
            map = new Map1(windowSize);
            vitesse = 2;
        }

        public override void LoadContent(ContentManager content)
        {
            map.LoadContent(content);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            map.Draw(spriteBatch);
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

            if (newKeyboardState.IsKeyDown(Keys.Right))
                map.Moving(new Vector2(1, 0) * vitesse);
            if (newKeyboardState.IsKeyDown(Keys.Left))
                map.Moving(new Vector2(-1, 0) * vitesse);

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