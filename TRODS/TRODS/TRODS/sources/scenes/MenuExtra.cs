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
    class MenuExtra : AbstractScene
    {
        private KeyboardState _keyboardState;
        private MouseState _mouseState;
        private Rectangle _windowSize;

        private List<AnimatedSprite> _images;
        private List<TextSprite> _textes;

        public MenuExtra(Rectangle windowSize, KeyboardState keyboardState, MouseState mouseState)
        {
            _windowSize = windowSize;
            _mouseState = mouseState;
            _keyboardState = keyboardState;

            _images = new List<AnimatedSprite>();
            _textes = new List<TextSprite>();
            _textes.Add(new TextSprite("SpriteFont1", _windowSize, new Rectangle(50, 50, 100, 50), Color.Gold, "Gallerie"));
        }

        public override void LoadContent(ContentManager content)
        {
            foreach (AnimatedSprite a in _images)
                a.LoadContent(content);
            foreach (TextSprite t in _textes)
                t.LoadContent(content);
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
            if (parent.Window.ClientBounds != _windowSize)
            {
                _windowSize = parent.Window.ClientBounds;
                windowResized(_windowSize);
            }
           
            _keyboardState = newKeyboardState;
            _mouseState = newMouseState;
        }

        public override void Activation(Game1 parent)
        {
            _mouseState = Mouse.GetState();
            _keyboardState = Keyboard.GetState();
        }

        public override void EndScene(Game1 parent)
        {
        }

        private void windowResized(Rectangle rect)
        {
        }
    }
}
