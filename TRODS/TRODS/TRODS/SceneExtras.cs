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
            textures.Add(new AnimatedSprite(new Rectangle(), windowSize, "sprites/canalisation1_16x13", 16, 13));
            textures.Add(new AnimatedSprite(new Rectangle(), windowSize, "sprites/explosion0_8x6", 8, 6));
            textures.Add(new AnimatedSprite(new Rectangle(), windowSize, "sprites/explosion1_8x6", 8, 6));
            textures.Add(new AnimatedSprite(new Rectangle(), windowSize, "sprites/explosion2_8x8", 8, 8));
            textures.Add(new AnimatedSprite(new Rectangle(), windowSize, "sprites/explosion3_8x4", 8, 4));
            textures.Add(new AnimatedSprite(new Rectangle(), windowSize, "sprites/explosion4_8x8", 8, 8));
            textures.Add(new AnimatedSprite(new Rectangle(), windowSize, "sprites/fireWall_11x6r23r45", 11, 6));
            textures.Add(new AnimatedSprite(new Rectangle(), windowSize, "sprites/popGreen_8x4", 8, 4));
            textures.Add(new AnimatedSprite(new Rectangle(), windowSize, "sprites/spriteElectric_11x3r12r23", 11, 3));
            int c = textures.Count;
            int wi = windowSize.Width / c;
            for (int i = 0; i < c; i++)
            {
                textures.ElementAt<AnimatedSprite>(i).setRelatvePos(
                    new Rectangle(i * wi, 485, wi, windowSize.Height-485), windowSize.Width, windowSize.Height);
            }
        }

        public override void LoadContent(ContentManager content)
        {
            mouse.LoadContent(content, "menu/cursor0_8x4r");
            tailleSelection.LoadContent(content, "menu/sizeSelection");
            foreach (AnimatedSprite s in textures)
                s.LoadContent(content);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            foreach (AnimatedSprite p in animations)
                p.Draw(spriteBatch);
            foreach (Sprite s in textures)
                s.Draw(spriteBatch);
            tailleSelection.Draw(spriteBatch);
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
            foreach (AnimatedSprite s in textures)
                s.Update(elapsedTime);
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
                bool startAnimation = true;
                if (clic.Intersects(tailleSelection.Position))
                {
                    startAnimation = false;
                    currentSize = clic.X - tailleSelection.Position.Width;
                }
                foreach (AnimatedSprite s in textures)
                {
                    if (clic.Intersects(s.Position))
                    {
                        startAnimation = false;
                        selectedSprite = textures.IndexOf(s);
                    }
                }
                if (startAnimation)
                {
                    AnimatedSprite newAnim = new AnimatedSprite(new Rectangle(clic.X-currentSize/2,clic.Y-currentSize/2,currentSize,currentSize),_windowSize,
                }
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
