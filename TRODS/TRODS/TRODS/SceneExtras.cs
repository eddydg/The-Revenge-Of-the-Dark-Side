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
    class SceneExtras : AbstractScene
    {
        private KeyboardState _keyboardState;
        private MouseState _mouseState;
        private Rectangle _windowSize;

        private int selectedSprite;
        private int currentSize;
        private List<AnimatedSprite> animations;
        private List<AnimatedSprite> textures;
        private Sprite tailleSelection;
        private AnimatedSprite mouse;

        public SceneExtras(Rectangle windowSize)
        {
            _windowSize = windowSize;
            selectedSprite = 0;
            currentSize = 150;
            tailleSelection = new Sprite(new Rectangle(0, 400, windowSize.Width, 85), windowSize);
            mouse = new AnimatedSprite(new Rectangle(-100, -100, 60, 80), _windowSize, 8,4, 30);

            animations = new List<AnimatedSprite>();
            textures = new List<AnimatedSprite>();
        }

        public override void LoadContent(ContentManager content)
        {
            mouse.LoadContent(content, "menu/cursor0_8x4r");
            tailleSelection.LoadContent(content, "menu/sizeSelection");
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            tailleSelection.Draw(spriteBatch);
            foreach (Sprite s in textures)
                s.Draw(spriteBatch);
            foreach (AnimatedSprite p in animations)
                p.Draw(spriteBatch);
            mouse.Draw(spriteBatch);
            spriteBatch.End();
        }

        public override void Update(float elapsedTime)
        {
            for (int i = 0; i < animations.Count; i++)
            {
                animations.ElementAt<AnimatedSprite>(i).Update(elapsedTime);
                if (animations.ElementAt<AnimatedSprite>(i).IsEnd())
                {
                    animations.RemoveAt(i);
                    i--;
                }
            }
            mouse.Update(elapsedTime);
        }

        public override void HandleInput(KeyboardState newKeyboardState, MouseState newMouseState, Game1 parent)
        {
            if (parent.Window.ClientBounds != _windowSize)
            {
                _windowSize = parent.Window.ClientBounds;
                windowResized(_windowSize);
            }
            if (!newKeyboardState.IsKeyDown(Keys.Escape) && _keyboardState.IsKeyDown(Keys.Escape))
                parent.SwitchScene(Scene.MainMenu);
            if (_mouseState != newMouseState)
                mouse.Position = new Rectangle(newMouseState.X, newMouseState.Y, mouse.Position.Width, mouse.Position.Height);

            bool isClick = newMouseState.LeftButton == ButtonState.Pressed && _mouseState.LeftButton != ButtonState.Pressed;

            if (isClick)
            {
                Rectangle clic = new Rectangle(mouse.Position.X, mouse.Position.Y, 1, 1);
            }

            _keyboardState = newKeyboardState;
            _mouseState = newMouseState;
        }

        public override void Activation()
        {
            _mouseState = Mouse.GetState();
        }

        public override void EndScene()
        {
            animations.Clear();
        }

        /// <summary>
        /// Fonction adaptant les textures au
        /// redimensionnement de la fenetre
        /// </summary>
        /// <param name="rect">Nouvelle dimension de la fenetre obtenue par *Game1*.Window.ClientBounds()</param>
        private void windowResized(Rectangle rect)
        {
            foreach (AnimatedSprite p in animations)
                p.windowResized(rect);
            foreach (Sprite p in textures)
                p.windowResized(rect);
            tailleSelection.windowResized(rect);
            mouse.windowResized(rect);
        }
    }
}
