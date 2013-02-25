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
    class Character : AbstractScene
    {
        internal Rectangle _windowSize;
        internal GraphicalBounds<CharacterActions> _graphicalBounds;
        internal CharacterActions _action;
        internal AnimatedSprite _sprite;
        internal Vector2 _position;
        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }
        internal Physics _physics;
        public bool _canMove { get; internal set; }
        public bool _isOnGround { get; internal set; }
        public bool _jumping { get; internal set; }
        public int _jumpHeight { get; internal set; }
        internal bool _direction;
        internal int _timer;
        internal List<Attac> _attacks;
        public List<Attac> AttackList
        {
            get { return _attacks; }
            set { _attacks = value; }
        }
        internal Weapon _weapon;

        internal KeyboardState _oldKeyboardState;
        internal MouseState _oldMouseState;

        public Character(Rectangle winSize, Vector2 position, int width, int height,
            string assetName, int textureColumns, int textureLines)
        {
            _windowSize = winSize;
            _position = position;
            _graphicalBounds = new GraphicalBounds<CharacterActions>(new Dictionary<CharacterActions, Rectangle>());
            _attacks = new List<Attac>();
            _sprite = new AnimatedSprite(new Rectangle((int)position.X - width / 2, (int)position.Y - height, width, height), winSize, assetName, textureColumns, textureLines, 30, 1, -1, -1, true);
            _canMove = true;
            _jumping = false;
            _jumpHeight = 0;
            _direction = true; // = right
            _timer = 0;
            _physics = new Physics();
            _action = CharacterActions.StandRight;
        }

        public override void LoadContent(ContentManager content)
        {
            _sprite.LoadContent(content);
            if (_weapon != null)
                _weapon.LoadContent(content);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!_jumping)
            {
                _sprite.Draw(spriteBatch);
                if (_weapon != null)
                    _weapon.Draw(spriteBatch, _sprite.Position);
            }
            else
            {
                _sprite.Draw(spriteBatch, new Vector2(_sprite.Position.X, _sprite.Position.Y - _jumpHeight));
                if (_weapon != null)
                    _weapon.Draw(spriteBatch, new Rectangle(_sprite.Position.X, _sprite.Position.Y - _jumpHeight, _sprite.Position.Width, _sprite.Position.Height));
            }
        }
        public override void Update(float elapsedTime)
        {
            _timer -= (int)elapsedTime;
            _sprite.Update(elapsedTime);
            _weapon.Update(elapsedTime);
            if (!_sprite._repeating && _sprite.IsEnd())
                Stand(_direction);
            if (_jumping)
                _jumpHeight += _physics.Update(elapsedTime);
            testOnGround();
            if (_timer < 0 && _canMove == false)
            {
                _timer = 0;
                _canMove = true;
                Stand(_direction);
                /*if (_direction) // right
                    _action = Actions.StandRight;
                else
                    _action = Actions.StandLeft;*/
            }
            for (int i = 0; i < _attacks.Count; i++)
            {
                _attacks[i].Update(elapsedTime);
                if (_attacks[i]._duree < 0)
                {
                    _attacks.RemoveAt(i);
                    i--;
                }
            }
        }
        public override void WindowResized(Rectangle rect)
        {
            _sprite.windowResized(rect);
            if (_weapon != null)
                _weapon.WindowResized(rect);

            float x = (float)rect.Width / (float)_windowSize.Width, y = (float)rect.Height / (float)_windowSize.Height;

            _physics.WindowResized(y);

            _position.X *= x;
            _position.Y *= y;

            _jumpHeight = (int)((float)_jumpHeight * y);

            foreach (Attac a in _attacks)
            {
                a.Sprite.windowResized(rect);
            }

            _windowSize = rect;
        }

        public void Stand(bool right)
        {
            _direction = right;
            if (right)
                _action = CharacterActions.StandRight;
            else
                _action = CharacterActions.StandLeft;
            actualizeSpriteGraphicalBounds();
        }
        public void Jump()
        {
            if (_canMove && !_jumping)
            {
                if (_direction)
                    _action = CharacterActions.JumpRight;
                else
                    _action = CharacterActions.JumpLeft;
                actualizeSpriteGraphicalBounds();
                _jumpHeight = 0;
                _jumping = true;
                _isOnGround = false;
                _physics.Jump();
            }
        }
        public void Move(bool right)
        {
            if (_canMove)
            {
                _direction = right;
                if (right)
                    _action = CharacterActions.WalkRight;
                else
                    _action = CharacterActions.WalkLeft;
                actualizeSpriteGraphicalBounds();
            }
        }
        public void Paralize(int time)
        {
            _canMove = false;
            _timer = time;
            _action = CharacterActions.Paralized;
        }
        public void Free()
        {
            _canMove = true;
        }
        public void DoubleDash()
        {
        }

        internal bool testOnGround()
        {
            if (_jumpHeight < 0)
            {
                _jumping = false;
                _jumpHeight = 0;
                _isOnGround = true;
                Stand(_direction);
                _oldKeyboardState = new KeyboardState();
                return true;
            }
            else
                return false;
        }
        internal void actualizeSpritePosition()
        {
            _sprite.setRelatvePos(
                new Rectangle(
                    (int)_position.X - _sprite.Position.Width / 2,
                    (int)_position.Y - _sprite.Position.Height,
                    _sprite.Position.Width,
                    _sprite.Position.Height),
                    _windowSize.Width, _windowSize.Height);
        }
        internal void actualizeSpriteGraphicalBounds()
        {
            Rectangle r = _graphicalBounds.get(_action);
            _sprite.SetPictureBounds(r.Y, r.Width, r.X, true);
            _sprite.Speed = r.Height;
            if (_weapon != null)
                _weapon.actualizeSpriteGraphicalBounds(r);
        }
    }
}
