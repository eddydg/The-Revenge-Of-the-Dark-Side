﻿using System;
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
    class Attac : AbstractScene
    {
        private AnimatedSprite _sprite;
        public AnimatedSprite Sprite
        {
            get { return _sprite; }
            private set { _sprite = value; }
        }
        public Rectangle Position { get { return _sprite != null ? _sprite.Position : new Rectangle(); } }
        public int _duree { get; set; }

        public Attac(AnimatedSprite s, int duree = 10)
        {
            _duree = duree;
            _sprite = s;
        }

        public override void Draw(SpriteBatch s)
        {
            _sprite.Draw(s);
        }
        public override void Update(float elapsedTime)
        {
            _duree -= (int)elapsedTime;
            _sprite.Update(elapsedTime);
        }
        public bool Over()
        {
            return _duree < 0;
        }

        public override void WindowResized(Rectangle rect)
        {
            _sprite.windowResized(rect);
        }
    }
}
