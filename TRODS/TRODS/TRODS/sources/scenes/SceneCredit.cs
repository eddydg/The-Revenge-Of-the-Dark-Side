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
    class SceneCredit : AbstractScene
    {
        private KeyboardState _keyboardState;
        private Rectangle _windowSize;
        private List<AnimatedSprite> animations;
        private Sprite authors;

        public SceneCredit(Rectangle windowSize)
        {
            _windowSize = windowSize;

            authors = new Sprite(new Rectangle(300, 200, 400, 200), _windowSize, "menu/authors");

            animations = new List<AnimatedSprite>();
            animations.Add(new AnimatedSprite(new Rectangle(0, 0, _windowSize.Width, _windowSize.Height), _windowSize, "menu/etoiles1_10x10r51r100", 10, 10, 17, 51, 100, 1));
            animations.Add(new AnimatedSprite(new Rectangle(0, 0, _windowSize.Width, 2 * _windowSize.Height / 5), _windowSize, "menu/credit"));
            animations.Add(new AnimatedSprite(new Rectangle(-300, _windowSize.Height - 100, _windowSize.Width + 300, 100), _windowSize, "menu/lueur1_10x4r21r40", 10, 4, 15, 21, 40, 1));
        }

        public override void LoadContent(ContentManager content)
        {
            authors.LoadContent(content);
            foreach (AnimatedSprite s in animations)
                s.LoadContent(content);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            foreach (AnimatedSprite s in animations)
                s.Draw(spriteBatch);

            Random rand = new Random();
            authors.Draw(spriteBatch, Color.Red,
                authors.Position.X + rand.Next(-authors.Position.Width / 100, authors.Position.Width / 100),
                authors.Position.Y + rand.Next(-authors.Position.Width / 100, authors.Position.Width / 100));
            authors.Draw(spriteBatch, Color.Black);

            spriteBatch.End();
        }

        public override void Update(float elapsedTime)
        {
            foreach (AnimatedSprite s in animations)
                s.Update(elapsedTime);
        }

        public override void HandleInput(KeyboardState newKeyboardState, MouseState newMouseState, Game1 parent)
        {
            if (parent.Window.ClientBounds != _windowSize)
            {
                _windowSize = parent.Window.ClientBounds;
                windowResized(_windowSize);
            }
            if (newKeyboardState.IsKeyDown(Keys.Escape) && !_keyboardState.IsKeyDown(Keys.Escape))
                parent.SwitchScene(Scene.MainMenu);

            _keyboardState = newKeyboardState;
        }

        public override void Activation()
        {
            foreach (AnimatedSprite s in animations)
                s.ActualPicture = 1;
        }

        public override void EndScene()
        {
        }

        /// <summary>
        /// Fonction adaptant les textures au
        /// redimensionnement de la fenetre
        /// </summary>
        /// <param name="rect">Nouvelle dimension de la fenetre obtenue par *Game1*.Window.ClientBounds()</param>
        private void windowResized(Rectangle rect)
        {
            foreach (AnimatedSprite s in animations)
                s.windowResized(rect);
            authors.windowResized(rect);
        }
    }
}
