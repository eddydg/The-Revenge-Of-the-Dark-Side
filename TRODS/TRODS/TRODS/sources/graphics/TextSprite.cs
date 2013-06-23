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
    class TextSprite : AnimatedSprite
    {
        protected SpriteFont _spriteFont;
        public SpriteFont SpriteFont
        {
            get { return _spriteFont; }
            protected set { _spriteFont = value; }
        }

        protected string _text;
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

        protected int _showindSpeed;
        protected bool _showing;
        public bool Showing
        {
            get { return _showing; }
            private set { _showing = value; }
        }
        protected int _timer;
        protected int _showedCharacters;

        public Color Color { get; set; }

        public TextSprite(string assetName, Rectangle windowSize, Rectangle position, Color color, string text = "")
            : base(position, windowSize, assetName)
        {
            this.Color = color;
            this.Text = text;
            _showedCharacters = Text.Length;
            SetShowedCharacters(-1);
            _showindSpeed = 0;
            _showing = false;
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
            if (_showedCharacters <= Text.Length)
            {
                float num1 = this.min((float)this.Position.Width / this._spriteFont.MeasureString(this.Text).X, (float)this.Position.Height / this._spriteFont.MeasureString(this.Text).Y);
                Vector2 position2 = new Vector2((float)Position.Location.X, (float)Position.Location.Y);
                spriteBatch.DrawString(_spriteFont, Text.Substring(0, _showedCharacters), position2, Color, (float)0, Vector2.Zero, num1, (SpriteEffects)0, (float)0);
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Rectangle position)
        {
            if (_showedCharacters <= Text.Length)
            {
                float scale = this.min((float)position.Width / this._spriteFont.MeasureString(this.Text).X, (float)position.Height / this._spriteFont.MeasureString(this.Text).Y);
                spriteBatch.DrawString(this._spriteFont, this.Text.Substring(0, _showedCharacters), new Vector2((float)position.Location.X, (float)position.Location.Y), this.Color, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Color color, int X, int Y)
        {
            if (_showedCharacters <= Text.Length)
            {
                float num1 = this.min((float)this.Position.Width / this._spriteFont.MeasureString(this.Text).X, (float)this.Position.Height / this._spriteFont.MeasureString(this.Text).Y);
                spriteBatch.DrawString(_spriteFont, Text.Substring(0, _showedCharacters), new Vector2((float)X, (float)Y), color, 0f, Vector2.Zero, num1, (SpriteEffects)0, 0f);
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Color color)
        {
            if (_showedCharacters <= Text.Length)
            {
                float num1 = this.min((float)this.Position.Width / this._spriteFont.MeasureString(this.Text).X, (float)this.Position.Height / this._spriteFont.MeasureString(this.Text).Y);
                spriteBatch.DrawString(_spriteFont, Text.Substring(0, _showedCharacters), new Vector2(Position.Location.X, Position.Location.Y), color, 0f, Vector2.Zero, num1, (SpriteEffects)0, 0f);
            }
        }

        public override void Draw(SpriteBatch spriteBatch, byte alpha)
        {
            if (_showedCharacters <= Text.Length)
            {
                float num1 = this.min((float)this.Position.Width / this._spriteFont.MeasureString(this.Text).X, (float)this.Position.Height / this._spriteFont.MeasureString(this.Text).Y);
                Color c = Color.FromNonPremultiplied(Color.R, Color.G, Color.B, alpha);
                spriteBatch.DrawString(_spriteFont, Text.Substring(0, _showedCharacters), new Vector2(Position.Location.X, Position.Location.Y), c, 0f, Vector2.Zero, num1, (SpriteEffects)0, 0f);
            }
        }

        public override void Update(float elapsedTime)
        {
            base.Update(elapsedTime);
            if (_showing)
            {
                _timer += (int)elapsedTime;
                if (_timer >= _showindSpeed)
                {
                    if (!SetShowedCharacters(_showedCharacters + 1))
                        _showing = false;
                    _timer = 0;
                }
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

        public virtual bool SetShowedCharacters(int count)
        {
            if (count < 0 || count > Text.Length)
            {
                _showedCharacters = Text.Length;
                return false;
            }
            else
                _showedCharacters = count;
            return true;
        }

        public virtual void StartShowing(int showedCharacters = 0, int showingSpeed=-1)
        {
            SetShowedCharacters(showedCharacters);
            _showing = true;
            if (showingSpeed > 0)
                _showindSpeed = showingSpeed;
            _timer = 0;
        }

        public virtual bool FullyShowed()
        {
            return _showedCharacters >= _text.Length;
        }

        public virtual void StopShowing()
        {
            _showing = false;
        }
    }
}
