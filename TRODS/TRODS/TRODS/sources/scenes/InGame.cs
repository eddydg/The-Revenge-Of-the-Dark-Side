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
        private KeyboardState _keyboardState;
        private MouseState _mouseState;
        private Rectangle _windowSize;
        private Sprite mouse;
        private AbstractMap map;

        public InGame(Rectangle windowSize)
        {
            _windowSize = windowSize;

            mouse = new Sprite(new Rectangle(-100, -100, 30, 50), _windowSize);
            map = new AbstractMap(_windowSize);
            map.VuePosition = new Vector2(_windowSize.Width / 2, _windowSize.Height / 2);
            map.Visitable.Add(new Rectangle(_windowSize.Width / 2, _windowSize.Height / 2, 1000, 50));
            map.Elements.Add(new AbstractMap.Element(new Sprite(new Rectangle(0, 0, _windowSize.Width, _windowSize.Height), _windowSize, "map1/sky1")));
            map.Elements.Add(new AbstractMap.Element(new Sprite(new Rectangle(0, 0, _windowSize.Width, _windowSize.Height), _windowSize, "map1/back1r")));
            map.Elements.Add(new AbstractMap.Element(new Sprite(new Rectangle(0, _windowSize.Height / 2, _windowSize.Width, _windowSize.Height / 2), _windowSize, "map1/fore1")));
        }

        public override void LoadContent(ContentManager content)
        {
            mouse.LoadContent(content, "general/cursor1");
            map.LoadContent(content);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            map.Draw(spriteBatch, true);
            mouse.Draw(spriteBatch);

            spriteBatch.End();
        }

        public override void Update(float elapsedTime)
        {
            map.Update(elapsedTime);
        }

        public override void HandleInput(KeyboardState newKeyboardState, MouseState newMouseState, Game1 parent)
        {
            if (parent.Window.ClientBounds != _windowSize)
            {
                _windowSize = parent.Window.ClientBounds;
                windowResized(_windowSize);
            }
            if (!newKeyboardState.IsKeyDown(Keys.Escape) && _keyboardState.IsKeyDown(Keys.Escape))
                parent.SwitchScene(Scene.MainMenu);
            bool isClick = false;
            if (_mouseState != newMouseState)
            {
                mouse.Position = new Rectangle(newMouseState.X, newMouseState.Y, mouse.Position.Width, mouse.Position.Height);
                isClick = newMouseState.LeftButton == ButtonState.Pressed && _mouseState.LeftButton == ButtonState.Released;
            }



            _keyboardState = newKeyboardState;
            _mouseState = newMouseState;
        }

        public override void Activation()
        {
            _mouseState = Mouse.GetState();
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
            map.WindowResized(rect);
        }
    }
}