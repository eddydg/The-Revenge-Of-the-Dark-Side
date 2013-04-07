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
    class MultipleTextSprite : Sprite
    {
        public enum Layout
        {
            LeftAlign, MiddleAlign
        }

        private SpriteFont _spriteFont;
        private List<TextSprite> _elements;

        public MultipleTextSprite(string assetName, Rectangle windowSize, Rectangle Position) :
            base(Position, windowSize, assetName)
        {
            _elements = new List<TextSprite>();
        }

        public override void LoadContent(ContentManager content)
        {
            _spriteFont = content.Load<SpriteFont>(AssetName);
            foreach (TextSprite ts in _elements)
                ts.LoadContent(_spriteFont);
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

        public void SetLayout(Rectangle windowSize, Layout layout)
        {
            int h = Position.Height / _elements.Count;
            int c = 0;
            Rectangle pos = new Rectangle();

            if (layout == Layout.LeftAlign)
            {
                pos = Position;
                foreach (TextSprite ts in _elements)
                {
                    pos.Y += c * h;
                    ts.setRelatvePos(pos, windowSize.Width, windowSize.Height);
                    c++;
                }
            }
            else if (layout == Layout.MiddleAlign)
            {
                pos = Position;
                foreach (TextSprite ts in _elements)
                {
                    pos.Y += c * h;
                    pos.Width = (int)(((float)h / ts.GetOriginalTextSize().Y) * ts.GetOriginalTextSize().X);
                    pos.X = Position.X + Position.Width / 2 - pos.Width / 2;
                    if (pos.X < Position.X)
                    {
                        pos.X = Position.X;
                        pos.Width = Position.Width;
                    }
                    ts.setRelatvePos(pos, windowSize.Width, windowSize.Height);
                    c++;
                }
            }
        }
        public void LoadFromFile(string filename, Color color, Rectangle windowSize)
        {
            foreach (string line in EugLib.IO.FileStream.readFileLines(filename))
            {
                _elements.Add(new TextSprite(AssetName, windowSize, new Rectangle(), line, color));
                _elements.Last().LoadContent(_spriteFont);
            }
            SetLayout(windowSize, Layout.LeftAlign);
        }
        public override void windowResized(Rectangle rect, Rectangle oldWindowSize = new Rectangle())
        {
            foreach (TextSprite ts in _elements)
                ts.windowResized(rect, oldWindowSize);
        }
        public override void setRelatvePos(Rectangle a_position, int windowWidth, int windowHeight)
        {
            foreach (TextSprite ts in _elements)
                ts.setRelatvePos(a_position, windowWidth, windowHeight);
        }
    }
}
