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


        private string _text;
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
                SetShowedCharacters(-1);
            }
        }

        public Color Color { get; set; }

        private int _showedCharacters;

        public TextSprite(string assetName, Rectangle windowSize, Rectangle position, Color color, string text = "")
            : base(position, windowSize, assetName)
        {
            this.Color = color;
            this.Text = text;
            _showedCharacters = Text.Length;
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
            if (_showedCharacters != 0)
            {
                float num1 = this.min((float)this.Position.Width / this._spriteFont.MeasureString(this.Text).X, (float)this.Position.Height / this._spriteFont.MeasureString(this.Text).Y);
                Vector2 position2 = new Vector2((float)Position.Location.X, (float)Position.Location.Y);
                spriteBatch.DrawString(_spriteFont, Text.Substring(0, _showedCharacters), position2, Color, (float)0, Vector2.Zero, num1, (SpriteEffects)0, (float)0);
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Rectangle position)
        {
            if (_showedCharacters != 0)
            {
                float scale = this.min((float)position.Width / this._spriteFont.MeasureString(this.Text).X, (float)position.Height / this._spriteFont.MeasureString(this.Text).Y);
                spriteBatch.DrawString(this._spriteFont, this.Text.Substring(0, _showedCharacters), new Vector2((float)position.Location.X, (float)position.Location.Y), this.Color, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Color color, int X, int Y)
        {
            if (_showedCharacters != 0)
            {
                float num1 = this.min((float)this.Position.Width / this._spriteFont.MeasureString(this.Text).X, (float)this.Position.Height / this._spriteFont.MeasureString(this.Text).Y);
                spriteBatch.DrawString(_spriteFont, Text.Substring(0, _showedCharacters), new Vector2((float)X, (float)Y), color, 0f, Vector2.Zero, num1, (SpriteEffects)0, 0f);
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Color color)
        {
            if (_showedCharacters != 0)
            {
                float num1 = this.min((float)this.Position.Width / this._spriteFont.MeasureString(this.Text).X, (float)this.Position.Height / this._spriteFont.MeasureString(this.Text).Y);
                spriteBatch.DrawString(_spriteFont, Text.Substring(0, _showedCharacters), new Vector2(Position.Location.X, Position.Location.Y), color, 0f, Vector2.Zero, num1, (SpriteEffects)0, 0f);
            }
        }

        private float min(float a, float b)
        {
            return (double)a > (double)b ? b : a;
        }

        public Vector2 GetOriginalTextSize()
        {
            return this._spriteFont.MeasureString(this.Text);
        }

        public void SetShowedCharacters(int count)
        {
            if (count < 0 || count > Text.Length)
                _showedCharacters = Text.Length;
            else
                _showedCharacters = count;
        }
    }
}
