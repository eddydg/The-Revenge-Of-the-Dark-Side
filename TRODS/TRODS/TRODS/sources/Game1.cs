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

            if (EugLib.IO.FileStream.readFile("files/language") == "")
                EugLib.IO.FileStream.writeFile("files/language", "e");
            INFO.ENG = EugLib.IO.FileStream.readFile("files/language")[0] == 'e';

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
            //this.Window.AllowUserResizing = true;
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
            scenes.Add(Scene.IntroVid, new VideoReader("general/introduction_trods", Scene.IntroHistoire));

            Animation anim = new Animation(winsize, Scene.Titre);
            anim.Add("game/game_over", new Rectangle(0, 0, winsize.Width, winsize.Height), new Rectangle(0, 0, winsize.Width, winsize.Height), 0, 5000, true, 200, true, 700);
            anim.Add(new TextSprite("SpriteFont1", winsize, new Rectangle(), Color.DarkRed, INFO.ENG?"Quest FAILED...":"Echec de la mission"),
                new Rectangle(300, 500, 300, 100),
                new Rectangle(330, 600, 240, 60),
                500, 4500, true, 700, true, 1000);
            scenes.Add(Scene.GameOver, anim);
            //intro
            anim = new Animation(winsize, Scene.Titre, Musiques.Intro);
            anim.Add("animation/intro/1", new Rectangle(0, 0, winsize.Width, winsize.Height), 500, 24000, 2000, 1500);
            MultipleTextSprite ts = new MultipleTextSprite("SpriteFont1", winsize, new Rectangle(50, 100, 300, 400), Color.Honeydew);
            ts.Add(EugLib.IO.FileStream.readFileLines("Content/animation/intro/"+(INFO.ENG?"1a.txt":"1.txt")));
            ts.StartShowing(0, 30);
            anim.Add(ts, new Rectangle(50, 100, 350, 400), new Rectangle(50, 100, 350, 400), 4000, 19000, true, 0, true, 1000);
            TextSprite tss = new TextSprite("SpriteFont1", winsize, new Rectangle(200, 250, 500, 100), Color.Honeydew, EugLib.IO.FileStream.readFile("Content/animation/intro/"+(INFO.ENG?"2a.txt":"2 - Ecran noir.txt")));
            anim.Add(tss, new Rectangle(200, 250, 500, 100), new Rectangle(100, 250, 700, 400), 24000, 4000, true, 400, true, 500);
            anim.Add("animation/intro/3", new Rectangle(0, 0, winsize.Width, winsize.Height), 28500, 28000, 100, 1000);
            ts = new MultipleTextSprite("SpriteFont1", winsize, new Rectangle(100, 300, 700, 800), Color.DarkRed);
            ts.Add(EugLib.IO.FileStream.readFileLines("Content/animation/intro/"+(INFO.ENG?"3a.txt":"3.txt")));
            ts.StartShowing(0, 30);
            anim.Add(ts, new Rectangle(100, 300, 700, 800), new Rectangle(100, -500, 700, 800), 29000, 27500, false, 0, true, 1000);
            anim.Add("animation/intro/4", new Rectangle(0, 0, winsize.Width, winsize.Height), 57500, 16000, 400, 1000);
            ts = new MultipleTextSprite("SpriteFont1", winsize, new Rectangle(100, 300, 700, 800), Color.Honeydew);
            ts.Add(EugLib.IO.FileStream.readFileLines("Content/animation/intro/"+(INFO.ENG?"4a.txt":"4.txt")));
            ts.StartShowing(0, 30);
            anim.Add(ts, new Rectangle(250, 100, 450, 400), new Rectangle(250, 100, 450, 400), 58000, 15000, true, 400, true, 400);
            scenes.Add(Scene.IntroHistoire, anim);
            //intro LateX
            anim = new Animation(winsize,Scene.InGame,Musiques.IntroLateX);
            ts = new MultipleTextSprite("SpriteFont1", winsize, new Rectangle(100, 100, 700, 800), Color.DarkRed);
            ts.Add(EugLib.IO.FileStream.readFileLines("Content/animation/LateX/1 - Arrivée.txt"));
            ts.StartShowing(0, 30);
            anim.Add("animation/LateX/1 - 0", new Rectangle(0, 0, winsize.Width, winsize.Height), 0, 9500, 1500, 1000);
            anim.Add("animation/LateX/1 - 1", new Rectangle(0, 0, winsize.Width, winsize.Height), 8500, 9500, 1000, 1500);
            anim.Add(ts, new Rectangle(250, 20, 400, 500),new Rectangle(250, 20, 400, 500), 1000, 16500, true,500,true,700);
            scenes.Add(Scene.IntroLateX, anim);
            //LateX Eradicated
            anim = new Animation(winsize, Scene.InGame, Musiques.TransitionLateX);
            ts = new MultipleTextSprite("SpriteFont1", winsize, new Rectangle(100, 250, 400, 500), Color.Honeydew);
            ts.Add(EugLib.IO.FileStream.readFileLines("Content/animation/LateX/2.txt"));
            ts.StartShowing(0, 30);
            anim.Add("animation/LateX/2", new Rectangle(0, 0, winsize.Width, winsize.Height), 0, 16000, 1500, 1000);
            anim.Add(ts, new Rectangle(250, 250, 400, 500), new Rectangle(250, -250, 400, 500), 1000, 15000, true, 500, true, 700);
            scenes.Add(Scene.LateXEradicated, anim);
            //Before LateX's King Fight
            anim = new Animation(winsize, Scene.InGame, Musiques.TransitionLateX);
            ts = new MultipleTextSprite("SpriteFont1", winsize, new Rectangle(100, 250, 400, 500), Color.Honeydew);
            ts.Add(EugLib.IO.FileStream.readFileLines("Content/animation/LateX/3.txt"));
            ts.StartShowing(0, 30);
            anim.Add("animation/LateX/3", new Rectangle(0, 0, winsize.Width, winsize.Height), 0, 16000, 1500, 1000);
            anim.Add(ts, new Rectangle(350, 100, 350, 350), new Rectangle(350, 100, 350, 350), 1000, 15000, true, 500, true, 700);
            scenes.Add(Scene.BeforeKingFight, anim);
            //After LateX's King Fight + come back to OpenEdge
            anim = new Animation(winsize, Scene.InGame, Musiques.TransitionLateX);
            ts = new MultipleTextSprite("SpriteFont1", winsize, new Rectangle(100, 250, 400, 500), Color.DarkRed);
            ts.Add(EugLib.IO.FileStream.readFileLines("Content/animation/LateX/4.txt"));
            ts.StartShowing(0, 30);
            anim.Add("animation/LateX/4", new Rectangle(0, 0, winsize.Width, winsize.Height), 0, 18000, 1500, 1000);
            anim.Add(ts, new Rectangle(100, 20, 700, 200), new Rectangle(100, 20, 700, 200), 500, 17000, true, 500, true, 700);
            ts = new MultipleTextSprite("SpriteFont1", winsize, new Rectangle(100, 250, 400, 500), Color.DarkRed);
            ts.Add(EugLib.IO.FileStream.readFileLines("Content/animation/OpenEdge/1.txt"));
            ts.StartShowing(0, 30);
            anim.Add("animation/OpenEdge/1-0", new Rectangle(0, 0, winsize.Width, winsize.Height), 19000, 9500, 1500, 1000); 
            anim.Add("animation/OpenEdge/1", new Rectangle(0, 0, winsize.Width, winsize.Height), 27500, 9500, 1000, 1000);
            anim.Add(ts, new Rectangle(100, 300, 700, 200), new Rectangle(100, 300, 700, 200), 20000, 17000, true, 500, true, 700);
            scenes.Add(Scene.AfterKingFight, anim);
            //Last EPIC fight
            anim = new Animation(winsize, Scene.InGame, Musiques.TransitionLateX);
            ts = new MultipleTextSprite("SpriteFont1", winsize, new Rectangle(100, 250, 400, 500), Color.Honeydew);
            ts.Add(EugLib.IO.FileStream.readFileLines("Content/animation/OpenEdge/2.txt"));
            ts.StartShowing(0, 30);
            anim.Add("animation/OpenEdge/2", new Rectangle(0, 0, winsize.Width, winsize.Height), 0, 16000, 1500, 1000);
            anim.Add(ts, new Rectangle(50, 150, 400, 300), new Rectangle(50, 150, 400, 300), 1000, 14500, true, 500, true, 700);
            scenes.Add(Scene.LastFight, anim);

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
                son.LoadContent(Content, Musiques.IntroLateX, "animation/LateX/1 (08)");
                son.LoadContent(Content, Musiques.TransitionLateX, "animation/LateX/Transition");
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
        protected override void OnExiting(object sender, EventArgs args)
        {
            base.OnExiting(sender, args);
            ((InGame)scenes[Scene.InGame]).StopAllConnections();
        }
    }
}