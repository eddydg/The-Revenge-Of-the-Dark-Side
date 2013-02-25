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
    class Weapon
    {
        private AnimatedSprite _sprite;

        public Weapon(string assetName, int lignes, int colones)
        {
            _sprite = new AnimatedSprite(new Rectangle(), new Rectangle(), assetName);
        }

        public void LoadContent(ContentManager content)
        {
            _sprite.LoadContent(content);
        }
        public void Draw(SpriteBatch s, Rectangle character)
        {
            _sprite.Draw(s, new Vector2(
                character.X + character.Width / 2 - _sprite.Texture.Width / _sprite.Colonnes / 2,
                character.Y + character.Height / 2 - _sprite.Texture.Height / _sprite.Lignes / 2));
        }
        public void Update(float elapsedTime)
        {
            _sprite.Update(elapsedTime);
        }
        public void actualizeSpriteGraphicalBounds(Rectangle rect)
        {
            _sprite.SetPictureBounds(rect.Y, rect.Width, rect.X, true);
            _sprite.Speed = rect.Height;
        }
    }
}
