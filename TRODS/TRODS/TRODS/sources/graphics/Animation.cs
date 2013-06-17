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
                // ...
            }
            public void Update( /* ... */ )
            {
                // ...
            }
            public void WindowResized( /* ... */ )
            {
                // ...
            }
        }
        private List<AnimPictures> animation;
        private Rectangle _windowSize;

        public Animation(Rectangle windowSize)
        {
            animation = new List<AnimPictures>();
            _windowSize = windowSize;
        }

        public void Start()
        {
            // foreach ...
            /*for (int i = 0; i < animation.Count; i++)
            {
                animation[i].running = true;
                animation[i]._timerStart = animation[i].t1;
                animation[i]._timerLifeTime = animation[i].t2;
                animation[i].Image.Position = animation[i].PosInitiale;
            }*/
        }

        public override void LoadContent(ContentManager content)
        {
            foreach (AnimPictures ap in animation)
                ap.Image.LoadContent(content);
        }

        public override void Update(float elapsedTime)
        {
            /*for (int i = 0; i < animation.Count; i++)
            {
                if (animation[i].running && animation[i]._timerStart <= 0)
                {
                    animation[i].Image.Update(elapsedTime);
                    animation[i]._timerLifeTime -= (int)elapsedTime;
                }
                else if (animation[i].running)
                {
                    animation[i]._timerStart -= (int)elapsedTime;
                }
                if (animation[i]._timerLifeTime <= 0)
                {
                    animation[i].running = false;
                }
            }*/
            // ...
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < animation.Count; i++)
            {
                if (animation[i].running && animation[i]._timerStart <= 0)
                {
                    byte alpha = 255;
                    if (animation[i].startFondu && (animation[i]._timerLifeTime > animation[i].t2 - animation[i]._timerFondu1))
                        alpha = (byte)((float)(animation[i].t2 - animation[i]._timerLifeTime) / (float)animation[i]._timerFondu1 * 255f);
                    else if (animation[i].endFondu && animation[i]._timerLifeTime <= animation[i]._timerFondu2)
                        alpha = (byte)((float)animation[i]._timerLifeTime / (float)animation[i]._timerFondu2 * 255f);
                    animation[i].Image.Draw(spriteBatch, alpha);
                }
            }
        }

        public void Add(Sprite img, Rectangle positionInitiale, Rectangle positionFinale, int startTime, int lifeTime, bool startfondu = false, int fonduTime1 = 400, bool endfondu = false, int fonduTime2 = 400)
        {
            animation.Add(new AnimPictures(img, positionInitiale, positionFinale, startTime, lifeTime, startfondu, fonduTime1, endfondu, fonduTime2));
        }

        public override void WindowResized(Rectangle rect)
        {
            // foreach ...
            for (int i = 0; i < animation.Count; i++)
            {
                //animation[i].Image.windowResized(rect);
                /*animation[i].Image.Position = animation[i].PosInitiale = new Rectangle((int)((float)rect.X / (float)_windowSize.X * (float)animation[i].PosInitiale.X),
                    (int)((float)rect.Y / (float)_windowSize.Y * (float)animation[i].PosInitiale.Y),
                    (int)((float)rect.Width / (float)_windowSize.Width * (float)animation[i].PosInitiale.Width),
                    (int)((float)rect.Height / (float)_windowSize.Height * (float)animation[i].PosInitiale.Height));
                animation[i].PosFinale = new Rectangle((int)((float)rect.X / (float)_windowSize.X * (float)animation[i].PosFinale.X),
                    (int)((float)rect.Y / (float)_windowSize.Y * (float)animation[i].PosFinale.Y),
                    (int)((float)rect.Width / (float)_windowSize.Width * (float)animation[i].PosFinale.Width),
                    (int)((float)rect.Height / (float)_windowSize.Height * (float)animation[i].PosFinale.Height));*/
            }
            _windowSize = rect;
        }
    }
}
