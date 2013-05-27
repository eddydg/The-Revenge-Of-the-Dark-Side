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

        public float Damage { get; set; }

        public Tip Tip { get; set; }

        public Rectangle Position
        {
            get
            {
                return this._sprite.Position;
            }
        }

        public Weapon(Rectangle winsize, string assetName, int lignes, int colones, int width, int height, float damage = 1f)
        {
            this._sprite = new AnimatedSprite(new Rectangle(0, 0, width, height), winsize, assetName, colones, lignes, 30, 1, -1, -1, false);
            this.Damage = damage;
        }

        public void LoadContent(ContentManager content)
        {
            ((AbstractScene)this._sprite).LoadContent(content);
            ((AbstractScene)this.Tip).LoadContent(content);
        }

        public void LoadContent(Texture2D content)
        {
            ((AbstractScene)this._sprite).LoadContent(content);
        }

        public void Draw(SpriteBatch s, Rectangle character)
        {
            Vector2 position = new Vector2((float)(character.X + character.Width / 2 - this._sprite.Position.Width / 2), (float)character.Y);
            this._sprite.Position = new Rectangle((int)position.X, (int)position.Y, this._sprite.Position.Width, this._sprite.Position.Height);
            this._sprite.Draw(s, position);
        }

        public void Update(float elapsedTime)
        {
            this._sprite.Update(elapsedTime);
        }

        public void actualizeSpriteGraphicalBounds(Rectangle rect)
        {
            this._sprite.SetPictureBounds(rect.Y, rect.Width, rect.X, true);
            this._sprite.Speed = rect.Height;
        }

        public void WindowResized(Rectangle rect)
        {
            this._sprite.windowResized(rect, new Rectangle());
        }
    }
}