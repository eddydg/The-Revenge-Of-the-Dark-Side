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
    class SceneTitre : AbstractScene
    {
        private Rectangle _windowSize;
        private MouseState _mouseState;
        private KeyboardState _keyboardstate;

        private Sprite _wallpaper;
        private Sprite _wallpaperText;
        private Sprite _nuages;
        private Sprite _text;

        private ParticleEngine _particles;

        public SceneTitre(Rectangle windowSize,KeyboardState keyboardState,MouseState mouseState)
        {
            _windowSize = windowSize;
            _mouseState = mouseState;
            _keyboardstate = keyboardState;

            _wallpaper = new Sprite(new Rectangle(0, 0, windowSize.Width, windowSize.Height), windowSize, "menu/wallpaper");
            _wallpaperText = new Sprite(new Rectangle(0, 0, windowSize.Width, windowSize.Height), windowSize, "menu/wallpaperText");
            _nuages = new Sprite(new Rectangle(0, 0, windowSize.Width * 3, windowSize.Height), windowSize, "general/nuages0");
            _nuages.Direction = new Vector2(-1, 0);
            _nuages.Vitesse = 0.1f; // 1f = 1000 px/sec
            _text = new Sprite(new Rectangle(_windowSize.Width / 2 - 100, 4 * _windowSize.Height / 6+45, 200, 70), _windowSize, "menu/tittleText");
            _particles = new ParticleEngine(windowSize, new DecimalRectangle(0, windowSize.Height, windowSize.Width, 0), new Vector3(1, 15, 15),
                                new List<string>() { "particle/fire", "particle/smoke" }, 30, 0.2f, 1f, 90f, 20f, 0f, 360f, -2, 2, 20f, 200f);
        }

        public override void HandleInput(KeyboardState newKeyboardState, MouseState newMouseState, Game1 parent)
        {
            if (parent.Window.ClientBounds != _windowSize)
            {
                _windowSize = parent.Window.ClientBounds;
                WindowResized(_windowSize);
            }
            if (!_keyboardstate.IsKeyDown(Keys.Space) && newKeyboardState.IsKeyDown(Keys.Space))
                parent.SwitchScene(Scene.MainMenu);
            if (!_keyboardstate.IsKeyDown(Keys.Escape) && newKeyboardState.IsKeyDown(Keys.Escape))
                parent.Exit();

            _keyboardstate = newKeyboardState;
            _mouseState = newMouseState;
        }

        public override void LoadContent(ContentManager content)
        {
            _wallpaper.LoadContent(content);
            _wallpaperText.LoadContent(content);
            _nuages.LoadContent(content);
            _text.LoadContent(content);
            _particles.LoadContent(content);
        }

        public override void Update(float elapsedTime)
        {
            Rectangle p = _nuages.Position;
            if (p.X + p.Width <= 0)
                _nuages.Position = new Rectangle(0, p.Y, p.Width, p.Height);
            else
                _nuages.Update(elapsedTime);
            _particles.Update(elapsedTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            _wallpaper.Draw(spriteBatch);
            _nuages.Draw(spriteBatch);
            if (_nuages.Position.X + _nuages.Position.Width <= _windowSize.Width)
                _nuages.Draw(spriteBatch, new Vector2(_nuages.Position.X + _nuages.Position.Width,0));
            _particles.Draw(spriteBatch);
            _wallpaperText.Draw(spriteBatch);
            _text.Draw(spriteBatch);
            spriteBatch.End();
        }

        public override void WindowResized(Rectangle rect)
        {
            _wallpaper.windowResized(rect);
            _nuages.windowResized(rect);
            _wallpaperText.windowResized(rect);
            _text.windowResized(rect);
            _particles.WindowResized(rect);
        }

        public override void Activation(Game1 parent)
        {
            _keyboardstate = Keyboard.GetState();
            _mouseState = Mouse.GetState();
        }
    }
}
