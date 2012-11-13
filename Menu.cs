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

namespace WindowsGame1
{
    class Menu
    {
        private Sprite wallpaper;
        private Sprite mouse;
        private KeyboardState keyboardState;
        private MouseState mousestate;
        private SoundEffect selectionChangeSon;
        private Selection selection;
        private enum Selection { Play = 0, Exit = 1 };
        private List<Sprite> menuItems;
        private static Vector2 decalage = new Vector2(10,0);
        private static int amplitudeVibrationSelection = 5;
        private int windowHeight;
        private int windowWidth;

        public Menu(Rectangle windowSize)
        {
            windowWidth = windowSize.Width;
            windowHeight = windowSize.Height;
            selection = Selection.Play;
            wallpaper = new Sprite(new Rectangle(0, 0, windowWidth, windowHeight), windowWidth, windowHeight);
            mouse = new Sprite(new Rectangle(-100,-100,20,30));

            menuItems = new List<Sprite>();
            menuItems.Add(new Sprite(new Rectangle(155,327,110,44),windowWidth,windowHeight)); // play
            menuItems.Add(new Sprite(new Rectangle(507, 334, 90, 39), windowWidth, windowHeight)); // exit
        }

        public void LoadContent(ContentManager content)
        {
            wallpaper.LoadContent(content, "wallpaperR");
            mouse.LoadContent(content, "curseur");
            selectionChangeSon = content.Load<SoundEffect>("menuselection");
            menuItems.ElementAt<Sprite>(0).LoadContent(content,"play");
            menuItems.ElementAt<Sprite>(1).LoadContent(content, "exit");
        }
        public void HandleInput(KeyboardState newKeyboardState, MouseState newMouseState, Game1 parent)
        {
            Rectangle newWindowSize = parent.Window.ClientBounds;
            if (newWindowSize.Width != windowWidth || newWindowSize.Height != windowHeight)
                windowResized(newWindowSize);
            if (parent.IsActive)
            {
                if (newKeyboardState.IsKeyDown(Keys.Escape))
                {
                    parent.Exit();
                }
                if (newKeyboardState.IsKeyDown(Keys.Right) && !keyboardState.IsKeyDown(Keys.Right))
                {
                    selection++;
                    selectionChangeSon.Play();
                }
                if (newKeyboardState.IsKeyDown(Keys.Left) && !keyboardState.IsKeyDown(Keys.Left))
                {
                    selection--;
                    selectionChangeSon.Play();
                }
                if (newKeyboardState.IsKeyDown(Keys.Up) && !keyboardState.IsKeyDown(Keys.Up))
                {
                    selection--;
                    selectionChangeSon.Play();
                }
                if (newKeyboardState.IsKeyDown(Keys.Down) && !keyboardState.IsKeyDown(Keys.Down))
                {
                    selection++;
                    selectionChangeSon.Play();
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
                    mouse.Position = new Rectangle(newMouseState.X, newMouseState.Y,mouse.Position.Width,mouse.Position.Height);
                    int i = 0;
                    foreach (Sprite st in menuItems)
                    {
                        if (st.Position.Intersects(new Rectangle(mouse.Position.X,mouse.Position.Y,1,1)))
                        {
                            if ((int)selection != i)
                                selectionChangeSon.Play();
                            selection = (Selection)i;
                            if (newMouseState.LeftButton == ButtonState.Pressed && mousestate.LeftButton == ButtonState.Released)
                                selectionEvent(parent);
                        }
                        i++;
                    }
                }

                keyboardState = newKeyboardState;
                mousestate = newMouseState;
            }
        }
        public void Update(float elapsedTime)
        {
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            int i = 0;
            wallpaper.Draw(spriteBatch);
            foreach (Sprite st in menuItems)
            {
                Rectangle p = st.Position;
                if (i == (int)selection)
                {
                    st.DrawWith(spriteBatch, Color.Red, 
                                    (int)(p.X + new Random().Next(-amplitudeVibrationSelection,amplitudeVibrationSelection)+decalage.X),
                                    (int)(p.Y + new Random().Next(-amplitudeVibrationSelection,amplitudeVibrationSelection)+decalage.Y));
                    st.DrawWith(spriteBatch, Color.Black, (int)(p.X +decalage.X), (int)(p.Y+decalage.Y));
                }
                else if (i == (int)selection -1 || i == (int)selection +1)
                    st.DrawWith(spriteBatch, Color.Black, (int)(p.X + decalage.X/4), (int)(p.Y+decalage.Y/4));
                else
                    st.DrawWith(spriteBatch,Color.Black);
                i++;
            }
            spriteBatch.Draw(mouse.Texture, mouse.Position, Color.White);
        }

        private void selectionEvent(Game1 parent)
        {
            selectionChangeSon.Play();
            switch (selection)
            {
                case Selection.Exit:
                    parent.Exit();
                    break;
                case Selection.Play:
                    break;
            }
        }
        private void windowResized(Rectangle rect)
        {
            wallpaper.windowResized(rect);
            foreach (Sprite s in menuItems)
                s.windowResized(rect);
            windowWidth = rect.Width;
            windowHeight = rect.Height;
        }
    }
}
