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
        private Rectangle _windowSize;
        private Vector2 _deplacement;
        private int _visionRange;

        public Random Rand
        {
            get
            {
                return this._rand;
            }
            private set
            {
                this._rand = value;
            }
        }

        public bool IsNearPerso
        {
            get
            {
                return this._isNearPerso;
            }
            private set
            {
                this._isNearPerso = value;
            }
        }

        public bool _attack { get; set; }

        public Vector2 Deplacement
        {
            get
            {
                return this._deplacement;
            }
            set
            {
                this._deplacement = value;
            }
        }

        public IA(Rectangle windowsize, int seed, Vector2 speed, int attacDistance, int visionRange, int attackSpeed = 300)
        {
            this._attackDistance = attacDistance;
            this._attackSpeed = attackSpeed;
            this._timer = 0.0f;
            this._isNearPerso = false;
            this._windowSize = windowsize;
            this._attack = false;
            this._rand = new Random(seed);
            this._deplacement = new Vector2(1f, 1f);
            this._visionRange = visionRange;
            this._deplacement.Normalize();
        }

        public override void Update(float elapsedTime)
        {
            if (this._isNearPerso)
            {
                this._timer += elapsedTime;
            }
            else
            {
                this._timer = 0.0f;
                this._attack = false;
            }
            if ((double)this._timer >= (double)this._attackSpeed)
            {
                this._attack = true;
                this._timer = 0.0f;
            }
            else
                this._attack = false;
        }

        public void Actualize(Vector2 posPerso, Vector2 posMob, Rectangle playingZone)
        {
            this._isNearPerso = this.isNear(posPerso, posMob, this._attackDistance);
            if (!this._isNearPerso && this.isNear(posPerso, posMob, this._visionRange))
                this._deplacement = new Vector2(posPerso.X - posMob.X, posPerso.Y - posMob.Y);
            else if (this._isNearPerso)
                this._deplacement = Vector2.Zero;
            else if (!playingZone.Contains(new Rectangle((int)posMob.X, (int)posMob.Y, 1, 1)))
                this._deplacement = new Vector2((float)this._rand.Next(playingZone.Width) + (float)playingZone.X - posMob.X, (float)this._rand.Next(playingZone.Height) + (float)playingZone.Y - posMob.Y);
            this._deplacement.Normalize();
        }

        private bool isNear(Vector2 posPerso, Vector2 posMob, int distance)
        {
            return ((double)posPerso.X - (double)posMob.X) * ((double)posPerso.X - (double)posMob.X) + ((double)posPerso.Y - (double)posMob.Y) * ((double)posPerso.Y - (double)posMob.Y) <= (double)(distance * distance);
        }

        public override void WindowResized(Rectangle rect)
        {
            float num1 = (float)rect.Width / (float)this._windowSize.Width;
            float num2 = (float)rect.Height / (float)this._windowSize.Height;
            this._deplacement.X *= num1;
            this._deplacement.Y *= num2;
            this._deplacement.Normalize();
            this._windowSize = rect;
            this._attackDistance = (int)((double)this._attackDistance * (double)num1);
            this._visionRange = (int)((double)this._visionRange * (double)num1);
        }
    }
}
