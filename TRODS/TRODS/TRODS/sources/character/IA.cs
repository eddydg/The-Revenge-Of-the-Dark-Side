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
    class IA : AbstractScene
    {
        private Random _rand;
        private int _attackSpeed;
        private int _attackDistance;
        private float _timer;
        private bool _isNearPerso;
        public bool _attack { get; set; }
        private Rectangle _windowSize;
        private Vector2 _deplacement;
        public Vector2 Deplacement
        {
            get { return _deplacement; }
            set { _deplacement = value; }
        }

        public IA(Rectangle windowsize, int seed, Vector2 speed,int attacDistance, int attackSpeed = 300)
        {
            _attackDistance=attacDistance;
            this._attackSpeed = attackSpeed;
            this._timer = 0;
            _isNearPerso = false;
            this._windowSize = windowsize;
            this._attack = false;
            _rand = new Random(seed);
            _deplacement = new Vector2(1, 1);
            _deplacement.Normalize();
        }

        public override void Update(float elapsedTime)
        {
            if (_isNearPerso)
            {
                _timer += elapsedTime;
            }
            else
            {
                _timer = 0;
                _attack = false;
            }
            if (_timer >= _attackSpeed)
            {
                _attack = true;
                _timer = 0;
            }
            else
            {
                _attack = false;
            }
        }
        public void Actualize(Vector2 posPerso, Vector2 posMob, Rectangle playingZone)
        {
            _isNearPerso = playingZone.Contains(new Rectangle((int)posPerso.X, (int)posPerso.Y, 1, 1));
            if (!playingZone.Contains(new Rectangle((int)posMob.X, (int)posMob.Y, 1, 1)))
            {
                _deplacement = new Vector2((float)_rand.Next(playingZone.Width) + (float)playingZone.X - posMob.X, (float)_rand.Next(playingZone.Height) + (float)playingZone.Y - posMob.Y);
            }
            else if (_isNearPerso)
            {
                _deplacement = new Vector2(posPerso.X - posMob.X, posPerso.Y - posMob.Y);
            }
            _deplacement.Normalize();
        }

        public override void WindowResized(Rectangle rect)
        {
            float x = (float)rect.Width / (float)_windowSize.Width;
            float y = (float)rect.Height / (float)_windowSize.Height;
            _deplacement.X *= x;
            _deplacement.Y *= y;
            _deplacement.Normalize();
            _windowSize = rect;
            _attackDistance = (int)((float)_attackDistance * x);
        }
    }
}
