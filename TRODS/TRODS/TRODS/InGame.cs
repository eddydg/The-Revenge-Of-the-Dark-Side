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

        private List<AnimatedSprite> sprites;
        private Sprite mouse;

        public InGame(Rectangle windowSize)
        {
            _windowSize = windowSize;

            sprites = new List<AnimatedSprite>();
            mouse = new Sprite(new Rectangle(-100, -100, 30, 50), _windowSize);
        }

        public override void LoadContent(ContentManager content)
        {
            mouse.LoadContent(content, "menuCursor");
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            foreach (AnimatedSprite p in sprites)
                p.Draw(spriteBatch);
            mouse.Draw(spriteBatch);
            spriteBatch.End();
        }

        public override void Update(float elapsedTime)
        {
            foreach (AnimatedSprite p in sprites)
            {
                if (p.IsEnd())
                {
                    sprites.RemoveAt(sprites.IndexOf(p));
                    break;
                }
                p.Update(elapsedTime);
            }
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
            if (_mouseState != newMouseState)
                mouse.Position = new Rectangle(newMouseState.X, newMouseState.Y, mouse.Position.Width, mouse.Position.Height);


            if (newMouseState.LeftButton == ButtonState.Pressed && _mouseState.LeftButton != ButtonState.Pressed)
            {
                sprites.Add(new AnimatedSprite(new Rectangle(newMouseState.X - _windowSize.Width / 3 / 2, newMouseState.Y - _windowSize.Width / 3 / 2, _windowSize.Width / 3, _windowSize.Width / 3), _windowSize, 8, 6, 25));
                sprites.Last<AnimatedSprite>().LoadContent(parent.Content, "explosion1");
            }
            if (newMouseState.RightButton == ButtonState.Pressed && _mouseState.RightButton != ButtonState.Pressed)
            {
                sprites.Add(new AnimatedSprite(new Rectangle(newMouseState.X - _windowSize.Width / 5 / 2, newMouseState.Y - _windowSize.Width / 5 / 2, _windowSize.Width / 5, _windowSize.Width / 5), _windowSize, 8, 4, 25));
                sprites.Last<AnimatedSprite>().LoadContent(parent.Content, "explosion2");
            }
            if (newMouseState.MiddleButton == ButtonState.Pressed && _mouseState.MiddleButton != ButtonState.Pressed)
            {
                sprites.Add(new AnimatedSprite(new Rectangle(newMouseState.X - _windowSize.Width / 6 / 2, newMouseState.Y - _windowSize.Width / 6 / 2, _windowSize.Width / 6, _windowSize.Width / 6), _windowSize, 5,5, 15));
                sprites.Last<AnimatedSprite>().LoadContent(parent.Content, "explosion3");
            }

            _keyboardState = newKeyboardState;
            _mouseState = newMouseState;
        }

        /// <summary>
        /// Fonction adaptant les textures au
        /// redimensionnement de la fenetre
        /// </summary>
        /// <param name="rect">Nouvelle dimension de la fenetre obtenue par *Game1*.Window.ClientBounds()</param>
        private void windowResized(Rectangle rect)
        {
            foreach (AnimatedSprite p in sprites)
                p.windowResized(rect);
        }
    }
}