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
        public enum Attacks
        {
            SimpleHit
        }
        public enum Actions
        {
            StandRight, StandLeft, WalkLeft, WalkRight, JumpRight, JumpLeft, Attack, Fall, Paralized
        }
        public struct GraphicalBounds
        {
            public GraphicalBounds(Dictionary<Actions, Rectangle> boundList)
            {
                this.BoundList = boundList;
            }
            Dictionary<Actions, Rectangle> BoundList;

            public void set(Actions ac, int first, int firstRepeat, int last, int speed = 30)
            {
                if (BoundList.ContainsKey(ac))
                    BoundList.Remove(ac);
                BoundList.Add(ac, new Rectangle(first, firstRepeat, last, speed));
            }
            /// <summary>
            /// Definit une nouvelle borne.
            /// </summary>
            /// <param name="a">Action a definir</param>
            /// <param name="v">Vecteur :
            /// X : Image de debut de l'animation
            /// Y : Premiere image pour la repetition de l'animation
            /// Z : Image de fin de l'animation</param>
            public void set(Actions a, Rectangle r)
            {
                if (BoundList.ContainsKey(a))
                    BoundList.Remove(a);
                BoundList.Add(a, r);
            }
            /// <summary>
            /// Petmet d'obtenir les bornes d'une action definie
            /// </summary>
            /// <param name="a">Action</param>
            /// <returns>Resultat de type Vector3</returns>
            public Rectangle get(Actions a)
            {
                Rectangle r = new Rectangle();
                BoundList.TryGetValue(a, out r);
                return r;
            }
        }

        internal Rectangle _windowSize;
        internal GraphicalBounds _graphicalBounds;
        internal Actions _action;
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
        public List<Attac> AttackLst
        {
            get { return _attacks; }
            set { _attacks = value; }
        }

        public Character(Rectangle winSize, Vector2 position, int width, int height,
            string assetName, int textureColumns, int textureLines)
        {
            _windowSize = winSize;
            _position = position;
            _graphicalBounds = new GraphicalBounds(new Dictionary<Actions, Rectangle>());
            _attacks = new List<Attac>();
            _sprite = new AnimatedSprite(new Rectangle((int)position.X - width / 2, (int)position.Y - height, width, height), winSize, assetName, textureColumns, textureLines, 30, 1, -1, -1, true);
            _canMove = true;
            _jumping = false;
            _jumpHeight = 0;
            _direction = true; // = right
            _timer = 0;
            _physics = new Physics();
            _action = Actions.StandRight;
        }

        public override void LoadContent(ContentManager content)
        {
            _sprite.LoadContent(content);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!_jumping)
                _sprite.Draw(spriteBatch);
            else
                _sprite.Draw(spriteBatch, new Vector2(_sprite.Position.X, _sprite.Position.Y - _jumpHeight));
        }
        public override void Update(float elapsedTime)
        {
            _timer -= (int)elapsedTime;
            _sprite.Update(elapsedTime);
            if (_jumping)
                _jumpHeight += _physics.Update(elapsedTime);
            testOnGround();
            if (_timer < 0)
            {
                _timer = 0;
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

            float x = (float)rect.Width / (float)_windowSize.Width, y = (float)rect.Height / (float)_windowSize.Height;

            _position.X *= x;
            _position.Y *= y;

            _jumpHeight = (int)((float)_jumpHeight * y);

            Rectangle r = new Rectangle();
            foreach (Attac a in _attacks)
            {
                r.X = (int)((float)a.Portee.X * x);
                r.Y = (int)((float)a.Portee.Y * y);
                r.Width = (int)((float)a.Portee.Width * x);
                r.Height = (int)((float)a.Portee.Height * y);
            }

            _windowSize = rect;
        }

        public void Stand(bool right)
        {
            _direction = right;
            if (right)
                _action = Actions.StandRight;
            else
                _action = Actions.StandLeft;
            actualizeSpriteGraphicalBounds();
        }
        public void Jump()
        {
            if (_canMove && !_jumping)
            {
                if (_direction)
                    _action = Actions.JumpRight;
                else
                    _action = Actions.JumpLeft;
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
                    _action = Actions.WalkRight;
                else
                    _action = Actions.WalkLeft;
                actualizeSpriteGraphicalBounds();
            }
        }
        public void Paralize(int time)
        {
            _canMove = false;
            _timer = time;
            _action = Actions.Paralized;
        }
        public void Free()
        {
            _canMove = true;
        }
        public void Attack(Attacks attack)
        {
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
                if (_direction) // right
                    _action = Actions.StandRight;
                else
                    _action = Actions.StandLeft;
                actualizeSpriteGraphicalBounds();
                _isOnGround = true;
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
        }
    }
}
