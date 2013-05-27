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
        private Sprite tailleSelection;
        private AnimatedSprite mouse;
        private ContextMenu menu;

        public SceneExtras(Rectangle windowSize, KeyboardState keyboardState, MouseState mouseState)
        {
            _windowSize = windowSize;
            _mouseState = mouseState;
            _keyboardState = keyboardState;
            selectedSprite = 0;
            currentSize = 150;
            tailleSelection = new Sprite(new Rectangle(0, _windowSize.Height - 85, windowSize.Width, 85), windowSize);
            mouse = new AnimatedSprite(new Rectangle(-100, -100, 60, 80), _windowSize, 8, 4, 30);

            animations = new List<AnimatedSprite>();
            menu = new ContextMenu(_windowSize, new AnimatedSprite(new Rectangle(200, 50, 300, 50), _windowSize, "menu/ContextualMenuBlackFull"), "menu/contextMenuExit", 100);
            menu.Title = new AnimatedSprite(new Rectangle(menu.Position.Width / 2 - 75, 0, 150, 50), _windowSize, "menu/contextMenuText");
            menu.Visible = false;
            menu.Add(new AnimatedSprite(new Rectangle(), windowSize, "sprites/canalisation1_16x13", 16, 13));
            menu.Add(new AnimatedSprite(new Rectangle(), windowSize, "sprites/explosion0_8x6", 8, 6));
            menu.Add(new AnimatedSprite(new Rectangle(), windowSize, "sprites/explosion1_8x6", 8, 6));
            menu.Add(new AnimatedSprite(new Rectangle(), windowSize, "sprites/explosion2_8x8", 8, 8));
            menu.Add(new AnimatedSprite(new Rectangle(), windowSize, "sprites/explosion3_8x4", 8, 4));
            menu.Add(new AnimatedSprite(new Rectangle(), windowSize, "sprites/explosion4_8x8", 8, 8));
            menu.Add(new AnimatedSprite(new Rectangle(), windowSize, "sprites/fireWall_11x6r23r44", 11, 6));
            menu.Add(new AnimatedSprite(new Rectangle(), windowSize, "sprites/popGreen_8x4", 8, 4));
            menu.Add(new AnimatedSprite(new Rectangle(), windowSize, "sprites/spriteElectric_11x3r12r23", 11, 3));
            menu.CuadricPositionning(new Rectangle(0, 0, 75, 75), 100, 20, 3, 3, true);
            menu.Add(new AnimatedSprite(new Rectangle(menu.Position.Width / 2 - 100, 65, 200, 22), _windowSize, "menu/contextMenuTextMainMenu"));
        }

        public override void LoadContent(ContentManager content)
        {
            mouse.LoadContent(content, "sprites/cursorFire_8x4r");
            tailleSelection.LoadContent(content, "menu/sizeSelection");
            menu.LoadContent(content);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            foreach (AnimatedSprite p in animations)
                p.Draw(spriteBatch);
            tailleSelection.Draw(spriteBatch);
            menu.Draw(spriteBatch);
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
            menu.Update(elapsedTime);
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
                menu.Visible = !menu.Visible;
            if (_mouseState != newMouseState)
                mouse.Position = new Rectangle(newMouseState.X, newMouseState.Y, mouse.Position.Width, mouse.Position.Height);

            menu.HandleInput(newKeyboardState, newMouseState, parent);
            if (menu.Choise == menu.Elements.Count - 1)
                parent.SwitchScene(Scene.MainMenu);
            else if (menu.Choise >= 0 && menu.Choise < menu.Elements.Count)
                selectedSprite = menu.Choise;

            if (newMouseState.LeftButton == ButtonState.Pressed && _mouseState.LeftButton != ButtonState.Pressed)
            {
                Rectangle clic = new Rectangle(mouse.Position.X, mouse.Position.Y, 1, 1);
                bool startAnimation = true;
                if (clic.Intersects(tailleSelection.Position))
                {
                    startAnimation = false;
                    currentSize = clic.X - tailleSelection.Position.X;
                }
                if (clic.Intersects(menu.Position) && menu.Visible)
                    startAnimation = false;
                if (startAnimation)
                {
                    AnimatedSprite s = menu.Elements.ElementAt<AnimatedSprite>(selectedSprite);
                    animations.Add(new AnimatedSprite(
                        new Rectangle(clic.X - currentSize / 2, clic.Y - currentSize / 2, currentSize, currentSize),
                        _windowSize, s.AssetName, s.Colonnes, s.Lignes));
                    animations.Last<AnimatedSprite>().LoadContent(parent.Content);
                }
            }

            _keyboardState = newKeyboardState;
            _mouseState = newMouseState;
        }

        public override void Activation(Game1 parent)
        {
            _mouseState = Mouse.GetState();
            _keyboardState = Keyboard.GetState();
            menu.Activation(parent);
        }

        public override void EndScene(Game1 parent)
        {
            animations.Clear();
            menu.EndScene(parent);
        }

        private void windowResized(Rectangle rect)
        {
            foreach (AnimatedSprite p in animations)
                p.windowResized(rect);
            tailleSelection.windowResized(rect);
            menu.WindowResized(rect);
            mouse.windowResized(rect);
        }
    }
}