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
    class MultipleTextSprite : TextSprite
    {
        public enum Layout
        {
            LeftAlign, MiddleAlign
        }

        private List<TextSprite> _elements;
        private Rectangle _windowSize;

        public MultipleTextSprite(string assetName, Rectangle windowSize, Rectangle Position, Color color) :
            base(assetName, windowSize, Position, color, "")
        {
            _elements = new List<TextSprite>();
            _windowSize = windowSize;
        }

        public override void LoadContent(ContentManager content)
        {
            _spriteFont = content.Load<SpriteFont>(AssetName);
            foreach (TextSprite ts in _elements)
                ts.LoadContent(_spriteFont);
            SetLayout();
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (TextSprite ts in _elements)
                ts.Draw(spriteBatch);
        }
        public override void Draw(SpriteBatch spriteBatch, Rectangle position)
        {
            foreach (TextSprite ts in _elements)
                ts.Draw(spriteBatch, position);
        }
        public override void Update(float elapsedTime)
        {
            _position = new Rectangle((int)(_vitesse * Direction.X * elapsedTime) + _position.X,
                                      (int)(_vitesse * Direction.Y * elapsedTime) + _position.Y,
                                      _position.Width, _position.Height);
            foreach (TextSprite ts in _elements)
                ts.Update(elapsedTime);
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

        public void Add(string text, int line = -1)
        {
            TextSprite n = new TextSprite(AssetName, _windowSize, new Rectangle(), Color, text);
            n.LoadContent(SpriteFont);
            n.Direction = Direction;
            n.Vitesse = Vitesse;
            if (line < 0)
                _elements.Add(n);
            else
                _elements.Insert(line, n);
            SetLayout();
            SetShowedCharacters(-1);
        }
        public void Add(List<string> lines)
        {
            foreach (string s in lines)
                Add(s);
        }
        public void SetLayout(Layout layout = Layout.LeftAlign)
        {
            int h = Position.Height / _elements.Count;
            if (layout == Layout.MiddleAlign)
            {
            }
            else
            {
                for (int i = 0; i < _elements.Count; i++)
                {
                    _elements[i].setRelatvePos(new Rectangle(Position.X, Position.Y + i * h, Position.Width, h), _windowSize.Width, _windowSize.Height);
                }
            }
        }
        public void windowResized(Rectangle rect)
        {
            foreach (TextSprite ts in _elements)
                ts.Vitesse = Vitesse;
            SetLayout();
        }
        public void setRelatvePos(Rectangle a_position)
        {
            Position = a_position;
            SetLayout();
        }

        public override bool SetShowedCharacters(int count)
        {
            _showedCharacters = count;
            if (_elements != null)
            {
                foreach (TextSprite t in _elements)
                    t.SetShowedCharacters(0);
                foreach (TextSprite t in _elements)
                {
                    if (t.SetShowedCharacters(count))
                        return true;
                    count -= t.Text.Length;
                }
            }
            return false;
        }
        public override bool FullyShowed()
        {
            if (_elements != null)
            {
                foreach (TextSprite t in _elements)
                {
                    if (!t.FullyShowed())
                        return false;
                }
                return true;
            }
            return false;
        }
    }
}
