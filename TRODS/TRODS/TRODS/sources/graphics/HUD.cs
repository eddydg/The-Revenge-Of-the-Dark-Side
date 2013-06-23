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
        private List<Tip> _weapons;
        private List<Sprite> _sprites;
        private Sprite _background;
        private AnimatedSprite _life;
        private AnimatedSprite _mana;
        private AnimatedSprite _xp;
        private TextSprite _level;
        private AnimatedSprite _portrait;
        private TextSprite _ennemiesLeft;

        public List<Tip> Weapons
        {
            get
            {
                return this._weapons;
            }
            private set
            {
                this._weapons = value;
            }
        }

        public string LevelText
        {
            get
            {
                return this._level.Text;
            }
            set
            {
                this._level.Text = value;
            }
        }

        public string EnnemiesText
        {
            get
            {
                return this._ennemiesLeft.Text;
            }
            set
            {
                this._ennemiesLeft.Text = value;
            }
        }

        public float LifeLevel { get; set; }

        public float ManaLevel { get; set; }

        public float XpLevel { get; set; }

        public HUD(Rectangle windowsize)
        {
            this.LifeLevel = 0.0f;
            this._weapons = new List<Tip>();
            this._background = new Sprite(new Rectangle(0, 0, windowsize.Width, windowsize.Height / 5), windowsize, "game/HUD");
            this._life = new AnimatedSprite(new Rectangle(278, 20, 300, 10), windowsize, "game/life_mob", 1, 1, 30, 1, -1, -1, false);
            this._mana = new AnimatedSprite(new Rectangle(278, 50, 300, 10), windowsize, "game/mana", 1, 1, 30, 1, -1, -1, false);
            this._xp = new AnimatedSprite(new Rectangle(278, 80, 300, 10), windowsize, "game/xp", 1, 1, 30, 1, -1, -1, false);
            this._level = new TextSprite("SpriteFont1", windowsize, new Rectangle(600, 40, 50, 70), Color.Gold, "1");
            this._ennemiesLeft = new TextSprite("SpriteFont1", windowsize, new Rectangle(700, 40, 60, 70), Color.DarkRed, "--");
            this._portrait = new AnimatedSprite(new Rectangle(10, 10, 175, 90), windowsize, "game/persoPortrait", 1, 1, 30, 1, -1, -1, false);
            this._sprites = new List<Sprite>();
            this._sprites.Add((Sprite)new TextSprite("SpriteFont1", windowsize, new Rectangle(600, 20, 50, 20), Color.Gold, "Level"));
            this._sprites.Add((Sprite)new TextSprite("SpriteFont1", windowsize, new Rectangle(700, 20, 60, 20), Color.DarkRed, "Enemies"));
            this._sprites.Add((Sprite)new TextSprite("SpriteFont1", windowsize, new Rectangle(215, 15, 45, 20), Color.Red, "Life"));
            this._sprites.Add((Sprite)new TextSprite("SpriteFont1", windowsize, new Rectangle(215, 45, 45, 20), Color.CornflowerBlue, "Mana"));
            this._sprites.Add((Sprite)new TextSprite("SpriteFont1", windowsize, new Rectangle(215, 75, 45, 20), Color.Gold, "Exp"));
        }

        public void AddSprite(Sprite sprite)
        {
            this._sprites.Add(sprite);
        }

        public override void LoadContent(ContentManager content)
        {
            ((AbstractScene)this._background).LoadContent(content);
            ((AbstractScene)this._life).LoadContent(content);
            ((AbstractScene)this._mana).LoadContent(content);
            ((AbstractScene)this._xp).LoadContent(content);
            ((AbstractScene)this._level).LoadContent(content);
            ((AbstractScene)this._portrait).LoadContent(content);
            ((AbstractScene)this._ennemiesLeft).LoadContent(content);
            foreach (AbstractScene abstractScene in this._sprites)
                abstractScene.LoadContent(content);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            ((AbstractScene)this._background).Draw(spriteBatch);
            this._life.Draw(spriteBatch, (byte)100);
            this._life.Draw(spriteBatch, new Rectangle(this._life.Position.X, this._life.Position.Y, (int)((double)this._life.Position.Width * (double)this.LifeLevel), this._life.Position.Height), new Rectangle(0, 0, (int)((double)this._life.Position.Width * (double)this.LifeLevel), this._life.Position.Height), Color.White);
            this._mana.Draw(spriteBatch, (byte)100);
            this._mana.Draw(spriteBatch, new Rectangle(this._mana.Position.X, this._mana.Position.Y, (int)((double)this._mana.Position.Width * (double)this.ManaLevel), this._mana.Position.Height), new Rectangle(0, 0, (int)((double)this._mana.Position.Width * (double)this.ManaLevel), this._mana.Position.Height), Color.White);
            this._xp.Draw(spriteBatch, (byte)100);
            this._xp.Draw(spriteBatch, new Rectangle(this._xp.Position.X, this._xp.Position.Y, (int)((double)this._xp.Position.Width * (double)this.XpLevel), this._xp.Position.Height), new Rectangle(0, 0, (int)((double)this._xp.Position.Width * (double)this.XpLevel), this._xp.Position.Height), Color.White);
            ((AbstractScene)this._level).Draw(spriteBatch);
            ((AbstractScene)this._portrait).Draw(spriteBatch);
            ((AbstractScene)this._ennemiesLeft).Draw(spriteBatch);
            foreach (AbstractScene abstractScene in this._sprites)
                abstractScene.Draw(spriteBatch);
            foreach (AbstractScene abstractScene in this._weapons)
                abstractScene.Draw(spriteBatch);
        }

        public override void Update(float elapsedTime)
        {
            this._background.Update(elapsedTime);
            this._life.Update(elapsedTime);
            this._mana.Update(elapsedTime);
            this._xp.Update(elapsedTime);
            this._level.Update(elapsedTime);
            this._portrait.Update(elapsedTime);
            this._ennemiesLeft.Update(elapsedTime);
            foreach (AbstractScene abstractScene in this._sprites)
                abstractScene.Update(elapsedTime);
            foreach (AbstractScene abstractScene in this._weapons)
                abstractScene.Update(elapsedTime);
        }

        public int SelectedWeapon(MouseState mouse)
        {
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                for (int index = 0; index < this._weapons.Count; ++index)
                {
                    if (this._weapons[index].Position.Contains(mouse.X, mouse.Y))
                        return index;
                }
            }
            return -1;
        }

        public void AddWeapon(Tip t)
        {
            this._weapons.Add(t);
        }

        public void RemoveWeapon(int i)
        {
            try
            {
                _weapons.RemoveAt(i);
            }
            catch (Exception) { }
        }

        public override void WindowResized(Rectangle rect)
        {
            this._background.windowResized(rect, new Rectangle());
            this._life.windowResized(rect, new Rectangle());
            this._mana.windowResized(rect, new Rectangle());
            this._xp.windowResized(rect, new Rectangle());
            this._level.windowResized(rect, new Rectangle());
            this._portrait.windowResized(rect, new Rectangle());
            this._ennemiesLeft.windowResized(rect, new Rectangle());
            foreach (Sprite sprite in this._sprites)
                sprite.windowResized(rect, new Rectangle());
            foreach (Tip sprite in this._weapons)
                sprite.WindowResized(rect);
        }
    }
}