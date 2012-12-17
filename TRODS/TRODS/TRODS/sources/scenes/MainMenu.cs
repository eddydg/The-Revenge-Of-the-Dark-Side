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
    class MainMenu : AbstractScene
    {
        private KeyboardState keyboardState;
        private MouseState mousestate;

        private Sprite wallpaper;
        private Sprite wallpaperText;
        private Sprite nuages;
        private AnimatedSprite mouse;
        private List<AnimatedSprite> sprites;
        private Texture2D cursorClic;

        private Selection selection;
        private enum Selection { Play = 0, Extra = 1, Exit = 2, Credit = 3 };
        private Dictionary<Selection, Sprite> menuItems;

        private static Vector2 decalage = new Vector2(10, 0);
        private static int amplitudeVibrationSelection = 5;
        private float relativeAmplitudeVibrationSelection;
        private static int textBorder = 1;
        private int windowHeight;
        private int windowWidth;

        public MainMenu(Rectangle windowSize)
        {
            windowWidth = windowSize.Width;
            windowHeight = windowSize.Height;
            selection = Selection.Play;
            wallpaper = new Sprite(new Rectangle(0, 0, windowWidth, windowHeight), windowSize);
            wallpaperText = new Sprite(new Rectangle(0, 0, windowWidth, windowHeight), windowSize);
            nuages = new Sprite(new Rectangle(0, 0, windowWidth * 3, windowHeight), windowSize);
            nuages.Direction = new Vector2(-1, 0);
            nuages.Vitesse = 0.1f; // 1f = 1000 px/sec
            mouse = new AnimatedSprite(new Rectangle(-100, -100, 80, 100), windowSize, 8, 4, 40);
            relativeAmplitudeVibrationSelection = (float)amplitudeVibrationSelection / (float)(windowHeight + windowWidth);
            sprites = new List<AnimatedSprite>();

            menuItems = new Dictionary<Selection, Sprite>();
            menuItems.Add(Selection.Play, new Sprite(new Rectangle(150, 400, 110, 55), windowSize));
            menuItems.Add(Selection.Extra, new Sprite(new Rectangle(215, 470, 110, 55), windowSize));
            menuItems.Add(Selection.Exit, new Sprite(new Rectangle(490, 400, 90, 55), windowSize));
            menuItems.Add(Selection.Credit, new Sprite(new Rectangle(560, 475, 110, 55), windowSize));
        }

        public override void LoadContent(ContentManager content)
        {
            wallpaper.LoadContent(content, "menu/wallpaper");
            wallpaperText.LoadContent(content, "menu/wallpaperText");
            nuages.LoadContent(content, "general/nuages0");
            mouse.LoadContent(content, "menu/cursor0_8x4r");
            cursorClic = content.Load<Texture2D>("menu/onde_8x4");
            menuItems[Selection.Play].LoadContent(content, "menu/textPlay");
            menuItems[Selection.Extra].LoadContent(content, "menu/textExtra");
            menuItems[Selection.Exit].LoadContent(content, "menu/textExit");
            menuItems[Selection.Credit].LoadContent(content, "menu/textCredit");
        }
        public override void HandleInput(KeyboardState newKeyboardState, MouseState newMouseState, Game1 parent)
        {
            Rectangle newWindowSize = parent.Window.ClientBounds;
            if (newWindowSize.Width != windowWidth || newWindowSize.Height != windowHeight)
                windowResized(newWindowSize);
            if (parent.IsActive)
            {
                if (newKeyboardState.IsKeyDown(Keys.Escape) && !keyboardState.IsKeyDown(Keys.Escape))
                {
                    selection = Selection.Exit;
                    parent.son.Play(Sons.MenuSelection);
                }
                if (newKeyboardState.IsKeyDown(Keys.Right) && !keyboardState.IsKeyDown(Keys.Right))
                {
                    selection++;
                    parent.son.Play(Sons.MenuSelection);
                }
                if (newKeyboardState.IsKeyDown(Keys.Left) && !keyboardState.IsKeyDown(Keys.Left))
                {
                    selection--;
                    parent.son.Play(Sons.MenuSelection);
                }
                if (newKeyboardState.IsKeyDown(Keys.Up) && !keyboardState.IsKeyDown(Keys.Up))
                {
                    selection--;
                    parent.son.Play(Sons.MenuSelection);
                }
                if (newKeyboardState.IsKeyDown(Keys.Down) && !keyboardState.IsKeyDown(Keys.Down))
                {
                    selection++;
                    parent.son.Play(Sons.MenuSelection);
                }
                if ((int)selection >= menuItems.Count)
                    selection = (Selection)0;
                else if ((int)selection < 0)
                    selection = (Selection)(menuItems.Count - 1);

                if (newKeyboardState.IsKeyDown(Keys.Enter) && !keyboardState.IsKeyDown(Keys.Enter))
                {
                    selectionEvent(parent);
                }

                if (mousestate != newMouseState)
                {
                    bool isClicked = newMouseState.LeftButton == ButtonState.Pressed && mousestate.LeftButton == ButtonState.Released;
                    mouse.Position = new Rectangle(newMouseState.X, newMouseState.Y, mouse.Position.Width, mouse.Position.Height);
                    int i = 0;
                    foreach (Sprite st in menuItems.Values)
                    {
                        if (st.Position.Intersects(new Rectangle(mouse.Position.X, mouse.Position.Y, 1, 1)))
                        {
                            if ((int)selection != i)
                                parent.son.Play(Sons.MenuSelection);
                            selection = (Selection)i;
                            if (isClicked)
                                selectionEvent(parent);
                        }
                        i++;
                    }
                    if (isClicked)
                    {
                        sprites.Add(new AnimatedSprite(new Rectangle(newMouseState.X - windowWidth / 3 / 2, newMouseState.Y - windowHeight / 3 / 2, windowWidth / 3, windowHeight / 3), newWindowSize, 8, 5, 35));
                        sprites.Last<AnimatedSprite>().LoadContent(cursorClic);
                    }
                }

                if (MediaPlayer.State != MediaState.Playing)
                    parent.son.Play(Musiques.MenuMusic);
                keyboardState = newKeyboardState;
                mousestate = newMouseState;
            }
        }
        public override void Update(float elapsedTime)
        {
            Rectangle p = nuages.Position;
            if (p.X + p.Width <= 0)
                nuages.Position = new Rectangle(0, p.Y, p.Width, p.Height);
            else
                nuages.Update(elapsedTime);
            mouse.Update(elapsedTime);
            for (int i = 0; i < sprites.Count; i++)
            {
                sprites.ElementAt<AnimatedSprite>(i).Update(elapsedTime);
                if (sprites.ElementAt<AnimatedSprite>(i).IsEnd())
                {
                    sprites.RemoveAt(i);
                    i--;
                }
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            wallpaper.Draw(spriteBatch);

            Rectangle np = nuages.Position;
            nuages.Draw(spriteBatch);
            if (np.X + np.Width < windowWidth)
                nuages.Draw(spriteBatch, Color.White, np.X + np.Width, np.Y);

            wallpaperText.Draw(spriteBatch);

            foreach (Sprite st in menuItems.Values)
            {
                Rectangle p = st.Position;
                if (st == menuItems[selection])
                {
                    st.Draw(spriteBatch, Color.Red,
                                    (int)(p.X + new Random().Next(-amplitudeVibrationSelection, amplitudeVibrationSelection) + decalage.X),
                                    (int)(p.Y + new Random().Next(-amplitudeVibrationSelection, amplitudeVibrationSelection) + decalage.Y));
                    st.Draw(spriteBatch, Color.Black, (int)(p.X + decalage.X), (int)(p.Y + decalage.Y));
                }
                else
                {
                    st.Draw(spriteBatch, Color.White, p.X - textBorder, p.Y - textBorder);
                    st.Draw(spriteBatch, Color.White, p.X + textBorder, p.Y + textBorder);
                    st.Draw(spriteBatch, Color.Black);
                }
            }

            foreach (AnimatedSprite p in sprites)
                p.Draw(spriteBatch);

            mouse.Draw(spriteBatch);

            spriteBatch.End();
        }

        public override void Activation(Game1 parent)
        {
            mousestate = Mouse.GetState();
            sprites.Clear();
            mouse.Position = new Rectangle(-100, -100, mouse.Position.Width, mouse.Position.Height);
            parent.son.Play(Musiques.MenuMusic);
        }

        public override void EndScene(Game1 parent)
        {
            parent.son.Stop();
        }

        /// <summary>
        /// Gestion de l'evenement de selection
        /// </summary>
        /// <param name="parent">Reference de la classe Game1 parent</param>
        private void selectionEvent(Game1 parent)
        {
            parent.son.Play(Sons.MenuSelection);
            switch (selection)
            {
                case Selection.Exit:
                    parent.Exit();
                    break;
                case Selection.Play:
                    parent.SwitchScene(Scene.InGame);
                    break;
                case Selection.Extra:
                    parent.SwitchScene(Scene.Extra);
                    break;
                case Selection.Credit:
                    parent.SwitchScene(Scene.Credit);
                    break;
            }
        }
        /// <summary>
        /// Fonction adaptant les textures au
        /// redimensionnement de la fenetre
        /// </summary>
        /// <param name="rect">Nouvelle dimension de la fenetre obtenue par *Game1*.Window.ClientBounds()</param>
        private void windowResized(Rectangle rect)
        {
            wallpaper.windowResized(rect);
            foreach (Sprite s in menuItems.Values)
                s.windowResized(rect);
            wallpaperText.windowResized(rect);
            nuages.windowResized(rect);
            windowWidth = rect.Width;
            windowHeight = rect.Height;
            amplitudeVibrationSelection = (int)(relativeAmplitudeVibrationSelection * (windowWidth + windowHeight));
            mouse.windowResized(rect);
            foreach (AnimatedSprite p in sprites)
                p.windowResized(rect);
        }
    }
}