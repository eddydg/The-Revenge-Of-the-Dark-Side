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
    public enum Attacks
    {
        Simplehit, SimpleHit2, SimpleHit3
    }

    class Character : AbstractScene
    {
        private Rectangle _windowSize;
        private AnimatedSprite _sprite;
        private Vector2 _position;
        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }
        public bool _canMove { get; private set; }
        public bool _jumping { get; private set; }
        public int _jumpHeight { get; private set; }
        public int _jumpMaxHeight { get; private set; }
        private bool _direction;
        private int _timer;

        public int _lifePoints { get; private set; }

        public Character(Rectangle winSize, Vector2 position, int width, int height, string assetName, int textureColumns, int textureLines, Vector2 speed, int jumpHeight)
        {
            _windowSize = winSize;
            _position = position;
            _sprite = new AnimatedSprite(new Rectangle((int)position.X + width / 2, (int)position.Y - height, width, height), winSize, assetName, textureColumns, textureLines);
            _canMove = true;
            _jumping = false;
            _jumpHeight = 0;
            _jumpMaxHeight = jumpHeight;
            _direction = true; // = right
            _timer = 0;
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
            _sprite.Update(elapsedTime);
        }
        public override void HandleInput(KeyboardState newKeyboardState, MouseState newMouseState, Game1 parent)
        {
        }
        public override void WindowResized(Rectangle rect)
        {
            _sprite.windowResized(rect);

            float x = (float)rect.Width / (float)_windowSize.Width, y = (float)rect.Height / (float)_windowSize.Height;

            _position.X *= x;
            _position.Y *= y;

            _jumpHeight = (int)((float)_jumpHeight * y);
            _jumpMaxHeight = (int)((float)_jumpMaxHeight * y);

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
            return false;
        }
        /// <summary>
        /// Execute un deplacement rapide.
        /// </summary>
        /// <returns>Booleen indiquant si le deplacement rapide a bien ete execute.</returns>
        public bool DoubleDash()
        {
            return false;
        }
    }
}
