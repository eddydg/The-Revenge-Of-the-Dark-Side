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
    class Sprite : AbstractScene
    {
        public string AssetName { get; set; }
        public Texture2D Texture { get; set; }
        private Rectangle _position;
        public Rectangle Position
        {
            get { return _position; }
            set { _position = value; }
        }
        public Vector2 Direction { get; set; }
        private float _vitesse;
        public float Vitesse
        {
            get
            {
                return _vitesse;
            }
            set
            {
                _vitesse = value;
                _relativeSpeed = _vitesse / _position.Width;
            }
        }

        private bool _isRelativePos;
        private float _relativePosX;
        private float _relativePosY;
        private float _relativeWidth;
        private float _relativeHeight;
        private float _relativeSpeed;

        /// <summary>
        /// Si les 2 derniers parametres sont donnes,
        /// le Sprite d'adapte automatiquement aux
        /// redimensionnements de la fenetre par
        /// l'appel de windowResized()
        /// </summary>
        /// <param name="a_position">Dimenson du Sprite dans la fenetre</param>
        /// <param name="windowWidth">Largeur de la fenetre</param>
        /// <param name="windowHeight">Hauteur de la fenetre</param>
        public Sprite(Rectangle a_position, Rectangle windowSize = new Rectangle(), string assetName = "")
        {
            _position = a_position;
            Direction = new Vector2();
            _vitesse = 0;
            AssetName = assetName;
            if (windowSize.Width > 0 && windowSize.Height > 0)
            {
                _relativePosX = (float)a_position.X / (float)windowSize.Width;
                _relativePosY = (float)a_position.Y / (float)windowSize.Height;
                _relativeWidth = (float)a_position.Width / (float)windowSize.Width;
                _relativeHeight = (float)a_position.Height / (float)windowSize.Height;
                _isRelativePos = true;
                _relativeSpeed = 0;
            }
            else
                _isRelativePos = false;
        }
        /// <summary>
        /// Constructeur de copie
        /// </summary>
        /// <param name="s">Sprite a copier</param>
        public Sprite(Sprite s)
        {
            AssetName = new string(s.AssetName.ToCharArray());
            _position = new Rectangle(s.Position.X, s.Position.Y, s.Position.Width, s.Position.Height);
            Direction = new Vector2(s.Direction.X, s.Direction.Y);
            _vitesse = s.Vitesse;
            _isRelativePos = s._isRelativePos;
            _relativePosX = s._relativePosY;
            _relativePosY = s._relativePosY;
            _relativeWidth = s._relativeWidth;
            _relativeHeight = s._relativeHeight;
            _relativeSpeed = s._relativeSpeed;
        }

        /// <summary>
        /// Chargement de la texture
        /// </summary>
        /// <param name="content">Gertionnaire de contenu de XNA</param>
        /// <param name="assetName">Nom de la texture</param>
        public virtual void LoadContent(ContentManager content, string assetName)
        {
            if (assetName == "")
                assetName = AssetName;
            else
                AssetName = assetName;
            Texture = content.Load<Texture2D>(assetName);
        }
        public override void LoadContent(ContentManager content)
        {
            Texture = content.Load<Texture2D>(AssetName);
        }
        /// <summary>
        /// Assigne la texture
        /// </summary>
        /// <param name="tex">Texture</param>
        public override void LoadContent(Texture2D tex)
        {
            Texture = tex;
        }
        public override void Update(float elapsedTime)
        {
            _position = new Rectangle((int)(_vitesse * Direction.X * elapsedTime) + _position.X,
                                      (int)(_vitesse * Direction.Y * elapsedTime) + _position.Y,
                                      _position.Width, _position.Height);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, _position, Color.White);
        }
        public virtual void Draw(SpriteBatch spriteBatch, Color color)
        {
            spriteBatch.Draw(Texture, _position, color);
        }
        public virtual void Draw(SpriteBatch spriteBatch, Rectangle position)
        {
            spriteBatch.Draw(Texture, position, Color.White);
        }
        public virtual void Draw(SpriteBatch spriteBatch, byte alpha)
        {
            spriteBatch.Draw(Texture, _position, Color.FromNonPremultiplied(255, 255, 255, alpha));
        }
        public virtual void Draw(SpriteBatch spriteBatch, Color color, int X, int Y)
        {
            spriteBatch.Draw(Texture, new Rectangle(X, Y, _position.Width, _position.Height), color);
        }
        public virtual void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(Texture, new Rectangle((int)position.X, (int)position.Y, _position.Width, _position.Height), Color.White);
        }
        public virtual void Draw(SpriteBatch spriteBatch, Rectangle destination, Rectangle source, Color color)
        {
            spriteBatch.Draw(Texture, destination, source, color);
        }

        /// <summary>
        /// Active l'adaptation automatique de la texture
        /// au redomensionnement de la fenetre
        /// </summary>
        /// <param name="a_position">_position actuelle du Sprite</param>
        /// <param name="windowWidth">Largeur de la fenetre</param>
        /// <param name="windowHeight">Hauteur de la fenetre</param>
        public virtual void setRelatvePos(Rectangle a_position, int windowWidth, int windowHeight)
        {
            _position = a_position;
            if (windowWidth > 0 && windowHeight > 0)
            {
                _relativePosX = (float)a_position.X / (float)windowWidth;
                _relativePosY = (float)a_position.Y / (float)windowHeight;
                _relativeWidth = (float)a_position.Width / (float)windowWidth;
                _relativeHeight = (float)a_position.Height / (float)windowHeight;
                _isRelativePos = true;
            }
            else
                _isRelativePos = false;
        }
        /// <summary>
        /// Adapte la taille et la _position de la texture
        /// et la _vitesse a celle de la fenetre
        /// </summary>
        /// <param name="rect"></param>
        public virtual void windowResized(Rectangle rect, Rectangle oldWindowSize = new Rectangle())
        {
            if (oldWindowSize == new Rectangle())
            {
                if (_isRelativePos)
                {
                    _position = new Rectangle(
                        (int)(_relativePosX * (float)rect.Width),
                        (int)(_relativePosY * (float)rect.Height),
                        (int)(_relativeWidth * (float)rect.Width),
                        (int)(_relativeHeight * (float)rect.Height));
                    _vitesse = _relativeSpeed * (float)_position.Width;
                }
            }
            else
            {
                setRelatvePos(_position, oldWindowSize.Width, oldWindowSize.Height);
                windowResized(rect);
            }
        }
        public virtual void Dispose()
        {
            Texture.Dispose();
        }
    }
}