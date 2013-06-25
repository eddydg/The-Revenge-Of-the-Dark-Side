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
    class Animation : AbstractScene
    {
        private class AnimPictures
        {
            Sprite image;
            public Sprite Image
            {
                get { return image; }
                set { image = value; }
            }

            Rectangle _posInitiale, _posFinale;
            public Rectangle PosInitiale
            {
                get { return _posInitiale; }
                set { _posInitiale = value; }
            }
            public Rectangle PosFinale
            {
                get { return _posFinale; }
                set { _posFinale = value; }
            }
            public int _timerStart, _timerLifeTime, _timerFondu1, _timerFondu2, t1, t2;
            public bool running, startFondu, endFondu;

            public AnimPictures(Sprite img, Rectangle positionInitiale, Rectangle positionFinale, int startTime, int lifeTime, bool startfondu = false, int fonduTime1 = 400, bool endfondu = false, int fonduTime2 = 400)
            {
                image = img;
                _posInitiale = positionInitiale;
                _posFinale = positionFinale;
                image.Vitesse = 1;
                image.Direction = new Vector2((float)(_posFinale.X - _posInitiale.X) / (float)lifeTime, (float)(_posFinale.Y - _posInitiale.Y) / (float)lifeTime);
                image.Position = _posInitiale;
                _timerStart = t1 = startTime;
                _timerLifeTime = t2 = lifeTime;
                running = false;
                this.startFondu = startfondu;
                this.endFondu = endfondu;
                _timerFondu1 = fonduTime1;
                _timerFondu2 = fonduTime2;

            }

            public void Start()
            {
                running = true;
                _timerStart = t1;
                _timerLifeTime = t2;
                Image.Position = PosInitiale;
            }
            public void Update(float elapsedTime)
            {
                if (running && _timerStart <= 0)
                {
                    Image.Update(elapsedTime);
                    _timerLifeTime -= (int)elapsedTime;

                    float p = ((float)t2 - (float)_timerLifeTime) / (float)t2;
                    Rectangle position = PosInitiale;
                    position.X += (int)((float)(PosFinale.X - PosInitiale.X) * p);
                    position.Y += (int)((float)(PosFinale.Y - PosInitiale.Y) * p);
                    position.Width += (int)((float)(PosFinale.Width - PosInitiale.Width) * p);
                    position.Height += (int)((float)(PosFinale.Height - PosInitiale.Height) * p);
                    Image.Position = position;
                }
                else if (running)
                {
                    _timerStart -= (int)elapsedTime;
                }
                if (_timerLifeTime <= 0)
                {
                    running = false;
                }
            }
            public void Draw(SpriteBatch spriteBatch)
            {
                if (running && _timerStart <= 0)
                {
                    byte alpha = 255;
                    if (startFondu && (_timerLifeTime > t2 - _timerFondu1))
                        alpha = (byte)((float)(t2 - _timerLifeTime) / (float)_timerFondu1 * 255f);
                    else if (endFondu && _timerLifeTime <= _timerFondu2)
                        alpha = (byte)((float)_timerLifeTime / (float)_timerFondu2 * 255f);
                    Image.Draw(spriteBatch, alpha);
                }
            }
            public void WindowResized(Rectangle rect, Rectangle _windowSize)
            {
                _posInitiale = new Rectangle((int)((float)rect.X / (float)_windowSize.X * (float)_posInitiale.X),
                    (int)((float)rect.Y / (float)_windowSize.Y * (float)_posInitiale.Y),
                    (int)((float)rect.Width / (float)_windowSize.Width * (float)_posInitiale.Width),
                    (int)((float)rect.Height / (float)_windowSize.Height * (float)_posInitiale.Height));
                _posFinale = new Rectangle((int)((float)rect.X / (float)_windowSize.X * (float)_posFinale.X),
                    (int)((float)rect.Y / (float)_windowSize.Y * (float)_posFinale.Y),
                    (int)((float)rect.Width / (float)_windowSize.Width * (float)_posFinale.Width),
                    (int)((float)rect.Height / (float)_windowSize.Height * (float)_posFinale.Height));
                image.setRelatvePos(image.Position, _windowSize.Width, _windowSize.Height);
                image.windowResized(rect);
            }
        }

        private List<AnimPictures> animation;
        private Rectangle _windowSize;
        private Scene _nextScene;
        public Scene NextScene
        {
            get { return _nextScene; }
            set { _nextScene = value; }
        }
        private KeyboardState _keyboardState;
        private Musiques _music;
        private List<int> _textToShow;

        public Animation(Rectangle windowSize, Scene nextScene = Scene.MainMenu, Musiques musique = Musiques.None)
        {
            animation = new List<AnimPictures>();
            _windowSize = windowSize;
            _nextScene = nextScene;
            _music = musique;
            _textToShow = new List<int>();
        }

        public void Start()
        {
            foreach (AnimPictures ap in animation)
                ap.Start();
        }

        public override void LoadContent(ContentManager content)
        {
            foreach (AnimPictures ap in animation)
                ap.Image.LoadContent(content);
        }

        public override void Update(float elapsedTime)
        {
            foreach (AnimPictures ap in animation)
                ap.Update(elapsedTime);
        }

        public override void HandleInput(KeyboardState newKeyboardState, MouseState newMouseState, Game1 parent)
        {
            if (newKeyboardState.IsKeyDown(Keys.Space) && _keyboardState.IsKeyUp(Keys.Space) || Over())
                parent.SwitchScene(_nextScene);
            _keyboardState = newKeyboardState;
            if (parent.Window.ClientBounds != _windowSize)
                WindowResized(parent.Window.ClientBounds);
            foreach (AnimPictures ap in animation)
                ap.Image.HandleInput(newKeyboardState, newMouseState, parent);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            foreach (AnimPictures ap in animation)
                ap.Draw(spriteBatch);
            spriteBatch.End();
        }

        public void Add(Sprite img, Rectangle positionInitiale, Rectangle positionFinale, int startTime, int lifeTime, bool startfondu = false, int fonduTime1 = 400, bool endfondu = false, int fonduTime2 = 400)
        {
            animation.Add(new AnimPictures(img, positionInitiale, positionFinale, startTime, lifeTime, startfondu, fonduTime1, endfondu, fonduTime2));
            try
            {
                if (((TextSprite)img).Showing)
                    _textToShow.Add(animation.Count - 1);
            }
            catch (Exception) { }
        }

        public void Add(string assetName, Rectangle positionInitiale, Rectangle positionFinale, int startTime, int lifeTime, bool startfondu = false, int fonduTime1 = 400, bool endfondu = false, int fonduTime2 = 400)
        {
            animation.Add(new AnimPictures(new Sprite(positionInitiale, _windowSize, assetName), positionInitiale, positionFinale, startTime, lifeTime, startfondu, fonduTime1, endfondu, fonduTime2));
        }

        public void Add(string assetName, Rectangle position, int startTime, int lifeTime, int fonduTime1 = 400, int fonduTime2 = 400)
        {
            animation.Add(new AnimPictures(new Sprite(position, _windowSize, assetName), position, position, startTime, lifeTime, true, fonduTime1, true, fonduTime2));
        }

        public override void WindowResized(Rectangle rect)
        {/*
            foreach (AnimPictures ap in animation)
                ap.WindowResized(rect, _windowSize);
            _windowSize = rect;*/
        }

        public bool Over()
        {
            foreach (AnimPictures a in animation)
            {
                if (a._timerLifeTime > 0)
                    return false;
            }
            return true;
        }

        public override void Activation(Game1 parent = null)
        {
            Start();
            _keyboardState = Keyboard.GetState();
            Game1.son.Play(_music);
            foreach (int i in _textToShow)
            {
                try
                {
                    ((TextSprite)animation[i].Image).StartShowing();
                }
                catch (Exception) { }
            }
        }

        public override void EndScene(Game1 parent = null)
        {
            Game1.son.Stop();
        }
    }
}
