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
	* Classe de test temporaire
	* Ingame sera la classe qui contiendra le moteur de jeu
	* ( La plus bg des classes les plus importantes )
	* */
    class InGame : AbstractScene
    {
        private Sprite black;
        private Sprite nuages;
        private KeyboardState kb;

        public InGame(Rectangle windowSize)
        {
            black = new Sprite(new Rectangle(0, 0, windowSize.Width, windowSize.Height), windowSize.Width, windowSize.Height);
            nuages = new Sprite(new Rectangle(0,0,windowSize.Width*3,windowSize.Height), windowSize.Width, windowSize.Height);
            nuages.Direction = new Vector2(-1, 0);
            nuages.Vitesse = 0.1f;
        }

        public override void LoadContent(ContentManager content)
        {
            black.LoadContent(content, "black");
            nuages.LoadContent(content, "nuages1");
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            black.Draw(spriteBatch);

            Rectangle np = nuages.Position;
            nuages.Draw(spriteBatch);
            if (np.X + np.Width < np.Width/3)
                nuages.DrawWith(spriteBatch, Color.White, np.X + np.Width, np.Y);

            spriteBatch.End();
        }
        public override void Update(float elapsedTime)
        {
            Rectangle p = nuages.Position;
            if (p.X + p.Width <= 0)
                nuages.Position = new Rectangle(0, p.Y, p.Width, p.Height);
            else
                nuages.Update(elapsedTime);
        }
        public override void HandleInput(KeyboardState newKeyboardState, MouseState newMouseState, Game1 parent)
        {
            if (!newKeyboardState.IsKeyDown(Keys.Escape) && kb.IsKeyDown(Keys.Escape))
                parent.SwitchScene(Scene.MainMenu);
            kb = newKeyboardState;
        }
    }
}