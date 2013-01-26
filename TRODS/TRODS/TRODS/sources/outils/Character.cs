using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;


namespace TRODS
{
    class Character : AbstractScene
    {
        private Rectangle _positionSprite;
        public Rectangle PositionSprite
        {
            get { return _positionSprite; }
            set { _positionSprite = value; }
        }
        private Vector2 _position;
        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }
        private Texture2D _texture;
        public Texture2D Texture
        {
            get { return _texture; }
            set { _texture = value; }
        }
        private string _assetName;
        private Vector2 _speed;
        public Vector2 Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }
        public bool _canMove { get; set; }
        private bool _jumping;
        private int _jumpHeight;
    }
}
