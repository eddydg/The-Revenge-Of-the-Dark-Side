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
        private Sprite _lifeSprite;

        public Mob(Rectangle winSize, int seed, Vector2 position, int width, int height, string assetName, int textureColumns, int textureLines, Vector2 speed, Vector2 mapSpeed, int attackSpeed, int attackDistance, Rectangle playingZone)
            : base(winSize, position, width, height, assetName, textureColumns, textureLines)
        {
            _ia = new IA(winSize, seed, speed, attackDistance, 300, attackSpeed);
            this._playingZone = playingZone;
            this._mapSpeed = mapSpeed;
            _speed = speed;
            Life = 1;
            _lifeSprite = new Sprite(new Rectangle(_sprite.Position.X, _sprite.Position.Y - (int)((1f / 30f) * (float)_sprite.Position.Height), _sprite.Position.Width, (int)((1f / 30f) * (float)_sprite.Position.Height)), _windowSize, "game/life_mob");
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            _lifeSprite.Draw(spriteBatch, 30);
            _lifeSprite.Draw(spriteBatch, new Rectangle(_lifeSprite.Position.X, _lifeSprite.Position.Y, (int)((float)_lifeSprite.Position.Width * Life), _lifeSprite.Position.Height), new Rectangle(0, 0, (int)((float)_lifeSprite.Position.Width * Life), _lifeSprite.Position.Height), Color.FromNonPremultiplied(255, 255, 255, 200));
        }
        public override void Update(float elapsedTime)
        {
            base.Update(elapsedTime);
            _ia.Update(elapsedTime);
            if (_timer < 0)
            {

                if (!_ia.IsNearPerso)
                    _position += _ia.Deplacement * _speed;
                else if (!_ia._attack)
                    Stand(_direction);
                if (_ia._attack && _action != CharacterActions.Attack1Left && _action != CharacterActions.Attack1Right)
                {
                    _action = _direction ? CharacterActions.Attack1Right : CharacterActions.Attack1Left;
                    _canMove = false;
                    _timer = 500;
                    actualizeSpriteGraphicalBounds();
                    // ADDATTACK
                }
                else if (_canMove && !_ia.IsNearPerso)
                    Move(_ia.Deplacement.X > 0);
            }
            _lifeSprite.setRelatvePos(new Rectangle(_sprite.Position.X+_sprite.Position.Width/4, _sprite.Position.Y - (int)((1f / 20f) * (float)_sprite.Position.Height), _sprite.Position.Width-_sprite.Position.Width/2, (int)((1f / 20f) * (float)_sprite.Position.Height)), _windowSize.Width, _windowSize.Height);
            Life += ((float)_ia.Rand.Next(10) - 5f) / 50f;
            actualizeSpritePosition();
        }
        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);
            _lifeSprite.LoadContent(content);
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
            if (_ia.IsNearPerso)
                _direction = posPerso.X > Position.X;
            _ia.Actualize(posPerso, _position, _playingZone);
        }

        public override void WindowResized(Rectangle rect)
        {
            float x = (float)rect.Width / (float)_windowSize.Width;
            float y = (float)rect.Height / (float)_windowSize.Height;

            _lifeSprite.windowResized(rect, _windowSize);
            base.WindowResized(rect);
            _ia.WindowResized(rect);

            PlayingZone = new Rectangle(
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
