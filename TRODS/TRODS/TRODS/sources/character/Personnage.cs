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
    class Personnage : Character
    {
        public enum KeysActions
        {
            WalkRight, WalkLeft, WalkUp, WalkDown, Jump,
            Attack1, AttackStun, Attack2
        };

        private InputManager<Personnage.KeysActions, Keys> _inputManager;
        private ExperienceCounter _experience;

        public float Mana { get; set; }

        internal ExperienceCounter Experience
        {
            get
            {
                return this._experience;
            }
            private set
            {
                this._experience = value;
            }
        }

        public Personnage(Rectangle winsize, Vector2 position)
            : base(winsize, position, 140, 190, "game\\perso", 15, 4)
        {
            this._graphicalBounds = new GraphicalBounds<CharacterActions>(new Dictionary<CharacterActions, Rectangle>());
            this._graphicalBounds.set(CharacterActions.WalkRight, 3, 6, 15, 30);
            this._graphicalBounds.set(CharacterActions.WalkLeft, 18, 21, 30, 30);
            this._graphicalBounds.set(CharacterActions.StandRight, 1, 1, 2, 4);
            this._graphicalBounds.set(CharacterActions.StandLeft, 16, 16, 17, 4);
            this._graphicalBounds.set(CharacterActions.JumpRight, 31, 31, 35, 30);
            this._graphicalBounds.set(CharacterActions.JumpLeft, 36, 36, 40, 30);
            this._graphicalBounds.set(CharacterActions.Attack1Right, 41, 41, 49, 50);
            this._graphicalBounds.set(CharacterActions.Attack1Left, 50, 50, 58, 50);
            this._graphicalBounds.set(CharacterActions.AttackStunRight, 1, 1, 2, 4);
            this._graphicalBounds.set(CharacterActions.AttackStunLeft, 16, 16, 17, 4);
            this._graphicalBounds.set(CharacterActions.ReceiveAttackRight, 60, 60, 60, 30);
            this._graphicalBounds.set(CharacterActions.ReceiveAttackLeft, 59, 59, 59, 30);
            this._graphicalBounds.set(CharacterActions.Attack2Right, 1, 1, 2, 5);
            this._graphicalBounds.set(CharacterActions.Attack2Left, 16, 16, 17, 5);
            this.Action = CharacterActions.StandRight;
            this._physics.MaxHeight = 400;
            this._physics.TimeOnFlat = 500;
            this._inputManager = new InputManager<Personnage.KeysActions, Keys>();
            this._weapons.Add(new Weapon(winsize, "game/weapon", this.Sprite.Lignes, this.Sprite.Colonnes, this.Sprite.Position.Width, this.Sprite.Position.Height, 1f));
            Enumerable.Last<Weapon>((IEnumerable<Weapon>)this._weapons).Tip = new Tip(this._windowSize, new Rectangle(this._windowSize.Width - 60, this._windowSize.Height - 60, 40, 40), "game/tips/sword2_tip", "SpriteFont1", "x" + this.Weapons[0].Damage.ToString() + "   ", Color.Gold);
            this._weapons.Add(new Weapon(winsize, "game/weapon2", this.Sprite.Lignes, this.Sprite.Colonnes, this.Sprite.Position.Width, this.Sprite.Position.Height, 1.5f));
            Enumerable.Last<Weapon>((IEnumerable<Weapon>)this._weapons).Tip = new Tip(this._windowSize, new Rectangle(this._windowSize.Width - 110, this._windowSize.Height - 60, 40, 40), "game/tips/sword3_tip", "SpriteFont1", "x" + this.Weapons[1].Damage.ToString() + "   ", Color.Gold);
            this.Weapon = 0;
            this.InitKeys();
            this.actualizeSpriteGraphicalBounds();
            this.actualizeSpritePosition();
            this.Jump();
            this.Mana = 1f;
            this._experience = new ExperienceCounter(ExperienceCounter.Growth.Cuadratic, 200);
            this.AddAttack(CharacterActions.AttackStunLeft, new Attack(this._windowSize, new AnimatedSprite(new Rectangle(0, 0, 400, 400), this._windowSize, "sprites/expl_spread_6x6", 6, 6, 30, 1, 32, 1, true), 1500, 1.0f / 1000.0f, 3000, 400, 0.3f));
            this.AddAttack(CharacterActions.AttackStunRight, new Attack(this._windowSize, new AnimatedSprite(new Rectangle(0, 0, 400, 400), this._windowSize, "sprites/expl_spread_6x6", 6, 6, 30, 1, 32, 1, true), 1500, 1.0f / 1000.0f, 3000, 400, 0.3f));
            this.AddAttack(CharacterActions.Attack1Right, new Attack(this._windowSize, new AnimatedSprite(new Rectangle(0, 0, 10, 10), this._windowSize, "general/vide", 1, 1, 30, 1, -1, -1, false), 50, 0.1f, 280, 300, 0.05f));
            this.AddAttack(CharacterActions.Attack1Left, new Attack(this._windowSize, new AnimatedSprite(new Rectangle(0, 0, 10, 10), this._windowSize, "general/vide", 1, 1, 30, 1, -1, -1, false), 50, 0.1f, 280, 300, 0.05f));
            AnimatedSprite a = new AnimatedSprite(new Rectangle(0, 0, 100, 100), this._windowSize, "sprites/distant_attack_5x7", 5, 7, 30, 1, 35, 1, false);
            a.Direction = new Vector2(-2, 1); a.Vitesse = 0.15f;
            this.AddAttack(CharacterActions.Attack2Left, new Attack(this._windowSize, a, 2000, 0.05f, 1000, 1000, 0.5f));
            a = new AnimatedSprite(new Rectangle(0, 0, 100, 100), this._windowSize, "sprites/distant_attack_5x7", 5, 7, 30, 1, 35, 1, false);
            a.Direction = new Vector2(2, 1); a.Vitesse = 0.15f;
            this.AddAttack(CharacterActions.Attack2Right, new Attack(this._windowSize, a, 2000, 0.05f, 1000, 1000, 0.5f));
        }

        public override void Update(float elapsedTime)
        {
            base.Update(elapsedTime);
            this.Mana += elapsedTime / 40000f;
            if ((double)this.Mana > 1.0)
            {
                this.Mana = 1f;
            }
            else
            {
                if ((double)this.Mana >= 0.0)
                    return;
                this.Mana = 0.0f;
            }
        }

        public override void Attack(CharacterActions attack)
        {
            if ((double)this.Mana > (double)this._attacks[attack].Consumption / (double)(1 + this.Experience.Level / 7))
                base.Attack(attack);
            if (!this._attacks.ContainsKey(attack))
                return;
            this.Mana -= this.Attacks[attack].Consumption / (float)(1 + this.Experience.Level / 7);
        }

        public override void HandleInput(KeyboardState newKeyboardState, MouseState newMouseState, Game1 parent)
        {
            if (!this._canMove)
                return;
            if (!this._jumping)
            {
                if (newKeyboardState.IsKeyDown(this._inputManager.Get(Personnage.KeysActions.WalkRight)))
                    this.Move(true);
                else if (newKeyboardState.IsKeyDown(this._inputManager.Get(Personnage.KeysActions.WalkLeft)))
                    this.Move(false);
                else if (newKeyboardState.IsKeyDown(this._inputManager.Get(Personnage.KeysActions.WalkUp)))
                    this.Move(this._direction);
                else if (newKeyboardState.IsKeyDown(this._inputManager.Get(Personnage.KeysActions.WalkDown)))
                    this.Move(this._direction);
                else
                    this.Stand(this._direction);
                if (newKeyboardState.IsKeyDown(this._inputManager.Get(Personnage.KeysActions.Jump)))
                    this.Jump();
            }
            if (newKeyboardState.IsKeyDown(this._inputManager.Get(Personnage.KeysActions.Attack1)))
            {
                this.Action = this._direction ? CharacterActions.Attack1Right : CharacterActions.Attack1Left;
                this.Attack(this.Action);
                if (this.Attacks.ContainsKey(this.Action))
                    this._timer = this.Attacks[this.Action].AttackTime;
                this.actualizeSpriteGraphicalBounds();
            }
            if (newKeyboardState.IsKeyDown(this._inputManager.Get(Personnage.KeysActions.AttackStun)))
            {
                this._canMove = false;
                this.Action = this._direction ? CharacterActions.AttackStunRight : CharacterActions.AttackStunLeft;
                if (this.Attacks.ContainsKey(this.Action))
                    this._timer = this.Attacks[this.Action].AttackTime;
                this.Attack(this.Action);
                this.actualizeSpriteGraphicalBounds();
            }
            if (newKeyboardState.IsKeyDown(this._inputManager.Get(Personnage.KeysActions.Attack2)))
            {
                this._canMove = false;
                this.Action = this._direction ? CharacterActions.Attack2Right : CharacterActions.Attack2Left;
                if (this.Attacks.ContainsKey(this.Action))
                    this._timer = this.Attacks[this.Action].AttackTime;
                this.Attack(this.Action);
                this.actualizeSpriteGraphicalBounds();
            }
        }

        public void InitKeys()
        {
            this._inputManager.Add(Personnage.KeysActions.WalkRight, Keys.Right);
            this._inputManager.Add(Personnage.KeysActions.WalkLeft, Keys.Left);
            this._inputManager.Add(Personnage.KeysActions.WalkUp, Keys.Up);
            this._inputManager.Add(Personnage.KeysActions.WalkDown, Keys.Down);
            this._inputManager.Add(Personnage.KeysActions.Jump, Keys.Space);
            this._inputManager.Add(Personnage.KeysActions.Attack1, Keys.X);
            this._inputManager.Add(Personnage.KeysActions.AttackStun, Keys.A);
            this._inputManager.Add(Personnage.KeysActions.Attack2, Keys.S);
        }

        public List<Tip> GetSkillsTips()
        {
            return new List<Tip>()
      {
        new Tip(this._windowSize, new Rectangle(20, this._windowSize.Height - 50, 40, 40), "game/tips/sword1_tip", "SpriteFont1", ((object) this._inputManager.Get(Personnage.KeysActions.Attack1)).ToString(), Color.Gold),
        new Tip(this._windowSize, new Rectangle(70, this._windowSize.Height - 50, 40, 40), "game/tips/stun_tip", "SpriteFont1", ((object) this._inputManager.Get(Personnage.KeysActions.AttackStun)).ToString(), Color.Gold),
        new Tip(this._windowSize, new Rectangle(120, this._windowSize.Height - 50, 40, 40), "game/tips/distant_attack_tip", "SpriteFont1", ((object) this._inputManager.Get(Personnage.KeysActions.Attack2)).ToString(), Color.Gold)
      };
        }

        public float Damage()
        {
            return _weapons[Weapon].Damage * (Experience.Level > 10 ? (float)Experience.Level / 10f : 1);
        }
    }
}