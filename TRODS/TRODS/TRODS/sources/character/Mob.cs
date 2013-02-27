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
    class Mob : Character
    {
        private Rectangle _playingZone;
        public Rectangle PlayingZone
        {
            get { return _playingZone; }
            set { _playingZone = value; }
        }
        private Vector2 _mapSpeed;
        private Vector2 _speed;
        private IA _ia;

        public Mob(Rectangle winSize, int seed, Vector2 position, int width, int height, string assetName, int textureColumns, int textureLines, Vector2 speed, Vector2 mapSpeed, int attackSpeed, int attackDistance, Rectangle playingZone)
            : base(winSize, position, width, height, assetName, textureColumns, textureLines)
        {
            _ia = new IA(winSize, seed, speed, attackDistance, attackSpeed);
            this._playingZone = playingZone;
            this._mapSpeed = mapSpeed;
            _speed = speed;
        }

        public override void Update(float elapsedTime)
        {
            base.Update(elapsedTime);
            _ia.Update(elapsedTime);
            _position += _ia.Deplacement * _speed;
            if (_ia._attack)
            {
                _action = _direction ? CharacterActions.Attack1Right : CharacterActions.Attack1Left;
                _canMove = false;
                _timer = 50;
                actualizeSpriteGraphicalBounds();
            }
            else if (_canMove)
                Move(_ia.Deplacement.X > 0);
            actualizeSpritePosition();
        }

        public void Move(int x, int y)
        {
            _playingZone.X -= (int)((float)x * _mapSpeed.X);
            _playingZone.Y -= (int)((float)y * _mapSpeed.Y);
            _position.X -= (float)x * _mapSpeed.X;
            _position.Y -= (float)y * _mapSpeed.Y;
        }
        public void AddGraphicalBounds(CharacterActions action, Rectangle bounds)
        {
            _graphicalBounds.set(action, bounds);
        }
        public void Actualize(Vector2 posPerso)
        {
            _ia.Actualize(posPerso, _position, _playingZone);
        }

        public override void WindowResized(Rectangle rect)
        {
            base.WindowResized(rect);
            _ia.WindowResized(rect);

            float x = (float)rect.Width / (float)_windowSize.Width;
            float y = (float)rect.Height / (float)_windowSize.Height;

            _playingZone = new Rectangle(
                (int)((float)_playingZone.X * x),
                (int)((float)_playingZone.Y * y),
                (int)((float)_playingZone.Width * x),
                (int)((float)_playingZone.Height * y));
            _speed.X *= x;
            _speed.Y *= y;

            _windowSize = rect;
        }
    }
}
