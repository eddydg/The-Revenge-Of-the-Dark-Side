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
        public GraphicsDeviceManager Graphics
        {
            get { return graphics; }
            private set { graphics = value; }
        }
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

            List<String> ws = EugLib.IO.Tools.toArgv(EugLib.IO.FileStream.readFile("files/WinSize"));
            int a, b;
            if (ws.Count >= 2 && int.TryParse(ws.ElementAt(0), out a) && int.TryParse(ws.ElementAt(1), out b))
            {
                graphics.PreferredBackBufferWidth = a;
                graphics.PreferredBackBufferHeight = b;
            }
            else
            {
                graphics.PreferredBackBufferWidth = 900;
                graphics.PreferredBackBufferHeight = 600;
                EugLib.IO.FileStream.writeFile("files/WinSize", graphics.PreferredBackBufferWidth.ToString() + " " + graphics.PreferredBackBufferHeight.ToString());
            }
            graphics.ApplyChanges();
            this.Window.AllowUserResizing = true;
            System.Windows.Forms.Form.FromHandle(Window.Handle).MinimumSize = new System.Drawing.Size(400, 400);//taille minimale
            Rectangle winsize = Window.ClientBounds;
            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();

            son = new Son();

            scenes = new Dictionary<Scene, AbstractScene>();
            scenes.Add(Scene.InGame, new InGame(winsize, keyboardState, mouseState));
            scenes.Add(Scene.MainMenu, new MainMenu(winsize, keyboardState, mouseState));
            scenes.Add(Scene.Extra, new SceneExtras(winsize, keyboardState, mouseState));
            scenes.Add(Scene.Credit, new SceneCredit(winsize, keyboardState, mouseState));
            scenes.Add(Scene.Titre, new SceneTitre(winsize, keyboardState, mouseState));
            scenes.Add(Scene.Options, new SceneOptions(winsize, keyboardState, mouseState));
            scenes.Add(Scene.MenuExtra, new MenuExtra(winsize, keyboardState, mouseState));
            scenes.Add(Scene.IntroVid, new VideoReader("general/introduction_trods", Scene.IntroHistoire));

            Animation anim = new Animation(winsize, Scene.Titre);
            anim.Add("game/game_over", new Rectangle(0, 0, winsize.Width, winsize.Height), new Rectangle(0, 0, winsize.Width, winsize.Height), 0, 5000, true, 200, true, 700);
            anim.Add(new TextSprite("SpriteFont1", winsize, new Rectangle(), Color.DarkRed, "Quest FAILED..."),
                new Rectangle(300, 500, 300, 100),
                new Rectangle(330, 600, 240, 60),
                500, 4500, true, 700, true, 1000);
            scenes.Add(Scene.GameOver, anim);

            anim = new Animation(winsize, Scene.Titre, Musiques.Intro);
            anim.Add("animation/intro/1", new Rectangle(0, 0, winsize.Width, winsize.Height), 500, 24000, 2000, 1500);
            MultipleTextSprite ts = new MultipleTextSprite("SpriteFont1", winsize, new Rectangle(50, 100, 300, 400), Color.Honeydew);
            ts.Add(EugLib.IO.FileStream.readFileLines("Content/animation/intro/1.txt"));
            ts.StartShowing(0, 30);
            anim.Add(ts, new Rectangle(50, 100, 350, 400), new Rectangle(50, 100, 350, 400), 4000, 19000, true, 0, true, 1000);
            TextSprite tss = new TextSprite("SpriteFont1", winsize, new Rectangle(200, 250, 500, 100), Color.Honeydew, EugLib.IO.FileStream.readFile("Content/animation/intro/2 - Ecran noir.txt"));
            anim.Add(tss, new Rectangle(200, 250, 500, 100), new Rectangle(100, 250, 700, 400), 24000, 4000, true, 400, true, 500);
            anim.Add("animation/intro/3", new Rectangle(0, 0, winsize.Width, winsize.Height), 28500, 21500, 100, 1000);
            ts = new MultipleTextSprite("SpriteFont1", winsize, new Rectangle(), Color.Honeydew);
            ts.Add(EugLib.IO.FileStream.readFileLines("Content/animation/intro/3.txt"));
            ts.StartShowing(0, 30);
            anim.Add(ts, new Rectangle(100, 300, 700, 800), new Rectangle(100, -300, 700, 800), 29000, 18000, false, 0, true, 1000);
            scenes.Add(Scene.IntroHistoire, anim);

            currentScene = Scene.IntroVid;
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
                son.LoadContent(Content, Musiques.CreditMusic, "menu/songCredit");
                son.LoadContent(Content, Musiques.Intro, "animation/intro/intro");
                float _volumeEffect = 1f;
                float _volumeMusic = 1f;
                List<string> par = EugLib.IO.Tools.toArgv(EugLib.IO.FileStream.readFile(SceneOptions.SOUND_FILENAME));
                if (par.Count >= 2)
                {
                    float.TryParse(par.ElementAt<string>(0), out _volumeMusic);
                    float.TryParse(par.ElementAt<string>(1), out _volumeEffect);
                }
                son.MusiquesVolume = _volumeMusic;
                son.SonsVolume = _volumeEffect;
            }
            catch (Exception e)
            {
                EugLib.IO.FileStream.toStdOut("Loading ressources error.");
                EugLib.IO.FileStream.toStdOut(e.ToString());
                this.Exit();
            }
            scenes[currentScene].Activation(this);
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

            try
            {
                scenes[currentScene].Update(elapsedTime);
                scenes[currentScene].HandleInput(keyboardState, mouseState, this);
            }
            catch (Exception e)
            {
                EugLib.IO.FileStream.toStdOut(e.ToString());
                this.Exit();
            }
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
                EugLib.IO.FileStream.toStdOut("Erreur d'affichage :");
                EugLib.IO.FileStream.toStdOut(e.ToString());
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
                EugLib.IO.FileStream.toStdOut("Erreur de changement de scene :");
                EugLib.IO.FileStream.toStdOut(e.ToString());
                this.Exit();
            }
        }
    }
}