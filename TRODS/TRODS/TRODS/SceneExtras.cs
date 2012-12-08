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
        public struct Dimension
        {
            public int colonnes,lignes;
            public Dimension(int x,int y)
            {
                this.colonnes = x;
                this.lignes = y;
            }
        }
        private List<Dimension> dimensions;
        private List<AnimatedSprite> sprites;
        private List<Sprite> textures;
        private Sprite tailleSelection;
        private AnimatedSprite mouse;

        public SceneExtras(Rectangle windowSize)
        {
            _windowSize = windowSize;
            selectedSprite = 0;
            currentSize = 150;
            sprites = new List<AnimatedSprite>();
            textures = new List<Sprite>();
            tailleSelection = new Sprite(new Rectangle(0, 400, windowSize.Width, 85), windowSize);
            mouse = new AnimatedSprite(new Rectangle(-100, -100, 40, 65), _windowSize, 5, 2, 25);

            dimensions = new List<Dimension>();
            dimensions.Add(new Dimension(16, 11));
            dimensions.Add(new Dimension(8,4));
            dimensions.Add(new Dimension(8,6));
            dimensions.Add(new Dimension(8,6));
            dimensions.Add(new Dimension(8,8));
            dimensions.Add(new Dimension(16,13));
            dimensions.Add(new Dimension(8,4));
            dimensions.Add(new Dimension(8,4));
            dimensions.Add(new Dimension(8,4));
            dimensions.Add(new Dimension(8,8));
        }

        public override void LoadContent(ContentManager content)
        {
            mouse.LoadContent(content, "cursor2");
            tailleSelection.LoadContent(content, "extraSize");
            for (int i = 0; i < 10; i++)
            {
                textures.Add(new Sprite(new Rectangle(i * 80, 485, _windowSize.Width / 10, 115), _windowSize));
                textures.ElementAt<Sprite>(i).LoadContent(content, "extra" + (i + 1).ToString());
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            tailleSelection.Draw(spriteBatch);
            foreach (Sprite s in textures)
                s.Draw(spriteBatch);
            foreach (AnimatedSprite p in sprites)
                p.Draw(spriteBatch);
            mouse.Draw(spriteBatch);
            spriteBatch.End();
        }

        public override void Update(float elapsedTime)
        {
            for (int i = 0; i < sprites.Count; i++)
            {
                sprites.ElementAt<AnimatedSprite>(i).Update(elapsedTime);
                if (sprites.ElementAt<AnimatedSprite>(i).IsEnd())
                {
                    sprites.RemoveAt(i);
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
                foreach (Sprite p in textures)
                    if (p.Position.Intersects(clic))
                        selectedSprite = textures.IndexOf(p);
                if (tailleSelection.Position.Intersects(clic))
                    currentSize = clic.X;
                else if (currentSize >= 0 && selectedSprite >= 0 && selectedSprite < textures.Count)
                {
                    sprites.Add(new AnimatedSprite(new Rectangle(clic.X - currentSize / 2, clic.Y - currentSize / 2, currentSize, currentSize), _windowSize, dimensions.ElementAt<Dimension>(selectedSprite).colonnes, dimensions.ElementAt<Dimension>(selectedSprite).lignes, 30));
                    sprites.Last<AnimatedSprite>().LoadContent(textures.ElementAt<Sprite>(selectedSprite).Texture);
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
            sprites.Clear();
        }

        /// <summary>
        /// Fonction adaptant les textures au
        /// redimensionnement de la fenetre
        /// </summary>
        /// <param name="rect">Nouvelle dimension de la fenetre obtenue par *Game1*.Window.ClientBounds()</param>
        private void windowResized(Rectangle rect)
        {
            foreach (AnimatedSprite p in sprites)
                p.windowResized(rect);
            foreach (Sprite p in textures)
                p.windowResized(rect);
            tailleSelection.windowResized(rect);
            mouse.windowResized(rect);
        }
    }
}
