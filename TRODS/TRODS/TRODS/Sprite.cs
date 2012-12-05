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
    class Sprite
    {
        private Texture2D _texture;
        public Texture2D Texture
        {
            get { return _texture; }
            set { _texture = value; }
        }

        private Rectangle _position;
        public Rectangle Position
        {
            get { return _position; }
            set { _position = value; }
        }


        private Vector2 _direction;
        public Vector2 Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        private float _vitesse;
        public float Vitesse
        {
            get { return _vitesse; }
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
        /// <param name="aposition">Dimenson du Sprite dans la fenetre</param>
        /// <param name="windowWidth">Largeur de la fenetre</param>
        /// <param name="windowHeight">Hauteur de la fenetre</param>
        public Sprite(Rectangle aposition, int windowWidth = 0, int windowHeight = 0)
        {
            _position = aposition;
            _direction = new Vector2();
            _vitesse = 0;
            if (windowWidth > 0 && windowHeight > 0)
            {
                _relativePosX = (float)aposition.X / (float)windowWidth;
                _relativePosY = (float)aposition.Y / (float)windowHeight;
                _relativeWidth = (float)aposition.Width / (float)windowWidth;
                _relativeHeight = (float)aposition.Height / (float)windowHeight;
                _isRelativePos = true;
                _relativeSpeed = 0;
            }
            else
                _isRelativePos = false;
        }

        /// <summary>
        /// Chargement de la texture
        /// </summary>
        /// <param name="content">Gertionnaire de contenu de XNA</param>
        /// <param name="assetName">Nom de la texture</param>
        public virtual void LoadContent(ContentManager content, string assetName)
        {
            _texture = content.Load<Texture2D>(assetName);
        }

        /// <summary>
        /// Met a jour la position du sprite
        /// </summary>
        /// <param name="elapsedTime">Temps ecoule depuis le dernier appel de la fonction</param>
        public void Update(float elapsedTime)
        {
            _position.X += (int)(_vitesse * _direction.X * elapsedTime);
            _position.Y += (int)(_vitesse * _direction.Y * elapsedTime);
        }

        /// <summary>
        /// Dessine le Sprite avec ses parametres par defaut
        /// </summary>
        /// <param name="spriteBatch">Gestionnaire de dessin XNA</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, Color.White);
        }
        /// <summary>
        /// Dessine le Sprite avec des parametres specfiques
        /// </summary>
        /// <param name="spriteBatch">Gestionnaire de dessin XNA</param>
        /// <param name="color">Coloration de la texture</param>
        /// <param name="X">Absisse</param>
        /// <param name="Y">Ordonee</param>
        public void DrawWith(SpriteBatch spriteBatch, Color color, int X, int Y)
        {
            spriteBatch.Draw(_texture,
                            new Rectangle(X, Y, _position.Width, _position.Height),
                            color);
        }
        /// <summary>
        /// Dessine le Sprite avec des parametres specfiques
        /// </summary>
        /// <param name="spriteBatch">Gestionnaire de dessin XNA</param>
        /// <param name="color">Coloration de la texture</param>
        /// <param name="rotate">Angle de rotation en radians</param>
        /// <param name="origin">Origine de la rotation</param>
        /// <param name="alpha">Transparente alpha (0:transparent - 255:opaque)</param>
         public void DrawWith(SpriteBatch spriteBatch, Color color,float rotate=0,Vector2 origin=new Vector2(),int alpha=255)
         {
             spriteBatch.Draw(_texture, _position,_position, color, rotate, origin, new SpriteEffects(), alpha);
         }


        /// <summary>
        /// Active l'adaptation automatique de la texture
        /// au redomensionnement de la fenetre
        /// </summary>
        /// <param name="aposition">Position actuelle du Sprite</param>
        /// <param name="windowWidth">Largeur de la fenetre</param>
        /// <param name="windowHeight">Hauteur de la fenetre</param>
        public void setRelatvePos(Rectangle aposition, int windowWidth, int windowHeight)
        {
            _position = aposition;
            if (windowWidth > 0 && windowHeight > 0)
            {
                _relativePosX = (float)aposition.X / (float)windowWidth;
                _relativePosY = (float)aposition.Y / (float)windowHeight;
                _relativeWidth = (float)aposition.Width / (float)windowWidth;
                _relativeHeight = (float)aposition.Height / (float)windowHeight;
                _isRelativePos = true;
            }
            else
                _isRelativePos = false;
        }
        /// <summary>
        /// Adapte la taille et la position de la texture
        /// et la vitesse a celle de la fenetre
        /// </summary>
        /// <param name="rect"></param>
        public void windowResized(Rectangle rect)
        {
            if (_isRelativePos)
            {
                _position = new Rectangle(
                    (int)(_relativePosX * rect.Width),
                    (int)(_relativePosY * rect.Height),
                    (int)(_relativeWidth * rect.Width),
                    (int)(_relativeHeight * rect.Height));
                _vitesse = _relativeSpeed * _position.Width;
            }
        }
        /// <summary>
        /// Libere les textures
        /// </summary>
        public void Dispose()
        {
            _texture.Dispose();
        }
    }
}