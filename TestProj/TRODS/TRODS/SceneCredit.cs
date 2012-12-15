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
    class SceneCredit : AbstractScene
    {
        private KeyboardState _keyboardState;
        private Rectangle _windowSize;

        private Sprite top;

        public SceneCredit(Rectangle windowSize)
        {
            _windowSize = windowSize;
            top = new Sprite(new Rectangle(0, 0, _windowSize.Width, 2 * _windowSize.Height / 5), _windowSize);
        }

        public override void LoadContent(ContentManager content)
        {
            top.LoadContent(content, "menu/credit");
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            top.Draw(spriteBatch);

            spriteBatch.End();
        }

        public override void Update(float elapsedTime)
        {
        }

        public override void HandleInput(KeyboardState newKeyboardState, MouseState newMouseState, Game1 parent)
        {
            if (parent.Window.ClientBounds != _windowSize)
            {
                _windowSize = parent.Window.ClientBounds;
                windowResized(_windowSize);
            }
            if (newKeyboardState.IsKeyDown(Keys.Escape) && !_keyboardState.IsKeyDown(Keys.Escape))
                parent.SwitchScene(Scene.MainMenu);

            _keyboardState = newKeyboardState;
        }

        public override void Activation()
        {
        }

        public override void EndScene()
        {
        }

        /// <summary>
        /// Fonction adaptant les textures au
        /// redimensionnement de la fenetre
        /// </summary>
        /// <param name="rect">Nouvelle dimension de la fenetre obtenue par *Game1*.Window.ClientBounds()</param>
        private void windowResized(Rectangle rect)
        {
            top.windowResized(rect);
        }
    }
}
