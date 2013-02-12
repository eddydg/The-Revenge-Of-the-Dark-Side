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
            StandRight, StandLeft, WalkLeft, WalkRight, Jump, Attack
        }
        /// <summary>
        /// Permet de definir les bornes de parcours des images dans un sprite pour des mouvements donnes.
        /// </summary>
        public struct GraphicalBounds
        {
            public GraphicalBounds(Dictionary<Actions, Vector3> BoundList)
            {
                this.BoundList = BoundList;
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

        private Rectangle _windowSize;
        private GraphicalBounds _graphicalBounds;
        private Actions _action;
        private AnimatedSprite _sprite;
        /// <summary>
        /// En bas au milieu du sprite
        /// </summary>
        private Vector2 _position;
        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }
        private Physics _physics;
        public bool _canMove { get; private set; }
        public bool _isOnGround { get; private set; }
        public bool _jumping { get; private set; }
        public int _jumpHeight { get; private set; }
        private bool _direction;
        private int _timer;
        private List<Attac> _attacks;
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
            _sprite = new AnimatedSprite(new Rectangle((int)position.X - width / 2, (int)position.Y - height, width, height), winSize, assetName, textureColumns, textureLines, 30, 1, -1, -1, true);
            _canMove = true;
            _jumping = false;
            _jumpHeight = 0;
            _direction = true; // = right
            _timer = 0;
            _physics = new Physics();
        }

        public override void LoadContent(ContentManager content)
        {
            _sprite.LoadContent(content);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            _sprite.Draw(spriteBatch);
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
                if (_direction) // right
                    _action = Actions.StandRight;
                else
                    _action = Actions.StandLeft;
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
        public bool Jump()
        {
            return false;
        }
        /// <summary>
        /// Met a jour la direction du personnage et actualise l'animation.
        /// </summary>
        /// <param name="right"></param>
        public void Move(bool right)
        {
        }
        /// <summary>
        /// Paralise le personnage empechant tout mouvement.
        /// </summary>
        /// <param name="time">Temps de bloquage.</param>
        public void Paralize(int time)
        {
        }
        /// <summary>
        /// Libere les mouvements du personnage.
        /// </summary>
        public void Free()
        {
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

        /// <summary>
        /// Gestion de l'atterissage du personnage
        /// </summary>
        /// <returns>Si le personnage</returns>
        private bool testOnGround()
        {
            if (_jumpHeight < 0)
            {
                _jumping = false;
                if (_direction) // right
                    _action = Actions.StandRight;
                else
                    _action = Actions.StandLeft;
                Vector3 b = _graphicalBounds.get(_action);
                _sprite.SetPictureBounds((int)b.X, (int)b.Y, (int)b.Z, true);
                _jumpHeight = 0;
                return true;
            }
            else
                return false;
        }
        private void actualizeSpritePosition()
        {
            _sprite.setRelatvePos(
                new Rectangle(
                    (int)_position.X - _sprite.Position.Width / 2,
                    (int)_position.Y - _sprite.Position.Height,
                    _sprite.Position.Width,
                    _sprite.Position.Height),
                    _windowSize.Width, _windowSize.Height);
        }
        private void actualizeSpriteGraphicalBounds()
        {
            _sprite.SetPictureBounds(
                (int)_graphicalBounds.get(_action).X,
                (int)_graphicalBounds.get(_action).Y,
                (int)_graphicalBounds.get(_action).Z,
                true);
        }
    }
}
