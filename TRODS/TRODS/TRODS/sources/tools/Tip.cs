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
    class Tip : Sprite
    {
        private TextSprite _text;

        public Tip(Rectangle winsize, Rectangle position, string imageAssetName, string spriteFont, string text, Color color)
            : base(position, winsize, imageAssetName)
        {
            this._text = new TextSprite(spriteFont, winsize, position, color, text);
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);
            ((AbstractScene)this._text).LoadContent(content);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            ((AbstractScene)this._text).Draw(spriteBatch);
        }

        public override void WindowResized(Rectangle rect)
        {
            this.windowResized(rect, new Rectangle());
            this._text.windowResized(rect);
        }
    }
}
