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
    class InGame : AbstractScene
    {
        private KeyboardState _keyboardState;
        private MouseState _mouseState;
        private Rectangle _windowSize;
        private Sprite mouse;
        private List<AbstractMap> _maps;
        private int _currentMap;
        private ContextMenu _menu;
        private Personnage personnage;
        private Random rand;
        private List<Texture2D> _mobsTextures;
        private List<List<Mob>> _mobs;
        private HUD _hud;
        private Rectangle _originalWindowSize;

        private List<Personnage> _players;
        private EugLib.Net.Server _server;
        private EugLib.Net.Client _client;

        private bool _dashing, _waitingDash;
        private float _dashTimer;
        private const float DashDuration = 1000;
        private const float DashKeyDelay = 200;
        private float DashSpeed = 4;

        public InGame(Rectangle windowSize, KeyboardState keyboardState, MouseState mouseState)
        {
            this._windowSize = windowSize;
            _dashing = _waitingDash = false;
            _dashTimer = 0;
            this._mouseState = mouseState;
            this._keyboardState = keyboardState;
            this._originalWindowSize = windowSize;
            this._menu = new ContextMenu(this._windowSize, new AnimatedSprite(new Rectangle(0, 30, 200, 50), this._windowSize, "game/menu/container", 1, 1, 30, 1, -1, -1, false), "menu/contextMenuExit", (byte)235);
            this._menu.Title = new AnimatedSprite(new Rectangle(this._menu.Position.Width / 2 - 75, 0, 150, 50), this._windowSize, "menu/contextMenuText", 1, 1, 30, 1, -1, -1, false);
            this._menu.Visible = false;
            this._menu.Add(new AnimatedSprite(new Rectangle(this._menu.Position.Width / 2 - 100, 65, 200, 22), this._windowSize, "menu/contextMenuTextMainMenu", 1, 1, 30, 1, -1, -1, false));
            this._menu.Add(new TextSprite("SpriteFont1", _windowSize, new Rectangle(0, 0, 200, 22), Color.Red, "Start Server"));
            this._menu.Add(new TextSprite("SpriteFont1", _windowSize, new Rectangle(0, 0, 200, 22), Color.Red, "Connection"));
            this._menu.CuadricPositionning(new Rectangle(0, 0, 150, 20), 65, 15, 10, 10, true);
            this.mouse = new Sprite(new Rectangle(-100, -100, 30, 50), this._windowSize, "");
            this._maps = new List<AbstractMap>();
            AbstractMap abstractMap1 = new AbstractMap(this._windowSize);
            abstractMap1.AddVisitable(450, 450, 2800, 130);
            abstractMap1.VuePosition = new Vector2(460f, 579f);
            abstractMap1.Elements.Add(new AbstractMap.Element(new AnimatedSprite(new Rectangle(0, 0, this._windowSize.Width, this._windowSize.Height), windowSize, "map1/sky1", 1, 1, 30, 1, -1, -1, false), 0.2f, 0.0f, true, false, false, true, false));
            abstractMap1.Elements.Add(new AbstractMap.Element(new AnimatedSprite(new Rectangle(0, 190, 960, 300), windowSize, "map1/back1r", 1, 1, 30, 1, -1, -1, false), 0.8f, 0.2f, true, false, false, true, false));
            abstractMap1.Elements.Add(new AbstractMap.Element(new AnimatedSprite(new Rectangle(0, 415, 1082, 193), windowSize, "map1/fore1", 1, 1, 30, 1, -1, -1, false), 1f, 0.5f, true, false, false, true, false));
            abstractMap1.Elements.Add(new AbstractMap.Element(new AnimatedSprite(new Rectangle(400, 450, 150, 150), this._windowSize, "game/heal_font_6x6", 6, 6, 30, 1, 32, 1, true), 1f, 0.5f, false, false, false, false, true));
            abstractMap1.Elements.Add(new AbstractMap.Element(new AnimatedSprite(new Rectangle(5, 150, this._windowSize.Width / 2, this._windowSize.Height - 150), this._windowSize, "sprites/fireWall_11x6r23r44", 11, 6, 30, 23, 44, 1, true), 1f, 0.5f, false, false, false, true, false));
            abstractMap1.Elements.Add(new AbstractMap.Element(new AnimatedSprite(new Rectangle(2550 + this._windowSize.Width / 2, 280, 320, 320), this._windowSize, "sprites/portal_6x6", 6, 6, 30, 1, 32, 1, true), 1f, 0.5f, false, false, true, true, false));
            this._maps.Add(abstractMap1);
            AbstractMap abstractMap2 = new AbstractMap(this._windowSize);
            abstractMap2.AddVisitable(450, 450, 2800, 130);
            abstractMap2.VuePosition = new Vector2(460f, 579f);
            abstractMap2.Elements.Add(new AbstractMap.Element(new AnimatedSprite(new Rectangle(0, 0, 1040, 320), windowSize, "map2/sky", 1, 1, 30, 1, -1, -1, false), 0.2f, 0.0f, true, false, false, true, false));
            abstractMap2.Elements.Add(new AbstractMap.Element(new AnimatedSprite(new Rectangle(0, 190, 960, 300), windowSize, "map2/mountain", 1, 1, 30, 1, -1, -1, false), 0.8f, 0.2f, true, false, false, true, false));
            abstractMap2.Elements.Add(new AbstractMap.Element(new AnimatedSprite(new Rectangle(0, 415, 1082, 193), windowSize, "map2/sand", 1, 1, 30, 1, -1, -1, false), 1f, 0.5f, true, false, false, true, false));
            abstractMap2.Elements.Add(new AbstractMap.Element(new AnimatedSprite(new Rectangle(0, 515, 1066, 92), windowSize, "map2/rock", 1, 1, 30, 1, -1, -1, false), 1f, 0.5f, true, true, false, true, false));
            abstractMap2.Elements.Add(new AbstractMap.Element(new AnimatedSprite(new Rectangle(250, 280, 320, 320), this._windowSize, "sprites/portal_6x6", 6, 6, 30, 1, 32, 1, true), 1f, 0.5f, false, false, true, false, false));
            abstractMap2.Elements.Add(new AbstractMap.Element(new AnimatedSprite(new Rectangle(2800 + this._windowSize.Width / 2, 150, this._windowSize.Width / 2, this._windowSize.Height - 150), this._windowSize, "sprites/fireWall_11x6r23r44", 11, 6, 30, 23, 44, 1, true), 1f, 0.5f, false, false, false, true, false));
            this._maps.Add(abstractMap2);
            this._currentMap = 0;
            this.personnage = new Personnage(this._windowSize, abstractMap2.VuePosition);
            this._mobsTextures = new List<Texture2D>();
            this._mobs = new List<List<Mob>>();
            this.rand = new Random();
            this._mobs.Add(new List<Mob>());
            this._mobs.Add(new List<Mob>());
            for (int index = 0; index < 25; ++index)
                this._mobs[0].Add(new Mob(this._windowSize, this.rand.Next(), new Vector2(1000f, 580f), 100, 200, "game/blitz", 5, 4, new Vector2(3f, 3f), new Vector2(1f, 0.5f), 300, 50, new Rectangle(900, 500, 2000, 80)));
            for (int index = 0; index < 40; ++index)
                this._mobs[1].Add(new Mob(this._windowSize, this.rand.Next(), new Vector2(1000f, 580f), 100, 200, "game/blitz", 5, 4, new Vector2(3f, 3f), new Vector2(1f, 0.5f), 300, 50, new Rectangle(900, 485, 2000, 100)));
            foreach (List<Mob> list in this._mobs)
            {
                foreach (Mob mob in list)
                {
                    mob.AddGraphicalBounds(CharacterActions.WalkRight, new Rectangle(6, 6, 10, 30));
                    mob.AddGraphicalBounds(CharacterActions.WalkLeft, new Rectangle(1, 1, 5, 30));
                    mob.AddGraphicalBounds(CharacterActions.StandLeft, new Rectangle(3, 3, 3, 30));
                    mob.AddGraphicalBounds(CharacterActions.StandRight, new Rectangle(8, 8, 8, 30));
                    mob.AddGraphicalBounds(CharacterActions.Attack1Left, new Rectangle(11, 11, 12, 4));
                    mob.AddGraphicalBounds(CharacterActions.Attack1Right, new Rectangle(16, 16, 17, 4));
                    mob.AddGraphicalBounds(CharacterActions.ReceiveAttackLeft, new Rectangle(13, 13, 13, 4));
                    mob.AddGraphicalBounds(CharacterActions.ReceiveAttackRight, new Rectangle(18, 18, 18, 4));
                }
            }
            this._hud = new HUD(this._windowSize);
            foreach (Sprite sprite in this.personnage.GetSkillsTips())
                this._hud.AddSprite(sprite);
            if (this.personnage.Weapons.Count <= 0)
                return;
            this._hud.AddWeapon(this.personnage.Weapons[0].Tip);

            if (!float.TryParse(EugLib.IO.FileStream.readFile("files/dash"), out DashSpeed))
            {
                DashSpeed = 4;
                EugLib.IO.FileStream.writeFile("files/dash", DashSpeed.ToString());
            }

            _players = new List<Personnage>();
            _server = null;
            _client = null;
        }

        public override void LoadContent(ContentManager content)
        {
            this.personnage.LoadContent(content);
            this.mouse.LoadContent(content, "general/cursor1");
            foreach (AbstractMap abstractScene in this._maps)
                abstractScene.LoadContent(content);
            this._menu.LoadContent(content);
            foreach (List<Mob> list in this._mobs)
            {
                foreach (AbstractScene abstractScene in list)
                    abstractScene.LoadContent(content);
            }
            this._hud.LoadContent(content);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            this._maps[this._currentMap].Draw(spriteBatch, false);
            bool flag1 = false;
            if (this._mobs != null && this._mobs[this._currentMap].Count > 0)
            {
                int index1 = 0;
                bool flag2 = false;
                List<int> list = new List<int>();
                while (list.Count < this._mobs[this._currentMap].Count + 1)
                {
                    float num = float.MaxValue;
                    for (int index2 = 0; index2 < this._mobs[this._currentMap].Count; ++index2)
                    {
                        if ((double)this._mobs[this._currentMap][index2].Position.Y < (double)num && !list.Contains(index2))
                        {
                            num = this._mobs[this._currentMap][index2].Position.Y;
                            index1 = index2;
                            if (!flag1 && (double)this.personnage.Position.Y < (double)this._mobs[this._currentMap][index2].Position.Y)
                                flag2 = true;
                            index2 = this._mobs[this._currentMap].Count + 1;
                        }
                    }
                    if (flag2)
                    {
                        this.personnage.Draw(spriteBatch);
                        flag2 = false;
                        flag1 = true;
                    }
                    else
                    {
                        this._mobs[this._currentMap][index1].Draw(spriteBatch);
                        list.Add(index1);
                    }
                }
            }
            if (!flag1)
                this.personnage.Draw(spriteBatch);
            this._maps[this._currentMap].Draw(spriteBatch, true);
            this._hud.Draw(spriteBatch);
            this._menu.Draw(spriteBatch);
            ((AbstractScene)this.mouse).Draw(spriteBatch);
            spriteBatch.End();
        }

        public override void Update(float elapsedTime)
        {
            _dashTimer += elapsedTime;
            if (_dashing && _dashTimer > DashDuration || _waitingDash && _dashTimer > DashKeyDelay)
            {
                if (!personnage._jumping)
                {
                    _dashing = false;
                    _waitingDash = false;
                }
            }
            this._maps[this._currentMap].Update(elapsedTime);
            this._menu.Update(elapsedTime);
            this.personnage.Update(elapsedTime);
            foreach (Mob mob in this._mobs[this._currentMap])
            {
                mob.Actualize(this.personnage.Position);
                mob.Update(elapsedTime);
            }
            this._hud.Update(elapsedTime);
            this._hud.LifeLevel = this.personnage.Life;
            this._hud.ManaLevel = this.personnage.Mana;
            this._hud.XpLevel = this.personnage.Experience.Percentage;
            this._hud.LevelText = this.personnage.Experience.Level.ToString();
            this._hud.EnnemiesText = this._mobs == null ? "--" : this._mobs[this._currentMap].Count.ToString();
            for (int index = 0; index < this._mobs[this._currentMap].Count; ++index)
            {
                if (this.personnage.Attacks.ContainsKey(this.personnage.Action) && this.personnage.Attacks[this.personnage.Action].Position.Intersects(this._mobs[this._currentMap][index].DrawingRectangle) && this.personnage.Attacks[this.personnage.Action].Active)
                    this._mobs[this._currentMap][index].ReceiveAttack(this.personnage.Attacks[this.personnage.Action].Damage * this.personnage.Weapons[this.personnage.Weapon].Damage, this.personnage.Attacks[this.personnage.Action].BlockTime);
                if ((double)this._mobs[this._currentMap][index].Life <= 0.0)
                {
                    int level = this.personnage.Experience.Level;
                    this._mobs[this._currentMap].RemoveAt(index);
                    --index;
                    this.personnage.Experience.Add(50);
                    if (level < this.personnage.Experience.Level && level == 9 && this.personnage.Weapons.Count >= 2)
                        this._hud.AddWeapon(this.personnage.Weapons[1].Tip);
                }
            }
            foreach (Character character in this._mobs[this._currentMap])
            {
                foreach (Attack attack in character.Attacks.Values)
                {
                    if (attack.Active && attack.Position.Intersects(this.personnage.DrawingRectangle))
                        this.personnage.ReceiveAttack(attack.Damage, attack.BlockTime);
                }
            }

            if (_dashing && _dashTimer < DashDuration && !personnage._jumping)
            {
                personnage.Mana -= elapsedTime / 5000f;
                if (personnage.Mana <= 0)
                    _waitingDash = _dashing = false;
            }
            if (!((double)this.personnage.Life >= 1.0 && (double)this.personnage.Mana >= 1.0))
            {
                foreach (AbstractMap.Element element in this._maps[this._currentMap].Elements)
                {
                    if (element.isHeal && element.sprite.Position.Intersects(this.personnage.DrawingRectangle))
                    {
                        this.personnage.Mana += 0.01f;
                        double num = (double)personnage.Life + 0.00999999977648258;
                        personnage.Life = (float)num;
                    }
                }
            }
        }

        public override void HandleInput(KeyboardState newKeyboardState, MouseState newMouseState, Game1 parent)
        {
            if (parent.Window.ClientBounds != this._windowSize)
                this.windowResized(parent.Window.ClientBounds);
            if (!newKeyboardState.IsKeyDown(Keys.Escape) && this._keyboardState.IsKeyDown(Keys.Escape))
                this._menu.Visible = !this._menu.Visible;
            bool flag = false;
            if (this._mouseState != newMouseState)
            {
                this.mouse.Position = new Rectangle(newMouseState.X, newMouseState.Y, this.mouse.Position.Width, this.mouse.Position.Height);
                flag = newMouseState.LeftButton == ButtonState.Pressed && this._mouseState.LeftButton == ButtonState.Released;
            }
            this._menu.HandleInput(newKeyboardState, newMouseState, parent);
            if (this._menu.Choise == 0)
                parent.SwitchScene(Scene.MainMenu);
            int num;
            if ((num = this._hud.SelectedWeapon(this._mouseState)) >= 0)
                this.personnage.Weapon = num;
            float move = (float)_windowSize.Width * 0.005555556f * (_dashing ? DashSpeed : 1f);
            if (newKeyboardState.IsKeyDown(Keys.Right) && this.personnage._canMove && this._maps[this._currentMap].Moving(new Vector2(move, 0.0f), true))
            {
                foreach (Mob mob in this._mobs[this._currentMap])
                    mob.Move((int)move, 0);
                foreach (Attack attack in this.personnage.Attacks.Values)
                    attack.Move((int)move, 0);
            }
            if (newKeyboardState.IsKeyDown(Keys.Left) && this.personnage._canMove && this._maps[this._currentMap].Moving(new Vector2(-move, 0.0f), true))
            {
                foreach (Mob mob in this._mobs[this._currentMap])
                    mob.Move(-(int)move, 0);
                foreach (Attack attack in this.personnage.Attacks.Values)
                    attack.Move(-(int)move, 0);
            }
            // DASH 
            if (newKeyboardState.IsKeyDown(Keys.Right) && _keyboardState.IsKeyUp(Keys.Right) || newKeyboardState.IsKeyDown(Keys.Left) && _keyboardState.IsKeyUp(Keys.Left))
            {
                if (_waitingDash && _dashTimer < DashKeyDelay)
                {
                    _dashing = true;
                }
                else if (!_waitingDash)
                {
                    _waitingDash = true;
                }
                else
                    _waitingDash = false;
                _dashTimer = 0;
            }
            if (newKeyboardState.IsKeyDown(Keys.Up) && this.personnage._canMove && this._maps[this._currentMap].Moving(new Vector2(0.0f, -move), true))
            {
                foreach (Mob mob in this._mobs[this._currentMap])
                    mob.Move(0, -(int)move);
                foreach (Attack attack in this.personnage.Attacks.Values)
                    attack.Move(0, -(int)(0.4f * move));
            }
            if (newKeyboardState.IsKeyDown(Keys.Down) && this.personnage._canMove && this._maps[this._currentMap].Moving(new Vector2(0.0f, move), true))
            {
                foreach (Mob mob in this._mobs[this._currentMap])
                    mob.Move(0, (int)move);
                foreach (Attack attack in this.personnage.Attacks.Values)
                    attack.Move(0, (int)(0.4f * move));
            }
            if (newKeyboardState.IsKeyDown(Keys.E) && this._keyboardState.IsKeyUp(Keys.E))
            {
                this._currentMap += this._maps[this._currentMap].GetTravelState(this.personnage.DrawingRectangle);
                if (this._currentMap < 0)
                    this._currentMap = 0;
                else if (this._currentMap >= this._maps.Count)
                    this._currentMap = this._maps.Count - 1;
            }
            this.personnage.HandleInput(newKeyboardState, newMouseState, parent);
            this._hud.HandleInput(newKeyboardState, newMouseState, parent);
            if ((double)this.personnage.Life <= 0.0)
                parent.SwitchScene(Scene.GameOver);
            this._keyboardState = newKeyboardState;
            this._mouseState = newMouseState;
        }

        public override void Activation(Game1 parent)
        {
            this._mouseState = Mouse.GetState();
            this._keyboardState = Keyboard.GetState();
            this._menu.Activation(parent);
            Load();
        }

        private void Load()
        {
            try
            {
                List<string> data = EugLib.IO.FileStream.readFileLines("files/save");
                // map life mana xp
                _currentMap = int.Parse(data[0]);
                personnage.Life = float.Parse(data[1]);
                personnage.Mana = float.Parse(data[2]);
                personnage.Experience.Add(int.Parse(data[3]));
            }
            catch (Exception)
            {
                Reset(true);
            }
        }

        public override void EndScene(Game1 parent)
        {
            this._windowSize = parent.Window.ClientBounds;
            foreach (AbstractMap abstractMap in this._maps)
            {
                abstractMap.Activation((Game1)null);
                abstractMap.WindowResized(this._windowSize);
            }
            this.rand = new Random();
            foreach (List<Mob> list in this._mobs)
                list.Clear();
            foreach (List<Mob> list in this._mobs)
            {
                for (int index = 0; index < 40; ++index)
                    list.Add(new Mob(this._windowSize, this.rand.Next(), new Vector2(1000f, 580f), 100, 200, "game/blitz", 5, 4, new Vector2(3f, 3f), new Vector2(1f, 0.5f), 300, 50, new Rectangle(900, 485, 2000, 100)));
            }
            foreach (List<Mob> list in this._mobs)
            {
                foreach (Mob mob in list)
                {
                    mob.AddGraphicalBounds(CharacterActions.WalkRight, new Rectangle(6, 6, 10, 30));
                    mob.AddGraphicalBounds(CharacterActions.WalkLeft, new Rectangle(1, 1, 5, 30));
                    mob.AddGraphicalBounds(CharacterActions.StandLeft, new Rectangle(3, 3, 3, 30));
                    mob.AddGraphicalBounds(CharacterActions.StandRight, new Rectangle(8, 8, 8, 30));
                    mob.AddGraphicalBounds(CharacterActions.Attack1Left, new Rectangle(11, 11, 12, 4));
                    mob.AddGraphicalBounds(CharacterActions.Attack1Right, new Rectangle(16, 16, 17, 4));
                    mob.AddGraphicalBounds(CharacterActions.ReceiveAttackLeft, new Rectangle(13, 13, 13, 4));
                    mob.AddGraphicalBounds(CharacterActions.ReceiveAttackRight, new Rectangle(18, 18, 18, 4));
                }
            }
            foreach (List<Mob> list in this._mobs)
            {
                foreach (Mob mob in list)
                {
                    mob.LoadContent(parent.Content);
                    mob.WindowResized(this._windowSize);
                }
            }
            Reset();
        }

        public void Reset(bool force = false)
        {
            if (force || personnage.Life <= 0)
            {
                this._currentMap = 0;
                this.personnage.Life = 1f;
                this.personnage.Mana = 1f;
                this.personnage.Experience.Reset();
                EugLib.IO.FileStream.writeFile("files/save", "");
            }
            else
            {
                EugLib.IO.FileStream.writeFile("files/save", _currentMap.ToString() + '\n' + personnage.Life.ToString() + '\n' + personnage.Mana.ToString() + '\n' + personnage.Experience.Experience.ToString());
            }
        }

        private void windowResized(Rectangle rect)
        {/*
            this.personnage.WindowResized(rect);
            foreach (AbstractScene abstractScene in this._maps)
                abstractScene.WindowResized(rect);
            this._menu.WindowResized(rect);
            foreach (List<Mob> list in this._mobs)
            {
                foreach (AbstractScene abstractScene in list)
                    abstractScene.WindowResized(rect);
            }
            this._hud.WindowResized(rect);
            this._windowSize = rect;*/
        }
    }
}
