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
        public Sprite(Rectangle a_position, Rectangle windowSize=new Rectangle(), string assetName="")
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
            AssetName = s.AssetName;
            Texture = s.Texture;
            _position = s.Position;
            Direction = s.Direction;
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
        public virtual void LoadContent(ContentManager content, string assetName="")
        {
            if (assetName == "")
                assetName = AssetName;
            Texture = content.Load<Texture2D>(assetName);
        }
        /// <summary>
        /// Assigne la texture
        /// </summary>
        /// <param name="tex">Texture</param>
        public virtual void LoadContent(Texture2D tex)
        {
            Texture = tex;
        }

        /// <summary>
        /// Met a jour la _position du sprite
        /// </summary>
        /// <param name="elapsedTime">Temps ecoule depuis le dernier appel de la fonction</param>
        public virtual void Update(float elapsedTime)
        {
            _position = new Rectangle((int)(_vitesse * Direction.X * elapsedTime)+_position.X,
                                      (int)(_vitesse * Direction.Y * elapsedTime)+_position.Y, 
                                      _position.Width, _position.Height);
        }

        /// <summary>
        /// Dessine le Sprite avec ses parametres par defaut
        /// </summary>
        /// <param name="spriteBatch">Gestionnaire de dessin XNA</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, _position, Color.White);
        }
        /// <summary>
        /// Dessine le Sprite avec des parametres specfiques
        /// </summary>
        /// <param name="spriteBatch">Gestionnaire de dessin XNA</param>
        /// <param name="color">Coloration de la texture</param>
        /// <param name="X">Absisse</param>
        /// <param name="Y">Ordonee</param>
        public virtual void Draw(SpriteBatch spriteBatch, Color color, int X, int Y)
        {
            spriteBatch.Draw(Texture,
                            new Rectangle(X, Y, _position.Width, _position.Height),
                            color);
        }
        /// <summary>
        /// Dessine le Sprite avec des parametres specfiques
        /// </summary>
        /// <param name="spriteBatch">Gestionnaire de dessin XNA</param>
        /// <param name="color">Coloration de la texture</param>
        /// <param name="position">Position</param>
        public virtual void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(Texture,
                            new Rectangle((int)position.X, (int)position.Y, _position.Width, _position.Height),
                            Color.White);
        }
        /// <summary>
        /// Dessine le Sprite avec des parametres specfiques
        /// </summary>
        /// <param name="spriteBatch">Gestionnaire de dessin XNA</param>
        /// <param name="color">Coloration de la texture</param>
        /// <param name="pos">Posion de la texture a l'ecran</param>
        public virtual void Draw(SpriteBatch spriteBatch, Color color, Rectangle pos)
        {
            spriteBatch.Draw(Texture,pos,color);
        }
        /// <summary>
        /// Dessine le Sprite avec des parametres specfiques
        /// </summary>
        /// <param name="spriteBatch">Gestionnaire de dessin XNA</param>
        /// <param name="color">Coloration de la texture</param>
        /// <param name="pos">Posion de la texture a l'ecran</param>
        public virtual void Draw(SpriteBatch spriteBatch, Color color)
        {
            spriteBatch.Draw(Texture, _position, color);
        }
        /// <summary>
        /// Dessine le Sprite avec des parametres specfiques
        /// </summary>
        /// <param name="spriteBatch">Gestionnaire de dessin XNA</param>
        /// <param name="color">Coloration de la texture</param>
        /// <param name="rotate">Angle de rotation en radians</param>
        /// <param name="origin">Origine de la rotation</param>
        /// <param name="alpha">Transparente alpha (0:transparent - 255:opaque)</param>
        public virtual void Draw(SpriteBatch spriteBatch, Color color, float rotate = 0, Vector2 origin = new Vector2(), int alpha = 255)
        {
            spriteBatch.Draw(Texture, _position, _position, color, rotate, origin, new SpriteEffects(), alpha);
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
        public virtual void windowResized(Rectangle rect)
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
        public virtual void Dispose()
        {
            Texture.Dispose();
        }
    }
}