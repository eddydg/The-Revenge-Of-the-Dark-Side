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

namespace TestProj
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        List<AnimatedSprite> personnages;
        MouseState mouseState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        protected override void Initialize()
        {
            base.Initialize();
            this.IsMouseVisible = true;
            mouseState = Mouse.GetState();
            this.Window.AllowUserResizing = true;
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            personnages = new List<AnimatedSprite>();
        }
        protected override void UnloadContent()
        {
        }
        protected override void Update(GameTime gameTime)
        {
            if (this.IsActive)
            {
                MouseState newMouseState = Mouse.GetState();
                base.Update(gameTime);
                foreach (AnimatedSprite p in personnages)
                {
                    p.Next(gameTime.ElapsedGameTime.Milliseconds);
                }
                if (newMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton != ButtonState.Pressed)
                {
                    personnages.Add(new AnimatedSprite(Content.Load<Texture2D>("animation"), 8, 6, 40));
                    personnages.Last<AnimatedSprite>().XPos = mouseState.X - personnages.Last<AnimatedSprite>().sprite.Width / personnages.Last<AnimatedSprite>().colonnes / 2;
                    personnages.Last<AnimatedSprite>().YPos = mouseState.Y - personnages.Last<AnimatedSprite>().sprite.Height / personnages.Last<AnimatedSprite>().lignes / 2;
                    personnages.Last<AnimatedSprite>().actualPicture = personnages.Last<AnimatedSprite>().firstPicture;
                }
                if (newMouseState.RightButton == ButtonState.Pressed && mouseState.RightButton != ButtonState.Pressed)
                {
                    personnages.Add(new AnimatedSprite(Content.Load<Texture2D>("demo1"),8,4,30));
                    personnages.Last<AnimatedSprite>().XPos = mouseState.X - personnages.Last<AnimatedSprite>().sprite.Width / personnages.Last<AnimatedSprite>().colonnes / 2;
                    personnages.Last<AnimatedSprite>().YPos = mouseState.Y - personnages.Last<AnimatedSprite>().sprite.Height / personnages.Last<AnimatedSprite>().lignes / 2;
                    personnages.Last<AnimatedSprite>().actualPicture = personnages.Last<AnimatedSprite>().firstPicture;
                }
                if (newMouseState.MiddleButton == ButtonState.Pressed && mouseState.MiddleButton != ButtonState.Pressed)
                {
                    personnages.Add(new AnimatedSprite(Content.Load<Texture2D>("ninja"), 6,4,15,1,22));
                    personnages.Last<AnimatedSprite>().XPos = mouseState.X - personnages.Last<AnimatedSprite>().sprite.Width / personnages.Last<AnimatedSprite>().colonnes / 2;
                    personnages.Last<AnimatedSprite>().YPos = mouseState.Y - personnages.Last<AnimatedSprite>().sprite.Height / personnages.Last<AnimatedSprite>().lignes / 2;
                    personnages.Last<AnimatedSprite>().actualPicture = personnages.Last<AnimatedSprite>().firstPicture;
                }
                foreach (AnimatedSprite p in personnages)
                {
                    if (p.IsEnd())
                    {
                        personnages.RemoveAt(personnages.IndexOf(p));
                        break;
                    }
                }
                mouseState = newMouseState;
            }
        }
        protected override void Draw(GameTime gameTime)
        {
                GraphicsDevice.Clear(Color.CornflowerBlue);

                foreach (AnimatedSprite p in personnages)
                    p.Draw(spriteBatch);

                base.Draw(gameTime);
        }
    }
}
