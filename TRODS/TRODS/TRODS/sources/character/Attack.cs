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
    class Attack : AbstractScene
    {
        private AnimatedSprite _sprite;
        private int _lifetime;

        public bool Active { get; set; }

        public Rectangle Position
        {
            get
            {
                return this._sprite != null ? this._sprite.Position : new Rectangle();
            }
            private set
            {
            }
        }

        public int Duration { get; set; }

        public float Damage { get; set; }

        public int BlockTime { get; set; }

        public int AttackTime { get; set; }

        public float Consumption { get; set; }

        public Attack(Rectangle winSize, AnimatedSprite sprite, int duration = 50, float damage = 0.2f, int blockTime = 300, int attackTime = 50, float consumption = 0.1f)
        {
            this._sprite = sprite;
            this.Active = false;
            this._lifetime = 0;
            this.Duration = duration;
            this.Damage = damage;
            this.BlockTime = blockTime;
            this.AttackTime = attackTime;
            this.Consumption = consumption;
        }

        public void Launch(Rectangle position)
        {
            this.Active = true;
            this._lifetime = this.Duration;
            this._sprite.Position = position;
            this._sprite.ActualPicture = this._sprite.First;
        }

        public override void LoadContent(ContentManager content)
        {
            if (this._sprite == null || !(this._sprite.AssetName != ""))
                return;
            ((AbstractScene)this._sprite).LoadContent(content);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!this.Active || this._sprite == null)
                return;
            ((AbstractScene)this._sprite).Draw(spriteBatch);
        }

        public override void Update(float elapsedTime)
        {
            this._lifetime -= (int)elapsedTime;
            if (this.Active && this._sprite != null)
                this._sprite.Update(elapsedTime);
            if (this._lifetime >= 0)
                return;
            this.Active = false;
        }

        public void Move(int x, int y)
        {
            this._sprite.Position = new Rectangle(this._sprite.Position.X - x, this._sprite.Position.Y - y, this._sprite.Position.Width, this._sprite.Position.Height);
        }

        public override void WindowResized(Rectangle rect)
        {
            if (this._sprite == null)
                return;
            this._sprite.windowResized(rect, new Rectangle());
        }
    }
}
