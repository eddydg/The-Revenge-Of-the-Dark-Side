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
    class Character : AbstractScene
    {
        protected Dictionary<CharacterActions, Attack> _attacks;
        protected Rectangle _windowSize;
        protected GraphicalBounds<CharacterActions> _graphicalBounds;
        private CharacterActions _action;
        protected AnimatedSprite _sprite;
        protected Vector2 _position;
        protected Physics _physics;
        protected bool _direction;
        protected int _timer;
        protected List<Weapon> _weapons;

        public Dictionary<CharacterActions, Attack> Attacks
        {
            get
            {
                return this._attacks;
            }
            private set
            {
                this._attacks = value;
            }
        }

        public CharacterActions Action
        {
            get
            {
                return this._action;
            }
            set
            {
                this._action = value;
            }
        }

        public Vector2 Position
        {
            get
            {
                return this._position;
            }
            set
            {
                this._position = value;
            }
        }

        public Rectangle DrawingRectangle
        {
            get
            {
                return this._sprite != null ? this._sprite.Position : new Rectangle();
            }
        }

        public bool _canMove { get; protected set; }

        public bool _isOnGround { get; protected set; }

        public bool _jumping { get; protected set; }

        public int _jumpHeight { get; protected set; }

        public List<Weapon> Weapons
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

        public int Weapon { get; set; }

        public float Life { get; set; }

        public Character(Rectangle winSize, Vector2 position, int width, int height, string assetName, int textureColumns, int textureLines)
        {
            this._windowSize = winSize;
            this._position = position;
            this._weapons = new List<Weapon>();
            this._graphicalBounds = new GraphicalBounds<CharacterActions>(new Dictionary<CharacterActions, Rectangle>());
            this._sprite = new AnimatedSprite(new Rectangle((int)position.X - width / 2, (int)position.Y - height, width, height), winSize, assetName, textureColumns, textureLines, 30, 1, -1, -1, true);
            this._canMove = true;
            this._jumping = false;
            this._jumpHeight = 0;
            this._direction = true;
            this._timer = 0;
            this._physics = new Physics(200, 1000);
            this.Life = 1f;
            this._action = CharacterActions.StandRight;
            this._attacks = new Dictionary<CharacterActions, Attack>();
        }

        public override void LoadContent(ContentManager content)
        {
            ((AbstractScene)this._sprite).LoadContent(content);
            foreach (Weapon weapon in this._weapons)
            {
                if (weapon != null)
                    weapon.LoadContent(content);
            }
            foreach (AbstractScene abstractScene in this._attacks.Values)
                abstractScene.LoadContent(content);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!this._jumping)
            {
                ((AbstractScene)this._sprite).Draw(spriteBatch);
                if (this._weapons.Count > 0)
                    this.Weapons[this.Weapon].Draw(spriteBatch, this._sprite.Position);
            }
            else
            {
                this._sprite.Draw(spriteBatch, new Vector2((float)this._sprite.Position.X, (float)(this._sprite.Position.Y - this._jumpHeight)));
                if (this._weapons.Count > 0)
                    this.Weapons[this.Weapon].Draw(spriteBatch, new Rectangle(this._sprite.Position.X, this._sprite.Position.Y - this._jumpHeight, this._sprite.Position.Width, this._sprite.Position.Height));
            }
            foreach (AbstractScene abstractScene in this._attacks.Values)
                abstractScene.Draw(spriteBatch);
        }

        public override void Update(float elapsedTime)
        {
            this._timer -= (int)elapsedTime;
            this._sprite.Update(elapsedTime);
            if (this._weapons.Count > 0)
                this.Weapons[this.Weapon].Update(elapsedTime);
            if (!this._sprite._repeating && this._sprite.IsEnd())
                this.Stand(this._direction);
            if (this._jumping)
                this._jumpHeight += this._physics.Update(elapsedTime);
            this.testOnGround();
            foreach (AbstractScene abstractScene in this._attacks.Values)
                abstractScene.Update(elapsedTime);
            if (this._timer < 0 && !this._canMove)
            {
                this._timer = 0;
                this._canMove = true;
                this.Stand(this._direction);
            }
            if ((double)this.Life < 0.0)
            {
                this.Life = 0.0f;
            }
            else
            {
                if ((double)this.Life <= 1.0)
                    return;
                this.Life = 1f;
            }
        }

        public override void WindowResized(Rectangle rect)
        {
            this._sprite.windowResized(rect, new Rectangle());
            foreach (Weapon weapon in this._weapons)
                weapon.WindowResized(rect);
            float num = (float)rect.Width / (float)this._windowSize.Width;
            float yRapt = (float)rect.Height / (float)this._windowSize.Height;
            this._physics.WindowResized(yRapt);
            this._position.X *= num;
            this._position.Y *= yRapt;
            this._jumpHeight = (int)((double)this._jumpHeight * (double)yRapt);
            foreach (AbstractScene abstractScene in this._attacks.Values)
                abstractScene.WindowResized(rect);
            this._windowSize = rect;
        }

        public virtual void AddAttack(CharacterActions action, Attack attac)
        {
            this._attacks.Add(action, attac);
        }

        public virtual void Attack(CharacterActions attack)
        {
            if (!this._attacks.ContainsKey(attack))
                return;
            Rectangle position;
            switch (attack)
            {
                case CharacterActions.Attack1Right:
                    this._attacks[attack].Launch(new Rectangle((int)this.Position.X, this._sprite.Position.Y, this._sprite.Position.Width / 2, this._sprite.Position.Height));
                    break;
                case CharacterActions.Attack1Left:
                    this._attacks[attack].Launch(new Rectangle((int)this.Position.X - this._sprite.Position.Width / 2, (int)this.Position.Y - this._sprite.Position.Height, this._sprite.Position.Width / 2, this._sprite.Position.Height));
                    break;
                case CharacterActions.AttackStunRight:
                case CharacterActions.AttackStunLeft:
                    position = this._sprite.Position;
                    this._attacks[attack].Launch(new Rectangle(position.X - 2 * position.Width, position.Y - position.Height, 5 * position.Width, (int)(2.20000004768372 * (double)position.Height)));
                    break;
                case CharacterActions.Attack2Right:
                    position = this._sprite.Position;
                    this._attacks[attack].Launch(new Rectangle(position.X - position.Width/2, position.Y - position.Height,position.Width*2,position.Height));
                    break;
                case CharacterActions.Attack2Left:
                    position = this._sprite.Position;
                    this._attacks[attack].Launch(new Rectangle(position.X - position.Width / 2, position.Y - position.Height, position.Width * 2, position.Height));
                    break;
            }
            this._canMove = false;
            this._timer = this._attacks[attack].AttackTime;
            this._action = attack;
        }

        public virtual void ReceiveAttack(float damage = 0.0f, int blockTime = 100)
        {
            this._action = this._direction ? CharacterActions.ReceiveAttackRight : CharacterActions.ReceiveAttackLeft;
            this._canMove = false;
            this._timer = blockTime;
            this.actualizeSpriteGraphicalBounds();
            this.Life -= damage;
        }

        public virtual void Stand(bool right)
        {
            if (right == this._direction && (this._action == CharacterActions.StandRight || this._action == CharacterActions.StandLeft))
                return;
            this._direction = right;
            this._action = !right ? CharacterActions.StandLeft : CharacterActions.StandRight;
            this.actualizeSpriteGraphicalBounds();
        }

        public virtual void Jump()
        {
            if (!this._canMove || this._jumping)
                return;
            this._action = !this._direction ? CharacterActions.JumpLeft : CharacterActions.JumpRight;
            this.actualizeSpriteGraphicalBounds();
            this._jumpHeight = 0;
            this._jumping = true;
            this._isOnGround = false;
            this._physics.Jump();
        }

        public virtual void Move(bool right)
        {
            if (!this._canMove || right == this._direction && (this._action == CharacterActions.WalkRight || this._action == CharacterActions.WalkLeft))
                return;
            this._direction = right;
            this._action = !right ? CharacterActions.WalkLeft : CharacterActions.WalkRight;
            this.actualizeSpriteGraphicalBounds();
        }

        public virtual void Paralize(int time)
        {
            this._canMove = false;
            this._timer = time;
            this._action = CharacterActions.Paralized;
        }

        public virtual void Free()
        {
            this._canMove = true;
        }

        public virtual void DoubleDash()
        {
        }

        protected virtual bool testOnGround()
        {
            if (this._jumpHeight >= 0)
                return false;
            this._jumping = false;
            this._jumpHeight = 0;
            this._isOnGround = true;
            this.Stand(this._direction);
            return true;
        }

        protected virtual void actualizeSpritePosition()
        {
            this._sprite.setRelatvePos(new Rectangle((int)this._position.X - this._sprite.Position.Width / 2, (int)this._position.Y - this._sprite.Position.Height, this._sprite.Position.Width, this._sprite.Position.Height), this._windowSize.Width, this._windowSize.Height);
        }

        protected virtual void actualizeSpriteGraphicalBounds()
        {
            Rectangle rect = this._graphicalBounds.get(this._action);
            this._sprite.SetPictureBounds(rect.Y, rect.Width, rect.X, true);
            this._sprite.Speed = rect.Height;
            foreach (Weapon weapon in this._weapons)
            {
                if (weapon != null)
                    weapon.actualizeSpriteGraphicalBounds(rect);
            }
        }
    }
}
