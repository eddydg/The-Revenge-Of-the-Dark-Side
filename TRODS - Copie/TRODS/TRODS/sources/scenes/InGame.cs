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

        private Character personnage;
        private static int _speedPerso = 7;


        public InGame(Rectangle windowSize, KeyboardState keyboardState, MouseState mouseState)
        {
            _windowSize = windowSize;
            _mouseState = mouseState;
            _keyboardState = keyboardState;

            mouse = new Sprite(new Rectangle(-100, -100, 30, 50), _windowSize);
            map = new AbstractMap(_windowSize);
            map.Visitable.Add(new Rectangle(450, 330, 1800, 150));
            map.VuePosition = new Vector2(450,330+150-1);
            map.Elements.Add(new AbstractMap.Element(new AnimatedSprite(new Rectangle(0, 0, _windowSize.Width, _windowSize.Height), _windowSize, "map1/sky1"), 0.2f, 0, true));
            map.Elements.Add(new AbstractMap.Element(new AnimatedSprite(new Rectangle(0, 0, _windowSize.Width, _windowSize.Height), _windowSize, "map1/back1r"), 0.8f, 0.2f, true));
            map.Elements.Add(new AbstractMap.Element(new AnimatedSprite(new Rectangle(0, _windowSize.Height / 2, _windowSize.Width, _windowSize.Height / 2), _windowSize, "map1/fore1"), 1f, 0.5f, true));
            map.Elements.Add(new AbstractMap.Element(new AnimatedSprite(new Rectangle(5, 150, _windowSize.Width / 2, _windowSize.Height - 150), _windowSize, "sprites/fireWall_11x6r23r44", 11, 6, 30, 23, 44, 1, true), 1f, 0.5f));
            map.Elements.Add(new AbstractMap.Element(new AnimatedSprite(new Rectangle(1800 + _windowSize.Width / 2, 150, _windowSize.Width / 2 - 20, _windowSize.Height - 150), _windowSize, "sprites/fireWall_11x6r23r44", 11, 6, 30, 23, 44, 1, true), 1f, 0.5f));

            personnage = new Character(new AnimatedSprite(new Rectangle(_windowSize.Width / 2 - 130/2, 370, 130, 170), _windowSize, "game/mainCharacter", 5, 7, 30, 1, -1, -1, true),
                new Vector3(1, 2, -1), new Vector3(16, 17, -1), new Vector3(6, 15, 3), new Vector3(21, 30, 18), new Vector3(31, 35, -1));

        }

        public override void LoadContent(ContentManager content)
        {
            personnage.LoadContent(content);
     
            mouse.LoadContent(content, "general/cursor1");
            map.LoadContent(content);

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            map.Draw(spriteBatch);
            personnage.Draw(spriteBatch);
            mouse.Draw(spriteBatch);
            spriteBatch.End();
        }

        public override void Update(float elapsedTime)
        {
            map.Update(elapsedTime);
            personnage.Update(elapsedTime);
        }

        public override void HandleInput(KeyboardState newKeyboardState, MouseState newMouseState, Game1 parent)
        {
            if (parent.Window.ClientBounds != _windowSize)
            {
                _windowSize = parent.Window.ClientBounds;
                windowResized(parent.Window.ClientBounds);
            }
            if (!newKeyboardState.IsKeyDown(Keys.Escape) && _keyboardState.IsKeyDown(Keys.Escape))
                parent.SwitchScene(Scene.MainMenu);
            bool isClick = false;
            if (_mouseState != newMouseState)
            {
                mouse.Position = new Rectangle(newMouseState.X, newMouseState.Y, mouse.Position.Width, mouse.Position.Height);
                isClick = newMouseState.LeftButton == ButtonState.Pressed && _mouseState.LeftButton == ButtonState.Released;
            }

            if (newKeyboardState.IsKeyDown(Keys.Right))
                map.Moving(new Vector2(_speedPerso, 0), true);
            if (newKeyboardState.IsKeyDown(Keys.Left))
                map.Moving(new Vector2(-_speedPerso, 0), true);
            if (newKeyboardState.IsKeyDown(Keys.Up))
                map.Moving(new Vector2(0, -_speedPerso), true);
            if (newKeyboardState.IsKeyDown(Keys.Down))
                map.Moving(new Vector2(0, _speedPerso), true);

            //perso
            if (_keyboardState != newKeyboardState)
            {
                if (newKeyboardState.IsKeyDown(Keys.Space))
                    personnage.JumpR(18);
                if (newKeyboardState.IsKeyDown(Keys.Right) && !newKeyboardState.IsKeyDown(Keys.Space))
                    personnage.RunR(18);
                if (newKeyboardState.IsKeyDown(Keys.Left) && !newKeyboardState.IsKeyDown(Keys.Space))
                    personnage.RunL(18);
                if (!newKeyboardState.IsKeyDown(Keys.Space) && !newKeyboardState.IsKeyDown(Keys.Left) && !newKeyboardState.IsKeyDown(Keys.Right))
                    personnage.Stop(3);
            }

            _keyboardState = newKeyboardState;
            _mouseState = newMouseState;
        }

        public override void Activation(Game1 parent)
        {
            _mouseState = Mouse.GetState();
            _keyboardState = Keyboard.GetState();
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
            personnage._character.windowResized(rect);
            map.WindowResized(rect);
        }
    }
}