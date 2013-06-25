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
    class Mob : Character
    {
        private Rectangle _playingZone;
        private Vector2 _mapSpeed;
        private Vector2 _speed;
        private IA _ia;
        private Sprite _lifeSprite;
        public int GivenExp { get; set; }

        public Rectangle PlayingZone
        {
            get
            {
                return this._playingZone;
            }
            set
            {
                this._playingZone = value;
            }
        }

        internal IA Ia
        {
            get
            {
                return this._ia;
            }
            set
            {
                this._ia = value;
            }
        }

        public Mob(Rectangle winSize, int seed, Vector2 position, int width, int height, string assetName, int textureColumns, int textureLines, Vector2 speed, Vector2 mapSpeed, int attackSpeed, int attackDistance, Rectangle playingZone)
            : base(winSize, position, width, height, assetName, textureColumns, textureLines)
        {
            this._ia = new IA(winSize, seed, speed, attackDistance, 300, attackSpeed);
            this._playingZone = playingZone;
            this._mapSpeed = mapSpeed;
            this._speed = speed;
            GivenExp = 50;
            this.Life = 1f;
            this._lifeSprite = new Sprite(new Rectangle(this.Sprite.Position.X, this.Sprite.Position.Y - (int)(0.0333333350718021 * (double)this.Sprite.Position.Height), this.Sprite.Position.Width, (int)(0.0333333350718021 * (double)this.Sprite.Position.Height)), this._windowSize, "game/life_mob");
            this.AddAttack(CharacterActions.Attack1Right, new Attack(this._windowSize, new AnimatedSprite(new Rectangle(0, 0, 10, 10), this._windowSize, "general/vide", 1, 1, 30, 1, -1, -1, false), 50, 0.005f, 50, 500, 0.05f));
            this.AddAttack(CharacterActions.Attack1Left, new Attack(this._windowSize, new AnimatedSprite(new Rectangle(0, 0, 10, 10), this._windowSize, "general/vide", 1, 1, 30, 1, -1, -1, false), 50, 0.005f, 50, 500, 0.05f));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            this._lifeSprite.Draw(spriteBatch, (byte)30);
            this._lifeSprite.Draw(spriteBatch, new Rectangle(this._lifeSprite.Position.X, this._lifeSprite.Position.Y, (int)((double)this._lifeSprite.Position.Width * (double)this.Life), this._lifeSprite.Position.Height), new Rectangle(0, 0, (int)((double)this._lifeSprite.Position.Width * (double)this.Life), this._lifeSprite.Position.Height), Color.FromNonPremultiplied((int)byte.MaxValue, (int)byte.MaxValue, (int)byte.MaxValue, 200));
        }

        public override void Update(float elapsedTime)
        {
            base.Update(elapsedTime);
            this._ia.Update(elapsedTime);
            if (this._timer < 0)
            {
                if (!this._ia.IsNearPerso)
                {
                    Mob mob = this;
                    Vector2 vector2 = mob._position + this._ia.Deplacement * this._speed;
                    mob._position = vector2;
                }
                else if (!this._ia._attack)
                    this.Stand(this._direction);
                if (this._ia._attack && this.Action != CharacterActions.Attack1Left && this.Action != CharacterActions.Attack1Right)
                {
                    this.Action = this._direction ? CharacterActions.Attack1Right : CharacterActions.Attack1Left;
                    this.Attack(this.Action);
                    this._ia._attack = false;
                    this.actualizeSpriteGraphicalBounds();
                }
                else if (this._canMove && !this._ia.IsNearPerso)
                    base.Move((double)this._ia.Deplacement.X > 0.0);
            }
            this._lifeSprite.setRelatvePos(new Rectangle(this.Sprite.Position.X + this.Sprite.Position.Width / 4, this.Sprite.Position.Y - (int)(0.0500000007450581 * (double)this.Sprite.Position.Height), this.Sprite.Position.Width - this.Sprite.Position.Width / 2, (int)(0.0500000007450581 * (double)this.Sprite.Position.Height)), this._windowSize.Width, this._windowSize.Height);
            this.actualizeSpritePosition();
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);
            ((AbstractScene)this._lifeSprite).LoadContent(content);
        }

        public void Move(int x, int y)
        {
            this._playingZone.X -= (int)((double)x * (double)this._mapSpeed.X);
            this._playingZone.Y -= (int)((double)y * (double)this._mapSpeed.Y);
            this._position.X -= (float)(int)((double)x * (double)this._mapSpeed.X);
            this._position.Y -= (float)(int)((double)y * (double)this._mapSpeed.Y);
        }

        public void AddGraphicalBounds(CharacterActions action, Rectangle bounds)
        {
            this._graphicalBounds.set(action, bounds);
        }

        public void Actualize(Vector2 posPerso)
        {
            if (this._ia.IsNearPerso)
                this._direction = (double)posPerso.X > (double)this.Position.X;
            this._ia.Actualize(posPerso, this._position, this._playingZone);
        }

        public override void Paralize(int time)
        {
            base.Paralize(time);
            this.Ia._attack = false;
        }

        public override void WindowResized(Rectangle rect)
        {
            float num1 = (float)rect.Width / (float)this._windowSize.Width;
            float num2 = (float)rect.Height / (float)this._windowSize.Height;
            this._lifeSprite.windowResized(rect, this._windowSize);
            base.WindowResized(rect);
            this._ia.WindowResized(rect);
            this.PlayingZone = new Rectangle((int)((double)this._playingZone.X * (double)num1), (int)((double)this._playingZone.Y * (double)num2), (int)((double)this._playingZone.Width * (double)num1), (int)((double)this._playingZone.Height * (double)num2));
            this._speed.X *= num1;
            this._speed.Y *= num2;
            this._windowSize = rect;
        }
    }
}
