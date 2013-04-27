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
        public Random Rand
        {
            get { return _rand; }
            private set { _rand = value; }
        }
        private int _attackSpeed;
        private int _attackDistance;
        private float _timer;
        private bool _isNearPerso;
        public bool IsNearPerso
        {
            get { return _isNearPerso; }
            private set { _isNearPerso = value; }
        }
        public bool _attack { get; set; }
        private Rectangle _windowSize;
        private Vector2 _deplacement;
        public Vector2 Deplacement
        {
            get { return _deplacement; }
            set { _deplacement = value; }
        }
        private int _visionRange;

        public IA(Rectangle windowsize, int seed, Vector2 speed,int attacDistance, int visionRange, int attackSpeed = 300)
        {
            _attackDistance=attacDistance;
            this._attackSpeed = attackSpeed;
            this._timer = 0;
            _isNearPerso = false;
            this._windowSize = windowsize;
            this._attack = false;
            _rand = new Random(seed);
            _deplacement = new Vector2(1, 1);
            this._visionRange = visionRange;
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
            _isNearPerso = isNear(posPerso, posMob,_attackDistance);
            if (!_isNearPerso && isNear(posPerso, posMob, _visionRange))
                _deplacement = new Vector2(posPerso.X - posMob.X, posPerso.Y - posMob.Y);
            else if (_isNearPerso)
                _deplacement = Vector2.Zero;
            else if (!playingZone.Contains(new Rectangle((int)posMob.X, (int)posMob.Y, 1, 1)))
            {
                _deplacement = new Vector2((float)_rand.Next(playingZone.Width) + (float)playingZone.X - posMob.X, (float)_rand.Next(playingZone.Height) + (float)playingZone.Y - posMob.Y);
            }
            _deplacement.Normalize();
        }

        private bool isNear(Vector2 posPerso, Vector2 posMob,int distance)
        {
            return (posPerso.X - posMob.X) * (posPerso.X - posMob.X) + (posPerso.Y - posMob.Y) * (posPerso.Y - posMob.Y) <= distance * distance;
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
            _visionRange = (int)((float)_visionRange * x);
        }
    }
}