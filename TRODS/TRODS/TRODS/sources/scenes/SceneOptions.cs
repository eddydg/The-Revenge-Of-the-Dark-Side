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
    class SceneOptions : AbstractScene
    {
        private KeyboardState _keyboardState;
        private MouseState _mouseState;
        private Rectangle _windowSize;
        private Sprite _wallpaper;
        private AnimatedSprite _mouse;
        public static string SOUND_FILENAME = "files/sound";
        private Sprite _textMusic;
        private Sprite _textEffects;
        private Sprite _soundMusic;
        private Sprite _soundEffect;
        private float _volumeMusic;
        private float _volumeEffect;

        public SceneOptions(Rectangle windowSize, KeyboardState keyboardState, MouseState mouseState)
        {
            _windowSize = windowSize;
            _keyboardState = keyboardState;
            _mouseState = mouseState;

            _wallpaper = new Sprite(new Rectangle(0, 0, _windowSize.Width, _windowSize.Height), _windowSize, "menu/wallpaper");
            _mouse = new AnimatedSprite(new Rectangle(-100, -100, 80, 100), windowSize, "sprites/cursorFire_8x4r", 8, 4, 40);
            _textMusic = new Sprite(new Rectangle(70, 433, 110, 40), _windowSize, "menu/soundMusic");
            _textEffects = new Sprite(new Rectangle(150, 490, 110, 40), _windowSize, "menu/soundEffect");
            _soundMusic = new Sprite(new Rectangle(180, 423, 110, 55), _windowSize, "menu/soundBars");
            _soundEffect = new Sprite(new Rectangle(260, 480, 110, 55), _windowSize, "menu/soundBars");
        }

        public override void LoadContent(ContentManager content)
        {
            _wallpaper.LoadContent(content);
            _mouse.LoadContent(content);
            _soundMusic.LoadContent(content);
            _soundEffect.LoadContent(content);
            _textMusic.LoadContent(content);
            _textEffects.LoadContent(content);

            List<string> par = EugLib.IO.Tools.toArgv(EugLib.IO.FileStream.readFile(SOUND_FILENAME));
            if (par.Count < 2 ||
                !float.TryParse(par.ElementAt<string>(0), out _volumeMusic) ||
                !float.TryParse(par.ElementAt<string>(1), out _volumeEffect))
            {
                _volumeEffect = 1f;
                _volumeMusic = 1f;
            }
        }

        public override void HandleInput(KeyboardState newKeyboardState, MouseState new_mouseState, Game1 parent)
        {
            if (_windowSize != parent.Window.ClientBounds)
            {
                _windowSize = parent.Window.ClientBounds;
                WindowResized(parent.Window.ClientBounds);
            }

            if (new_mouseState != _mouseState)
                _mouse.Position = new Rectangle(new_mouseState.X, new_mouseState.Y, _mouse.Position.Width, _mouse.Position.Height);

            if (newKeyboardState.IsKeyDown(Keys.Escape) && !_keyboardState.IsKeyDown(Keys.Escape))
                parent.SwitchScene(Scene.MainMenu);

            if (new_mouseState.LeftButton == ButtonState.Pressed)
            {
                Rectangle click = new Rectangle(new_mouseState.X, new_mouseState.Y, 1, 1);

                if (_soundEffect.Position.Intersects(click))
                    _volumeEffect = (float)(click.X - _soundEffect.Position.X) / (float)_soundEffect.Position.Width;
                if (_soundMusic.Position.Intersects(click))
                    _volumeMusic = (float)(click.X - _soundMusic.Position.X) / (float)_soundMusic.Position.Width;

                parent.son.MusiquesVolume = _volumeMusic;
                parent.son.SonsVolume = _volumeEffect;

                EugLib.IO.FileStream.writeFile(SOUND_FILENAME, _volumeMusic + " " + _volumeEffect);
            }

            _keyboardState = newKeyboardState;
            _mouseState = new_mouseState;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            _wallpaper.Draw(spriteBatch);
            _soundMusic.Draw(spriteBatch, 50);
            _soundMusic.Draw(spriteBatch,
                                new Rectangle(_soundMusic.Position.X, _soundMusic.Position.Y, (int)(_volumeMusic * _soundMusic.Position.Width), _soundMusic.Position.Height),
                                new Rectangle(0, 0, (int)(_volumeMusic * _soundMusic.Texture.Width), _soundMusic.Texture.Height),
                                Color.White);
            _soundEffect.Draw(spriteBatch, 50);
            _soundEffect.Draw(spriteBatch,
                                new Rectangle(_soundEffect.Position.X, _soundEffect.Position.Y, (int)(_volumeEffect * _soundEffect.Position.Width), _soundEffect.Position.Height),
                                new Rectangle(0, 0, (int)(_volumeEffect * _soundEffect.Texture.Width), _soundEffect.Texture.Height),
                                Color.White);
            _textMusic.Draw(spriteBatch);
            _textEffects.Draw(spriteBatch);
            _mouse.Draw(spriteBatch);

            spriteBatch.End();
        }

        public override void Update(float elapsedTime)
        {
            _mouse.Update(elapsedTime);
        }

        public override void WindowResized(Rectangle rect)
        {
            _wallpaper.windowResized(rect);
            _soundEffect.windowResized(rect);
            _soundMusic.windowResized(rect);
            _textMusic.windowResized(rect);
            _textEffects.windowResized(rect);
        }
    }
}
