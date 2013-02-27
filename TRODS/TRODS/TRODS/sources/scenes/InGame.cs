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
        private AbstractMap map;
        private ContextMenu _menu;
        private Personnage personnage;
        private List<Mob> _mobs;

        public InGame(Rectangle windowSize, KeyboardState keyboardState, MouseState mouseState)
        {
            _windowSize = windowSize;
            _mouseState = mouseState;
            _keyboardState = keyboardState;

            _menu = new ContextMenu(_windowSize, new AnimatedSprite(new Rectangle(0, 30, 200, 50), _windowSize, "menu/ContextualMenuBlackFull"), "menu/contextMenuExit", 235);
            _menu.Title = new AnimatedSprite(new Rectangle(_menu.Position.Width / 2 - 75, 0, 150, 50), _windowSize, "menu/contextMenuText");
            _menu.Visible = false;
            _menu.Add(new AnimatedSprite(new Rectangle(_menu.Position.Width / 2 - 100, 65, 200, 22), _windowSize, "menu/contextMenuTextMainMenu"));
            _menu.CuadricPositionning(new Rectangle(0, 0, 150, 20), 65, 15, 10, 10, true);

            mouse = new Sprite(new Rectangle(-100, -100, 30, 50), _windowSize);
            map = new AbstractMap(_windowSize);
            map.Visitable.Add(new Rectangle(450, 450, 2800, 130));
            map.VuePosition = new Vector2(460, 450 + 130 - 1);
            /*map.Elements.Add(new AbstractMap.Element(new AnimatedSprite(new Rectangle(0, 0, _windowSize.Width, _windowSize.Height), _windowSize, "map1/sky1"), 0.2f, 0, true));
            map.Elements.Add(new AbstractMap.Element(new AnimatedSprite(new Rectangle(0, 0, _windowSize.Width, _windowSize.Height), _windowSize, "map1/back1r"), 0.8f, 0.2f, true));
            map.Elements.Add(new AbstractMap.Element(new AnimatedSprite(new Rectangle(0, _windowSize.Height / 2, _windowSize.Width, _windowSize.Height / 2), _windowSize, "map1/fore1"), 1f, 0.5f, true));*/
            map.Elements.Add(new AbstractMap.Element(new AnimatedSprite(new Rectangle(0, 0, 1040, 320), windowSize, "map2/sky"), 0.2f, 0, true));
            map.Elements.Add(new AbstractMap.Element(new AnimatedSprite(new Rectangle(0, 190, 960, 300), windowSize, "map2/mountain"), 0.8f, 0.2f, true));
            map.Elements.Add(new AbstractMap.Element(new AnimatedSprite(new Rectangle(0, 415, 1082, 193), windowSize, "map2/sand"), 1f, 0.5f, true));
            map.Elements.Add(new AbstractMap.Element(new AnimatedSprite(new Rectangle(0, 515, 1066, 92), windowSize, "map2/rock"), 1f, 0.5f, true, true));
            map.Elements.Add(new AbstractMap.Element(new AnimatedSprite(new Rectangle(5, 150, _windowSize.Width / 2, _windowSize.Height - 150), _windowSize, "sprites/fireWall_11x6r23r44", 11, 6, 30, 23, 44, 1, true), 1f, 0.5f));
            map.Elements.Add(new AbstractMap.Element(new AnimatedSprite(new Rectangle(2800 + _windowSize.Width / 2, 150, _windowSize.Width / 2 - 20, _windowSize.Height - 150), _windowSize, "sprites/fireWall_11x6r23r44", 11, 6, 30, 23, 44, 1, true), 1f, 0.5f));

            personnage = new Personnage(_windowSize, map.VuePosition);
            _mobs = new List<Mob>();
            Random rand = new Random();
            for (int i = 0; i < 5; i++)
                _mobs.Add(new Mob(_windowSize, rand.Next(), new Vector2(1000, 580), 100, 200, "game/blitz", 5, 2, new Vector2(3f, 3f), new Vector2(1f, 0.5f), 300, 50, new Rectangle(900, 485, 500, 100)));
            for (int i = 0; i < 5; i++)
                _mobs.Add(new Mob(_windowSize, rand.Next(), new Vector2(2000, 580), 100, 200, "game/blitz", 5, 2, new Vector2(3f, 3f), new Vector2(1f, 0.5f), 300, 50, new Rectangle(2800, 485, 450, 100)));
            foreach (Mob m in _mobs)
            {
                m.AddGraphicalBounds(CharacterActions.WalkRight, new Rectangle(6, 6, 10, 30));
                m.AddGraphicalBounds(CharacterActions.WalkLeft, new Rectangle(1, 1, 5, 30));
                m.AddGraphicalBounds(CharacterActions.StandLeft, new Rectangle(3, 3, 3, 30));
                m.AddGraphicalBounds(CharacterActions.StandRight, new Rectangle(8, 8, 8, 30));
            }
        }

        public override void LoadContent(ContentManager content)
        {
            personnage.LoadContent(content);
            mouse.LoadContent(content, "general/cursor1");
            map.LoadContent(content);
            _menu.LoadContent(content);
            foreach (Mob m in _mobs)
                m.LoadContent(content);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            map.Draw(spriteBatch, false);
            foreach (Mob m in _mobs)
                m.Draw(spriteBatch);
            personnage.Draw(spriteBatch);
            map.Draw(spriteBatch, true);
            _menu.Draw(spriteBatch);
            mouse.Draw(spriteBatch);

            spriteBatch.End();
        }

        public override void Update(float elapsedTime)
        {
            map.Update(elapsedTime);
            _menu.Update(elapsedTime);
            personnage.Update(elapsedTime);
            foreach (Mob m in _mobs)
            {
                m.Actualize(personnage.Position);
                m.Update(elapsedTime);
            }
        }

        public override void HandleInput(KeyboardState newKeyboardState, MouseState newMouseState, Game1 parent)
        {
            if (parent.Window.ClientBounds != _windowSize)
            {
                _windowSize = parent.Window.ClientBounds;
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
                if (map.Moving(new Vector2(5, 0), true))
                    foreach (Mob m in _mobs)
                        m.Move(5, 0);
            }
            if (newKeyboardState.IsKeyDown(Keys.Left) && personnage._canMove)
            {
                if (map.Moving(new Vector2(-5, 0), true))
                    foreach (Mob m in _mobs)
                        m.Move(-5, 0);
            }
            if (newKeyboardState.IsKeyDown(Keys.Up) && personnage._canMove)
            {
                if (map.Moving(new Vector2(0, -5), true))
                    foreach (Mob m in _mobs)
                        m.Move(0, -5);
            }
            if (newKeyboardState.IsKeyDown(Keys.Down) && personnage._canMove)
            {
                if (map.Moving(new Vector2(0, 5), true))
                    foreach (Mob m in _mobs)
                        m.Move(0, 5);
            }
            personnage.HandleInput(newKeyboardState, newMouseState, parent);

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
        }

        /// <summary>
        /// Fonction adaptant les textures au
        /// redimensionnement de la fenetre
        /// </summary>
        /// <param name="rect">Nouvelle dimension de la fenetre obtenue par *Game1*.Window.ClientBounds()</param>
        private void windowResized(Rectangle rect)
        {
            personnage.WindowResized(rect);
            map.WindowResized(rect);
            _menu.WindowResized(rect);
            foreach (Mob m in _mobs)
                m.WindowResized(rect);
        }
    }
}