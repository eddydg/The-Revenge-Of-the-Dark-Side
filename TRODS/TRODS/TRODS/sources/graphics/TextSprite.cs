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

        public string Text { get; set; }

        public Color Color { get; set; }

        public TextSprite(string assetName, Rectangle windowSize, Rectangle position, Color color, string text = "")
            : base(position, windowSize, assetName)
        {
            this.Color = color;
            this.Text = text;
        }

        public override void LoadContent(ContentManager content)
        {
            this._spriteFont = content.Load<SpriteFont>(this.AssetName);
        }

        public void LoadContent(SpriteFont spritefont)
        {
            this._spriteFont = spritefont;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            float num1 = this.min((float)this.Position.Width / this._spriteFont.MeasureString(this.Text).X, (float)this.Position.Height / this._spriteFont.MeasureString(this.Text).Y);
            SpriteBatch spriteBatch1 = spriteBatch;
            SpriteFont spriteFont = this._spriteFont;
            string text = this.Text;
            Rectangle position1 = this.Position;
            double num2 = (double)position1.Location.X;
            position1 = this.Position;
            double num3 = (double)position1.Location.Y;
            Vector2 position2 = new Vector2((float)num2, (float)num3);
            Color color = this.Color;
            double num4 = 0.0;
            Vector2 zero = Vector2.Zero;
            double num5 = (double)num1;
            int num6 = 0;
            double num7 = 0.0;
            spriteBatch1.DrawString(spriteFont, text, position2, color, (float)num4, zero, (float)num5, (SpriteEffects)num6, (float)num7);
        }

        public override void Draw(SpriteBatch spriteBatch, Rectangle position)
        {
            float scale = this.min((float)position.Width / this._spriteFont.MeasureString(this.Text).X, (float)position.Height / this._spriteFont.MeasureString(this.Text).Y);
            spriteBatch.DrawString(this._spriteFont, this.Text, new Vector2((float)position.Location.X, (float)position.Location.Y), this.Color, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
        }

        private float min(float a, float b)
        {
            return (double)a > (double)b ? b : a;
        }

        public Vector2 GetOriginalTextSize()
        {
            return this._spriteFont.MeasureString(this.Text);
        }
    }
}
