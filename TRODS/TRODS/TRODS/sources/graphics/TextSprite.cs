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
    class TextSprite : Sprite
    {
        private SpriteFont _spriteFont;
        public String Text { get; set; }
        public Color Color { get; set; }

        public TextSprite(string assetName, Rectangle windowSize, Rectangle position, string text = "", Color color = new Color())
            : base(position, windowSize, assetName)
        {
            Color = color;
            Text = text;
        }

        public override void LoadContent(ContentManager content)
        {
            _spriteFont = content.Load<SpriteFont>(AssetName);
        }
        public void LoadContent(SpriteFont spritefont)
        {
            _spriteFont = spritefont;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            float scale = min((float)Position.Width / _spriteFont.MeasureString(Text).X, (float)Position.Height / _spriteFont.MeasureString(Text).Y);
            spriteBatch.DrawString(_spriteFont, Text, new Vector2(Position.Location.X, Position.Location.Y),
                                    Color, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
        }
        public override void Draw(SpriteBatch spriteBatch, Rectangle position)
        {
            float scale = min((float)position.Width / _spriteFont.MeasureString(Text).X, (float)position.Height / _spriteFont.MeasureString(Text).Y);
            spriteBatch.DrawString(_spriteFont, Text, new Vector2(position.Location.X, position.Location.Y),
                                    Color, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
        }

        private float min(float a, float b)
        {
            return a > b ? b : a;
        }
        public Vector2 GetOriginalTextSize()
        {
            return _spriteFont.MeasureString(Text);
        }
    }
}
