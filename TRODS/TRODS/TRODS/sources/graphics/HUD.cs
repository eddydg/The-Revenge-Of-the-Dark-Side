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
    class HUD : AbstractScene
    {
        private Sprite _background;
        private AnimatedSprite _life;

        public float LifeLevel { get; set; }

        public HUD(Rectangle windowsize, string[] assetName)
        {
            LifeLevel = 0;
            if (assetName.Length > 0)
                _background = new Sprite(new Rectangle(0, 0, windowsize.Width, windowsize.Height / 5), windowsize, assetName[0]);
            if (assetName.Length > 1)
                _life = new AnimatedSprite(new Rectangle(278, 40, 300, 10), windowsize, assetName[1]);
        }

        public override void HandleInput(KeyboardState newKeyboardState, MouseState newMouseState, Game1 parent)
        {
            base.HandleInput(newKeyboardState, newMouseState, parent);
        }

        public override void LoadContent(ContentManager content)
        {
            _background.LoadContent(content);
            _life.LoadContent(content);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _background.Draw(spriteBatch);
            _life.Draw(spriteBatch, 100);
            _life.Draw(spriteBatch, new Rectangle(_life.Position.X, _life.Position.Y, (int)((float)_life.Position.Width * LifeLevel), _life.Position.Height), new Rectangle(0, 0, (int)((float)_life.Position.Width * LifeLevel), _life.Position.Height), Color.White);
        }

        public override void Update(float elapsedTime)
        {
            _background.Update(elapsedTime);
            _life.Update(elapsedTime);
        }

        public override void WindowResized(Rectangle rect)
        {
            _background.windowResized(rect);
            _life.windowResized(rect);
        }
    }
}
