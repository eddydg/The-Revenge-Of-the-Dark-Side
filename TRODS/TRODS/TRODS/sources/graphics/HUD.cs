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
        private AnimatedSprite _mana;
        private AnimatedSprite _xp;
        private TextSprite _levelText;
        private TextSprite _level;
        public String LevelText
        {
            get { return _level.Text; }
            set { _level.Text = value; }
        }
        private AnimatedSprite _portrait;
        private TextSprite _ennemiesLeftText;
        private TextSprite _ennemiesLeft;
        public String EnnemiesText
        {
            get { return _ennemiesLeft.Text; }
            set { _ennemiesLeft.Text = value; }
        }

        public float LifeLevel { get; set; }
        public float ManaLevel { get; set; }
        public float XpLevel { get; set; }

        public HUD(Rectangle windowsize)
        {
            //"game/HUD", "game/life_mob", "game/mana", "game/xp", "SpriteFont1"
            LifeLevel = 0;
            _background = new Sprite(new Rectangle(0, 0, windowsize.Width, windowsize.Height / 5), windowsize, "game/HUD");
            _life = new AnimatedSprite(new Rectangle(278, 20, 300, 10), windowsize, "game/life_mob");
            _mana = new AnimatedSprite(new Rectangle(278, 50, 300, 10), windowsize, "game/mana");
            _xp = new AnimatedSprite(new Rectangle(278, 80, 300, 10), windowsize, "game/xp");
            _levelText = new TextSprite("SpriteFont1", windowsize, new Rectangle(600, 20, 50, 20), "Level", Color.Gold);
            _level = new TextSprite("SpriteFont1", windowsize, new Rectangle(600, 40, 50, 70), "1", Color.Gold);
            _ennemiesLeftText = new TextSprite("SpriteFont1", windowsize, new Rectangle(700, 20, 60, 20), "Ennemies", Color.DarkRed);
            _ennemiesLeft = new TextSprite("SpriteFont1", windowsize, new Rectangle(700, 40, 60, 70), "--", Color.DarkRed);
            _portrait = new AnimatedSprite(new Rectangle(10, 10, 175, 90), windowsize, "game/persoPortrait");
        }

        public override void HandleInput(KeyboardState newKeyboardState, MouseState newMouseState, Game1 parent)
        {
            base.HandleInput(newKeyboardState, newMouseState, parent);
        }

        public override void LoadContent(ContentManager content)
        {
            _background.LoadContent(content);
            _life.LoadContent(content);
            _mana.LoadContent(content);
            _xp.LoadContent(content);
            _levelText.LoadContent(content);
            _level.LoadContent(content);
            _portrait.LoadContent(content);
            _ennemiesLeftText.LoadContent(content);
            _ennemiesLeft.LoadContent(content);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _background.Draw(spriteBatch);
            _life.Draw(spriteBatch, 100);
            _life.Draw(spriteBatch, new Rectangle(
                                            _life.Position.X,
                                            _life.Position.Y,
                                            (int)((float)_life.Position.Width * LifeLevel),
                                            _life.Position.Height),
                                    new Rectangle(0, 0,
                                                (int)((float)_life.Position.Width * LifeLevel),
                                                _life.Position.Height), Color.White);

            _mana.Draw(spriteBatch, 100);
            _mana.Draw(spriteBatch, new Rectangle(
                                            _mana.Position.X,
                                            _mana.Position.Y,
                                            (int)((float)_mana.Position.Width * ManaLevel),
                                            _mana.Position.Height),
                                    new Rectangle(0, 0,
                                                (int)((float)_mana.Position.Width * ManaLevel),
                                                _mana.Position.Height), Color.White);

            _xp.Draw(spriteBatch, 100);
            _xp.Draw(spriteBatch, new Rectangle(
                                            _xp.Position.X,
                                            _xp.Position.Y,
                                            (int)((float)_xp.Position.Width * XpLevel),
                                            _xp.Position.Height),
                                    new Rectangle(0, 0,
                                                (int)((float)_xp.Position.Width * XpLevel),
                                                _xp.Position.Height), Color.White);
            _levelText.Draw(spriteBatch);
            _level.Draw(spriteBatch);
            _portrait.Draw(spriteBatch);
            _ennemiesLeftText.Draw(spriteBatch);
            _ennemiesLeft.Draw(spriteBatch);
        }

        public override void Update(float elapsedTime)
        {
            _background.Update(elapsedTime);
            _life.Update(elapsedTime);
            _mana.Update(elapsedTime);
            _xp.Update(elapsedTime);
            _levelText.Update(elapsedTime);
            _level.Update(elapsedTime);
            _portrait.Update(elapsedTime);
            _ennemiesLeftText.Update(elapsedTime);
            _ennemiesLeft.Update(elapsedTime);
        }

        public override void WindowResized(Rectangle rect)
        {
            _background.windowResized(rect);
            _life.windowResized(rect);
            _mana.windowResized(rect);
            _xp.windowResized(rect);
            _levelText.windowResized(rect);
            _level.windowResized(rect);
            _portrait.windowResized(rect);
            _ennemiesLeftText.windowResized(rect);
            _ennemiesLeft.windowResized(rect);
        }
    }
}
