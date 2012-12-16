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
    /// Classe principale de XNA
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private KeyboardState keyboardState;
        private MouseState mouseState;

        private Dictionary<Scene, AbstractScene> scenes;
        private Scene currentScene;

        public Son son;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 900;
            this.Window.AllowUserResizing = true;
            Rectangle winsize = this.Window.ClientBounds;

            son = new Son();

            scenes = new Dictionary<Scene, AbstractScene>();
            currentScene = Scene.MainMenu;
            scenes.Add(Scene.InGame, new InGame(winsize));
            scenes.Add(Scene.MainMenu, new MainMenu(winsize));
            scenes.Add(Scene.Extra, new SceneExtras(winsize));
            scenes.Add(Scene.Credit, new SceneCredit(winsize));
        }
        protected override void Initialize()
        {
            base.Initialize();
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            try
            {
                foreach (AbstractScene s in scenes.Values)
                    s.LoadContent(Content);
                son.LoadContent(Content, Sons.MenuSelection, "menu/selectionSound");
                son.LoadContent(Content, Musiques.MenuMusic, "menu/menuAmbience");
            }
            catch (Exception e)
            {
                EugLib.FileStream.toStdOut("Loading ressources error.");
                EugLib.FileStream.toStdOut(e.ToString());
                this.Exit();
            }
        }
        protected override void UnloadContent()
        {
        }
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            scenes[currentScene].Update(elapsedTime);
            scenes[currentScene].HandleInput(keyboardState,mouseState,this);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);

            try
            {
                scenes[currentScene].Draw(spriteBatch);
            }
            catch (Exception e)
            {
                EugLib.FileStream.toStdOut("Erreur d'affichage :");
                EugLib.FileStream.toStdOut(e.ToString());
                this.Exit();
            }
        }

        /// <summary>
        /// Changement de scene
        /// </summary>
        /// <param name="newScene">Nouvelle scene</param>
        public void SwitchScene(Scene newScene)
        {
            try
            {
                scenes[currentScene].EndScene(this);
                currentScene = newScene;
                scenes[newScene].Activation(this);
            }
            catch (Exception e)
            {
                EugLib.FileStream.toStdOut("La scene n'est pas definie :");
                EugLib.FileStream.toStdOut(e.ToString());
                this.Exit();
            }
        }
    }
}