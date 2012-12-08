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
    class SceneExtras : AbstractScene
    {
        private KeyboardState _keyboardState;
        private MouseState _mouseState;
        private Rectangle _windowSize;

        private List<AnimatedSprite> sprites;
        private AnimatedSprite mouse;

        public SceneExtras(Rectangle windowSize)
        {
            _windowSize = windowSize;

            sprites = new List<AnimatedSprite>();
            mouse = new AnimatedSprite(new Rectangle(-100, -100, 40, 65), _windowSize,5,2,25);
        }

        public override void LoadContent(ContentManager content)
        {
            mouse.LoadContent(content, "cursor2");
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
            mouse.Update(elapsedTime);
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

            try
            {
                if (newMouseState.LeftButton == ButtonState.Pressed && _mouseState.LeftButton != ButtonState.Pressed)
                {
                    sprites.Add(new AnimatedSprite(new Rectangle(newMouseState.X - 133, newMouseState.Y - 133, 266, 266), _windowSize, 8, 6, 30));
                    sprites.Last<AnimatedSprite>().LoadContent(parent.Content, "explosion1");
                }
                if (newMouseState.RightButton == ButtonState.Pressed && _mouseState.RightButton != ButtonState.Pressed)
                {
                    sprites.Add(new AnimatedSprite(new Rectangle(newMouseState.X - 80, newMouseState.Y - 80, 160,160), _windowSize, 8, 4, 30));
                    sprites.Last<AnimatedSprite>().LoadContent(parent.Content, "explosion2");
                }
                if (newMouseState.MiddleButton == ButtonState.Pressed && _mouseState.MiddleButton != ButtonState.Pressed)
                {
                    sprites.Add(new AnimatedSprite(new Rectangle(newMouseState.X - 66, newMouseState.Y - 66, 132, 132), _windowSize, 5, 5, 25));
                    sprites.Last<AnimatedSprite>().LoadContent(parent.Content, "explosion3");
                }
            }
            catch (Exception e)
            {
                EugLib.FileStream.toStdOut("Loading ressources error.");
                EugLib.FileStream.toStdOut(e.ToString());
                sprites.RemoveAt(sprites.Count - 1);
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
            foreach (AnimatedSprite p in sprites)
                p.Dispose();
            sprites.Clear();
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
            mouse.windowResized(rect);
        }
    }
}
