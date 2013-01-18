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
    /// <summary>
    /// Classe de test temporaire
    /// qui contiendra le moteur de jeu
    /// ( La plus bg des classes les plus importantes )
    /// </summary>
    class InGame : AbstractScene
    {
        private KeyboardState _keyboardState;
        private MouseState _mouseState;
        private Rectangle _windowSize;
        private Sprite mouse;
        private AbstractMap map;
        private ContextMenu _menu;

        //========= TEMP ==========
        private Sprite personnage;

        public InGame(Rectangle windowSize, KeyboardState keyboardState, MouseState mouseState)
        {
            _windowSize = windowSize;
            _mouseState = mouseState;
            _keyboardState = keyboardState;

            _menu = new ContextMenu(_windowSize, new AnimatedSprite(new Rectangle(0, 30, 200, 50), _windowSize, "menu/ContextualMenuBlackFull"), 235);
            _menu.Title = new AnimatedSprite(new Rectangle(_menu.Position.Width / 2 - 75, -25, 150, 50), _windowSize, "menu/contextMenuText");
            _menu.Visible = false;
            _menu.Add(new AnimatedSprite(new Rectangle(_menu.Position.Width / 2 - 100, 40, 200, 22), _windowSize, "menu/contextMenuTextMainMenu"));
            _menu.CuadricPositionning(new Rectangle(0, 0, 150, 20), 40, 15, 10, 10, true);

            mouse = new Sprite(new Rectangle(-100, -100, 30, 50), _windowSize);
            map = new AbstractMap(_windowSize);
            map.Visitable.Add(new Rectangle(450, 330, 1800, 150));
            map.VuePosition = new Vector2(map.Visitable.Last<Rectangle>().X + 1, map.Visitable.Last<Rectangle>().Y + map.Visitable.Last<Rectangle>().Height - 1);
            map.Elements.Add(new AbstractMap.Element(new AnimatedSprite(new Rectangle(0, 0, _windowSize.Width, _windowSize.Height), _windowSize, "map1/sky1"), 0.2f, 0, true));
            map.Elements.Add(new AbstractMap.Element(new AnimatedSprite(new Rectangle(0, 0, _windowSize.Width, _windowSize.Height), _windowSize, "map1/back1r"), 0.8f, 0.2f, true));
            map.Elements.Add(new AbstractMap.Element(new AnimatedSprite(new Rectangle(0, _windowSize.Height / 2, _windowSize.Width, _windowSize.Height / 2), _windowSize, "map1/fore1"), 1f, 0.5f, true));
            map.Elements.Add(new AbstractMap.Element(new AnimatedSprite(new Rectangle(5, 150, _windowSize.Width / 2, _windowSize.Height - 150), _windowSize, "sprites/fireWall_11x6r23r44", 11, 6, 30, 23, 44, 1, true), 1f, 0.5f));
            map.Elements.Add(new AbstractMap.Element(new AnimatedSprite(new Rectangle(1800 + _windowSize.Width / 2, 150, _windowSize.Width / 2 - 20, _windowSize.Height - 150), _windowSize, "sprites/fireWall_11x6r23r44", 11, 6, 30, 23, 44, 1, true), 1f, 0.5f));

            personnage = new Sprite(new Rectangle((int)map.VuePosition.X - 50, (int)map.VuePosition.Y - 50, 100, 100), _windowSize, "game/bear");
        }

        public override void LoadContent(ContentManager content)
        {
            personnage.LoadContent(content);
            mouse.LoadContent(content, "general/cursor1");
            map.LoadContent(content);
            _menu.LoadContent(content);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            map.Draw(spriteBatch);
            personnage.Draw(spriteBatch);
            _menu.Draw(spriteBatch);
            mouse.Draw(spriteBatch);

            spriteBatch.End();
        }

        public override void Update(float elapsedTime)
        {
            map.Update(elapsedTime);
            _menu.Update(elapsedTime);
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

            if (newKeyboardState.IsKeyDown(Keys.Right))
                map.Moving(new Vector2(5, 0), true);
            if (newKeyboardState.IsKeyDown(Keys.Left))
                map.Moving(new Vector2(-5, 0), true);
            if (newKeyboardState.IsKeyDown(Keys.Up))
                map.Moving(new Vector2(0, -5), true);
            if (newKeyboardState.IsKeyDown(Keys.Down))
                map.Moving(new Vector2(0, 5), true);

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
            personnage.windowResized(rect);
            map.WindowResized(rect);
            _menu.WindowResized(rect);
        }
    }
}