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
        private List<Mob> _mobs;
        private HUD _hud;
        private Rectangle _originalWindowSize;

        public InGame(Rectangle windowSize, KeyboardState keyboardState, MouseState mouseState)
        {
            _windowSize = windowSize;
            _mouseState = mouseState;
            _keyboardState = keyboardState;
            _originalWindowSize = windowSize;

            _menu = new ContextMenu(_windowSize, new AnimatedSprite(new Rectangle(0, 30, 200, 50), _windowSize, "menu/ContextualMenuBlackFull"), "menu/contextMenuExit", 235);
            _menu.Title = new AnimatedSprite(new Rectangle(_menu.Position.Width / 2 - 75, 0, 150, 50), _windowSize, "menu/contextMenuText");
            _menu.Visible = false;
            _menu.Add(new AnimatedSprite(new Rectangle(_menu.Position.Width / 2 - 100, 65, 200, 22), _windowSize, "menu/contextMenuTextMainMenu"));
            _menu.CuadricPositionning(new Rectangle(0, 0, 150, 20), 65, 15, 10, 10, true);

            mouse = new Sprite(new Rectangle(-100, -100, 30, 50), _windowSize);

            _maps = new List<AbstractMap>();
            AbstractMap map = new AbstractMap(_windowSize);
            map.AddVisitable(450, 450, 2800, 130);
            map.VuePosition = new Vector2(460, 450 + 130 - 1);
            /*map.Elements.Add(new AbstractMap.Element(new AnimatedSprite(new Rectangle(0, 0, _windowSize.Width, _windowSize.Height), _windowSize, "map1/sky1"), 0.2f, 0, true));
            map.Elements.Add(new AbstractMap.Element(new AnimatedSprite(new Rectangle(0, 0, _windowSize.Width, _windowSize.Height), _windowSize, "map1/back1r"), 0.8f, 0.2f, true));
            map.Elements.Add(new AbstractMap.Element(new AnimatedSprite(new Rectangle(0, _windowSize.Height / 2, _windowSize.Width, _windowSize.Height / 2), _windowSize, "map1/fore1"), 1f, 0.5f, true));*/
            map.Elements.Add(new AbstractMap.Element(new AnimatedSprite(new Rectangle(0, 0, 1040, 320), windowSize, "map2/sky"), 0.2f, 0, true));
            map.Elements.Add(new AbstractMap.Element(new AnimatedSprite(new Rectangle(0, 190, 960, 300), windowSize, "map2/mountain"), 0.8f, 0.2f, true));
            map.Elements.Add(new AbstractMap.Element(new AnimatedSprite(new Rectangle(0, 415, 1082, 193), windowSize, "map2/sand"), 1f, 0.5f, true));
            map.Elements.Add(new AbstractMap.Element(new AnimatedSprite(new Rectangle(0, 515, 1066, 92), windowSize, "map2/rock"), 1f, 0.5f, true, true));
            map.Elements.Add(new AbstractMap.Element(new AnimatedSprite(new Rectangle(5, 150, _windowSize.Width / 2, _windowSize.Height - 150), _windowSize, "sprites/fireWall_11x6r23r44", 11, 6, 30, 23, 44, 1, true), 1f, 0.5f));
            map.Elements.Add(new AbstractMap.Element(new AnimatedSprite(new Rectangle(2550 + _windowSize.Width / 2, 150, 400, 400), _windowSize, "sprites/portal_6x6", 6, 6, 30, 1, 32, 1, true), 1f, 0.5f));
            _maps.Add(map);
            _currentMap = 0;

            personnage = new Personnage(_windowSize, map.VuePosition);
            _mobs = new List<Mob>();
            rand = new Random();
            for (int i = 0; i < 50; i++)
                _mobs.Add(new Mob(_windowSize, rand.Next(), new Vector2(1000, 580), 100, 200, "game/blitz", 5, 4, new Vector2(3f, 3f), new Vector2(1f, 0.5f), 300, 50, new Rectangle(900, 485, 2000, 100)));
            foreach (Mob m in _mobs)
            {
                m.AddGraphicalBounds(CharacterActions.WalkRight, new Rectangle(6, 6, 10, 30));
                m.AddGraphicalBounds(CharacterActions.WalkLeft, new Rectangle(1, 1, 5, 30));
                m.AddGraphicalBounds(CharacterActions.StandLeft, new Rectangle(3, 3, 3, 30));
                m.AddGraphicalBounds(CharacterActions.StandRight, new Rectangle(8, 8, 8, 30));
                m.AddGraphicalBounds(CharacterActions.Attack1Left, new Rectangle(11, 11, 12, 4));
                m.AddGraphicalBounds(CharacterActions.Attack1Right, new Rectangle(16, 16, 17, 4));
                m.AddGraphicalBounds(CharacterActions.ReceiveAttackLeft, new Rectangle(13, 13, 13, 4));
                m.AddGraphicalBounds(CharacterActions.ReceiveAttackRight, new Rectangle(18, 18, 18, 4));
            }

            _hud = new HUD(_windowSize);
        }

        public override void LoadContent(ContentManager content)
        {
            personnage.LoadContent(content);
            mouse.LoadContent(content, "general/cursor1");
            foreach (AbstractMap map in _maps)
                map.LoadContent(content);
            _menu.LoadContent(content);
            foreach (Mob m in _mobs)
                m.LoadContent(content);
            _hud.LoadContent(content);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            _maps[_currentMap].Draw(spriteBatch, false);
            bool pdrn = false;
            // gestion profondeur des biatches
            if (_mobs != null && _mobs.Count > 0)
            {
                int l = 0;
                float min = float.MaxValue;
                bool dp = false;
                List<int> done = new List<int>();
                while (done.Count < _mobs.Count + 1)
                {
                    min = float.MaxValue;
                    for (int i = 0; i < _mobs.Count; i++)
                    {
                        if (_mobs[i].Position.Y < min && !done.Contains(i))
                        {
                            min = _mobs[i].Position.Y;
                            l = i;
                            if (!pdrn && personnage.Position.Y < _mobs[i].Position.Y)
                                dp = true;
                            i = _mobs.Count + 1;
                        }
                    }
                    if (dp)
                    {
                        personnage.Draw(spriteBatch);
                        dp = false;
                        pdrn = true;
                    }
                    else
                    {
                        _mobs[l].Draw(spriteBatch);
                        done.Add(l);
                    }
                }
            }
            if (!pdrn)
                personnage.Draw(spriteBatch);
            _maps[_currentMap].Draw(spriteBatch, true);
            _hud.Draw(spriteBatch);
            _menu.Draw(spriteBatch);
            mouse.Draw(spriteBatch);

            spriteBatch.End();
        }

        public override void Update(float elapsedTime)
        {
            _maps[_currentMap].Update(elapsedTime);
            _menu.Update(elapsedTime);
            personnage.Update(elapsedTime);
            foreach (Mob m in _mobs)
            {
                m.Actualize(personnage.Position);
                m.Update(elapsedTime);
            }
            _hud.Update(elapsedTime);
            _hud.LifeLevel = personnage.Life;
            _hud.ManaLevel = personnage.Mana;
            _hud.XpLevel = personnage.Experience.Percentage;
            _hud.LevelText = personnage.Experience.Level.ToString();
            if (_mobs != null)
                _hud.EnnemiesText = _mobs.Count.ToString();
            else
                _hud.EnnemiesText = "--";

            if (personnage.Action == CharacterActions.Attack1Left || personnage.Action == CharacterActions.Attack1Right)
            {
                for (int i = 0; i < _mobs.Count; i++)
                {
                    if (personnage.Weapon.Position.Intersects(_mobs[i].DrawingRectangle))
                    {
                        _mobs[i].ReceiveAttack(0.01f);
                    }
                    if (_mobs[i].Life <= 0)
                    {
                        _mobs.RemoveAt(i);
                        i--;
                        personnage.Experience.Add(50);
                    }
                }
            }

            foreach (Mob m in _mobs)
            {
                if (m.Ia._attack && m.DrawingRectangle.Intersects(personnage.DrawingRectangle))
                {
                    personnage.ReceiveAttack(0.01f);
                }
            }
        }

        public override void HandleInput(KeyboardState newKeyboardState, MouseState newMouseState, Game1 parent)
        {
            if (parent.Window.ClientBounds != _windowSize)
            {
                windowResized(parent.Window.ClientBounds);
            }
            if (!newKeyboardState.IsKeyDown(Keys.Escape) && _keyboardState.IsKeyDown(Keys.Escape))
                _menu.Visible = !_menu.Visible;
            bool isClick = false;
            if (_mouseState != newMouseState)
            {
                mouse.Position = new Rectangle(newMouseState.X, newMouseState.Y, mouse.Position.Width, mouse.Position.Height);
                isClick = newMouseState.LeftButton == ButtonState.Pressed && _mouseState.LeftButton == ButtonState.Released;
            }

            _menu.HandleInput(newKeyboardState, newMouseState, parent);
            if (_menu.Choise == 0)
                parent.SwitchScene(Scene.MainMenu);

            //// MOUVEMENT ////
            if (newKeyboardState.IsKeyDown(Keys.Right) && personnage._canMove)
            {
                if (_maps[_currentMap].Moving(new Vector2(5, 0), true))
                    foreach (Mob m in _mobs)
                        m.Move(5, 0);
            }
            if (newKeyboardState.IsKeyDown(Keys.Left) && personnage._canMove)
            {
                if (_maps[_currentMap].Moving(new Vector2(-5, 0), true))
                    foreach (Mob m in _mobs)
                        m.Move(-5, 0);
            }
            if (newKeyboardState.IsKeyDown(Keys.Up) && personnage._canMove)
            {
                if (_maps[_currentMap].Moving(new Vector2(0, -5), true))
                    foreach (Mob m in _mobs)
                        m.Move(0, -5);
            }
            if (newKeyboardState.IsKeyDown(Keys.Down) && personnage._canMove)
            {
                if (_maps[_currentMap].Moving(new Vector2(0, 5), true))
                    foreach (Mob m in _mobs)
                        m.Move(0, 5);
            }
            personnage.HandleInput(newKeyboardState, newMouseState, parent);
            _hud.HandleInput(newKeyboardState, newMouseState, parent);

            if (personnage.Life <= 0)
                parent.SwitchScene(Scene.MainMenu);

            _keyboardState = newKeyboardState;
            _mouseState = newMouseState;
        }

        public override void Activation(Game1 parent)
        {
            _mouseState = Mouse.GetState();
            _keyboardState = Keyboard.GetState();
            _menu.Activation(parent);
        }

        public override void EndScene(Game1 parent)
        {
            _windowSize = parent.Window.ClientBounds;
            /*map.VuePosition = new Vector2(460, 450 + 130 - 1);
            map.Elements[0].sprite.setRelatvePos(new Rectangle(0, 0, 1040, 320), _originalWindowSize.Width, _originalWindowSize.Height);
            map.Elements[1].sprite.setRelatvePos(new Rectangle(0, 190, 960, 300), _originalWindowSize.Width, _originalWindowSize.Height);
            map.Elements[2].sprite.setRelatvePos(new Rectangle(0, 415, 1082, 193), _originalWindowSize.Width, _originalWindowSize.Height);
            map.Elements[3].sprite.setRelatvePos(new Rectangle(0, 515, 1066, 92), _originalWindowSize.Width, _originalWindowSize.Height);
            map.Elements[4].sprite.setRelatvePos(new Rectangle(5, 150, _windowSize.Width / 2, _windowSize.Height - 150), _originalWindowSize.Width, _originalWindowSize.Height);
            map.Elements[5].sprite.setRelatvePos(new Rectangle(2800 + _windowSize.Width / 2, 150, _windowSize.Width / 2 - 20, _windowSize.Height - 150), _originalWindowSize.Width, _originalWindowSize.Height);
            map.WindowResized(_windowSize);*/
            foreach (AbstractMap m in _maps)
            {
                m.Activation();
                m.WindowResized(_windowSize);
            }
            _currentMap = 0;

            rand = new Random();
            _mobs.Clear();
            for (int i = 0; i < 50; i++)
                _mobs.Add(new Mob(_windowSize, rand.Next(), new Vector2(1000, 580), 100, 200, "game/blitz", 5, 4, new Vector2(3f, 3f), new Vector2(1f, 0.5f), 300, 50, new Rectangle(900, 485, 2000, 100)));
            foreach (Mob m in _mobs)
            {
                m.AddGraphicalBounds(CharacterActions.WalkRight, new Rectangle(6, 6, 10, 30));
                m.AddGraphicalBounds(CharacterActions.WalkLeft, new Rectangle(1, 1, 5, 30));
                m.AddGraphicalBounds(CharacterActions.StandLeft, new Rectangle(3, 3, 3, 30));
                m.AddGraphicalBounds(CharacterActions.StandRight, new Rectangle(8, 8, 8, 30));
                m.AddGraphicalBounds(CharacterActions.Attack1Left, new Rectangle(11, 11, 12, 4));
                m.AddGraphicalBounds(CharacterActions.Attack1Right, new Rectangle(16, 16, 17, 4));
                m.AddGraphicalBounds(CharacterActions.ReceiveAttackLeft, new Rectangle(13, 13, 13, 4));
                m.AddGraphicalBounds(CharacterActions.ReceiveAttackRight, new Rectangle(18, 18, 18, 4));
            }
            foreach (Mob m in _mobs)
            {
                m.LoadContent(parent.Content);
                m.WindowResized(_windowSize);
            }

            personnage.Life = 1;
            personnage.Mana = 1;
            personnage.Experience.Reset();
        }

        /// <summary>
        /// Fonction adaptant les textures au
        /// redimensionnement de la fenetre
        /// </summary>
        /// <param name="rect">Nouvelle dimension de la fenetre obtenue par *Game1*.Window.ClientBounds()</param>
        private void windowResized(Rectangle rect)
        {
            personnage.WindowResized(rect);
            foreach (AbstractMap map in _maps)
                map.WindowResized(rect);
            _menu.WindowResized(rect);
            foreach (Mob m in _mobs)
                m.WindowResized(rect);
            _windowSize = rect;
            _hud.WindowResized(rect);
        }
    }
}