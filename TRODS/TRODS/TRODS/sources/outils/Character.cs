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
        /// <summary>
        /// Liste des attaques du personnage
        /// </summary>
        public enum Attacks
        {
            SimpleHit
        }
        /// <summary>
        /// Liste des differentes actions du personnage
        /// </summary>
        public enum Actions
        {
            StandRight, StandLeft, WalkLeft, WalkRight, JumpRight, JumpLeft, Attack, Fall, Paralized
        }
        /// <summary>
        /// Permet de definir les bornes de parcours des images dans un sprite pour des mouvements donnes.
        /// </summary>
        public struct GraphicalBounds
        {
            public GraphicalBounds(Dictionary<Actions, Vector3> boundList)
            {
                this.BoundList = boundList;
            }
            Dictionary<Actions, Vector3> BoundList;
            /// <summary>
            /// Definit une nouvelle borne.
            /// </summary>
            /// <param name="a">Action a definir</param>
            /// <param name="x">Image de debut de l'animation</param>
            /// <param name="y">Premiere image pour la repetition de l'animation</param>
            /// <param name="z">Image de fin de l'animation</param>
            public void set(Actions a, int x, int y, int z)
            {
                if (BoundList.ContainsKey(a))
                    BoundList.Remove(a);
                BoundList.Add(a, new Vector3(x, y, z));
            }
            /// <summary>
            /// Definit une nouvelle borne.
            /// </summary>
            /// <param name="a">Action a definir</param>
            /// <param name="v">Vecteur :
            /// X : Image de debut de l'animation
            /// Y : Premiere image pour la repetition de l'animation
            /// Z : Image de fin de l'animation</param>
            public void set(Actions a, Vector3 v)
            {
                if (BoundList.ContainsKey(a))
                    BoundList.Remove(a);
                BoundList.Add(a, v);
            }
            /// <summary>
            /// Petmet d'obtenir les bornes d'une action definie
            /// </summary>
            /// <param name="a">Action</param>
            /// <returns>Resultat de type Vector3</returns>
            public Vector3 get(Actions a)
            {
                Vector3 r = new Vector3();
                BoundList.TryGetValue(a, out r);
                return r;
            }
        }

        internal Rectangle _windowSize;
        internal GraphicalBounds _graphicalBounds;
        internal Actions _action;
        internal AnimatedSprite _sprite;
        /// <summary>
        /// En bas au milieu du sprite
        /// </summary>
        internal Vector2 _position;
        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }
        internal Vector2 _speed;
        public Vector2 Speed
        {
            get { return _speed; }
            internal set { _speed = value; }
        }
        internal Physics _physics;
        public bool _canMove { get; internal set; }
        public bool _isOnGround { get; internal set; }
        public bool _jumping { get; internal set; }
        public int _jumpHeight { get; internal set; }
        public int _maxJumpHeight;
        internal bool _direction;
        internal int _timer;
        internal List<Attac> _attacks;
        public List<Attac> Attacks1
        {
            get { return _attacks; }
            set { _attacks = value; }
        }


        public int _lifePoints { get; private set; }

        public Character(Rectangle winSize, Vector2 position, int width, int height, string assetName, int textureColumns, int textureLines, Vector2 speed)
        {
            _windowSize = winSize;
            _position = position;
            _graphicalBounds = new GraphicalBounds(new Dictionary<Actions, Vector3>());
            _attacks = new List<Attac>();
            _sprite = new AnimatedSprite(new Rectangle((int)position.X - width / 2, (int)position.Y - height, width, height), winSize, assetName, textureColumns, textureLines, 30, 1, -1, -1, true);
            _canMove = true;
            _jumping = false;
            _jumpHeight = 0;
            _maxJumpHeight = 0;
            _direction = true; // = right
            _timer = 0;
            _physics = new Physics();
            _speed = speed;
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
                _sprite.Draw(spriteBatch, Color.White, _sprite.Position.X, _sprite.Position.Y - _jumpHeight);
        }
        public override void Update(float elapsedTime)
        {
            _timer -= (int)elapsedTime;
            _sprite.Update(elapsedTime);
            switch (_action)
            {
                case Actions.WalkRight:
                    _position.X += _speed.X * elapsedTime;
                    break;
                case Actions.WalkLeft:
                    _position.X -= _speed.X * elapsedTime;
                    break;
            }
            if (_jumping)
                _jumpHeight += _physics.Update(elapsedTime);
            testOnGround();
            // Fonctionnalite pas finie.
            /*if (_timer < 0)
            {
                if (_direction) // right
                    _action = Actions.StandRight;
                else
                    _action = Actions.StandLeft;
            }*/
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
        public override void HandleInput(KeyboardState newKeyboardState, MouseState newMouseState, Game1 parent)
        {
            // Pour les personnages de type joueur
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

        /// <summary>
        /// Execute un saut.
        /// </summary>
        /// <returns>Booleen indiquand si le saut a bien ete execute.</returns>
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
        /// <summary>
        /// Met a jour la direction du personnage et actualise l'animation.
        /// </summary>
        /// <param name="right"></param>
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
                // C'est pas le personnage qui bouge mais la map
                /*Vector3 bounds = _graphicalBounds.get(_action);
                _sprite.SetPictureBounds((int)bounds.Y, (int)bounds.Z, (int)bounds.X);
                switch (_action)
                {
                    case Actions.WalkRight:
                    case Actions.WalkLeft:
                        _sprite.Speed = 24;
                        break;
                    case Actions.StandRight:
                    case Actions.StandLeft:
                        _sprite.Speed = 3;
                        break;
                    case Actions.Jump:
                        Jump();
                        break;
                }*/
            }
        }
        /// <summary>
        /// Paralise le personnage empechant tout mouvement.
        /// </summary>
        /// <param name="time">Temps de bloquage.</param>
        public void Paralize(int time)
        {
            _canMove = false;
            _timer = time;
            _action = Actions.Paralized;
        }
        /// <summary>
        /// Libere les mouvements du personnage.
        /// </summary>
        public void Free()
        {
            _canMove = true;
        }
        /// <summary>
        /// Execute une attaque.
        /// </summary>
        /// <param name="attack">Element de l'enumeration Attacks.</param>
        /// <returns>Booleen indiquant si l'attaque a bien ete executee.</returns>
        public bool Attack(Attacks attack)
        {
            return true;
        }
        /// <summary>
        /// Execute un deplacement rapide.
        /// </summary>
        /// <returns>Booleen indiquant si le deplacement rapide a bien ete execute.</returns>
        public bool DoubleDash()
        {
            return false;
        }

        internal bool testOnGround()
        {
            if (_jumpHeight < 0)
            {
                _jumping = false;
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
            _sprite.SetPictureBounds(
                (int)_graphicalBounds.get(_action).X,
                (int)_graphicalBounds.get(_action).Y,
                (int)_graphicalBounds.get(_action).Z,
                true);
        }
    }
}
