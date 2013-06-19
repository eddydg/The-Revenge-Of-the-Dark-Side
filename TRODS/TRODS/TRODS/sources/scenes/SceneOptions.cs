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
using System.Net;

namespace TRODS
{
    class SceneOptions : AbstractScene
    {
        private KeyboardState _keyboardState;
        private MouseState _mouseState;
        private Rectangle _windowSize;
        private Sprite _wallpaper;
        private AnimatedSprite _mouse;
        public static string SOUND_FILENAME = "files/sound";
        private const string VERSION_NUMBER = "3,0";
        private Sprite _textMusic;
        private Sprite _textEffects;
        private Sprite _soundMusic;
        private Sprite _soundEffect;
        private float _volumeMusic;
        private float _volumeEffect;
        private TextSprite _checkUpdate;
        private TextSprite _serverConfig;

        public SceneOptions(Rectangle windowSize, KeyboardState keyboardState, MouseState mouseState)
        {
            _windowSize = windowSize;
            _keyboardState = keyboardState;
            _mouseState = mouseState;

            _wallpaper = new Sprite(new Rectangle(0, 0, _windowSize.Width, _windowSize.Height), _windowSize, "menu/wallpaper");
            _mouse = new AnimatedSprite(new Rectangle(-100, -100, 80, 100), windowSize, "sprites/cursorFire_8x4r", 8, 4, 40);
            _textMusic = new Sprite(new Rectangle(70, 433, 110, 40), _windowSize, "menu/soundMusic");
            _textEffects = new Sprite(new Rectangle(150, 490, 110, 40), _windowSize, "menu/soundEffect");
            _soundMusic = new Sprite(new Rectangle(180, 423, 110, 55), _windowSize, "menu/soundBars");
            _soundEffect = new Sprite(new Rectangle(260, 480, 110, 55), _windowSize, "menu/soundBars");
            _checkUpdate = new TextSprite("SpriteFont1", _windowSize, new Rectangle(530, 440, 171, 60), Color.Goldenrod, "Check Updates");
            _serverConfig = new TextSprite("SpriteFont1", _windowSize, new Rectangle(600, 500, 171, 60), Color.Goldenrod, "Server Options");
        }

        public override void LoadContent(ContentManager content)
        {
            _wallpaper.LoadContent(content);
            _mouse.LoadContent(content);
            _soundMusic.LoadContent(content);
            _soundEffect.LoadContent(content);
            _textMusic.LoadContent(content);
            _textEffects.LoadContent(content);
            _checkUpdate.LoadContent(content);
            _serverConfig.LoadContent(content);

            List<string> par = EugLib.IO.Tools.toArgv(EugLib.IO.FileStream.readFile(SOUND_FILENAME));
            if (par.Count < 2 ||
                !float.TryParse(par.ElementAt<string>(0), out _volumeMusic) ||
                !float.TryParse(par.ElementAt<string>(1), out _volumeEffect))
            {
                _volumeEffect = 1f;
                _volumeMusic = 1f;
            }
        }

        public override void HandleInput(KeyboardState newKeyboardState, MouseState newMouseState, Game1 parent)
        {
            if (_windowSize != parent.Window.ClientBounds)
            {
                _windowSize = parent.Window.ClientBounds;
                WindowResized(parent.Window.ClientBounds);
            }

            if (newMouseState != _mouseState)
            {
                _mouse.Position = new Rectangle(newMouseState.X, newMouseState.Y, _mouse.Position.Width, _mouse.Position.Height);
                if (_checkUpdate.Position.Contains(newMouseState.X, newMouseState.Y) && !_checkUpdate.Position.Contains(_mouseState.X, _mouseState.Y))
                    parent.son.Play(Sons.MenuSelection);
                if (_serverConfig.Position.Contains(newMouseState.X, newMouseState.Y) && !_serverConfig.Position.Contains(_mouseState.X, _mouseState.Y))
                    parent.son.Play(Sons.MenuSelection);
            }

            if (newKeyboardState.IsKeyDown(Keys.Escape) && !_keyboardState.IsKeyDown(Keys.Escape))
                parent.SwitchScene(Scene.MainMenu);

            if (newMouseState.LeftButton == ButtonState.Pressed)
            {
                Rectangle click = new Rectangle(newMouseState.X, newMouseState.Y, 1, 1);

                if (_soundEffect.Position.Intersects(click))
                    _volumeEffect = (float)(click.X - _soundEffect.Position.X) / (float)_soundEffect.Position.Width;
                if (_soundMusic.Position.Intersects(click))
                    _volumeMusic = (float)(click.X - _soundMusic.Position.X) / (float)_soundMusic.Position.Width;
                if (_checkUpdate.Position.Intersects(click) && _mouseState.LeftButton != ButtonState.Pressed)
                    CheckUpdate();
                if (_serverConfig.Position.Intersects(click) && _mouseState.LeftButton != ButtonState.Pressed)
                {
                    try
                    {
                        System.Diagnostics.Process.Start("ServerConfig.exe");
                    }
                    catch (Exception)
                    {
                        System.Windows.Forms.MessageBox.Show("File \"ServerConfig.exe\" not found.");
                    }
                }

                parent.son.MusiquesVolume = _volumeMusic;
                parent.son.SonsVolume = _volumeEffect;

                EugLib.IO.FileStream.writeFile(SOUND_FILENAME, _volumeMusic + " " + _volumeEffect);
            }

            _keyboardState = newKeyboardState;
            _mouseState = newMouseState;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            _wallpaper.Draw(spriteBatch);
            _soundMusic.Draw(spriteBatch, 50);
            _soundMusic.Draw(spriteBatch,
                                new Rectangle(_soundMusic.Position.X, _soundMusic.Position.Y, (int)(_volumeMusic * _soundMusic.Position.Width), _soundMusic.Position.Height),
                                new Rectangle(0, 0, (int)(_volumeMusic * _soundMusic.Texture.Width), _soundMusic.Texture.Height),
                                Color.White);
            _soundEffect.Draw(spriteBatch, 50);
            _soundEffect.Draw(spriteBatch,
                                new Rectangle(_soundEffect.Position.X, _soundEffect.Position.Y, (int)(_volumeEffect * _soundEffect.Position.Width), _soundEffect.Position.Height),
                                new Rectangle(0, 0, (int)(_volumeEffect * _soundEffect.Texture.Width), _soundEffect.Texture.Height),
                                Color.White);
            _textMusic.Draw(spriteBatch);
            _textEffects.Draw(spriteBatch);

            if (_checkUpdate.Position.Contains(_mouse.Position.Location))
                _checkUpdate.Draw(spriteBatch, Color.Red);
            else
                _checkUpdate.Draw(spriteBatch);

            if (_serverConfig.Position.Contains(_mouse.Position.Location))
                _serverConfig.Draw(spriteBatch, Color.Red);
            else
                _serverConfig.Draw(spriteBatch);

            _mouse.Draw(spriteBatch);

            spriteBatch.End();
        }

        public override void Update(float elapsedTime)
        {
            _mouse.Update(elapsedTime);
        }

        public override void WindowResized(Rectangle rect)
        {
            _wallpaper.windowResized(rect);
            _soundEffect.windowResized(rect);
            _soundMusic.windowResized(rect);
            _textMusic.windowResized(rect);
            _textEffects.windowResized(rect);
            _checkUpdate.windowResized(rect);
            _serverConfig.windowResized(rect);
        }

        //Fonctions Annexes/////////////////////////////////////
        private void CheckUpdate()
        {
            try
            {
                //recuperation de la version la plus récente
                WebClient versionPage = new WebClient();
                string version = versionPage.DownloadString("http://trods.free.fr/version.html");//de la forme: x.x#url#http://trods.free.fr/docs/.....
                string lastVersion = version.Substring(0, version.IndexOf('#'));
                string url = version.Substring(version.LastIndexOf('#') + 1);

                for (int i = 0; i < lastVersion.Length; i++)
                {
                    if (lastVersion[i] == '.')
                        lastVersion = lastVersion.Substring(0, i) + ',' + lastVersion.Substring(i + 1);
                }

                if (double.Parse(VERSION_NUMBER) < double.Parse(lastVersion))//Si le jeu n'est pas à jour.
                {
                    if (System.Windows.Forms.DialogResult.Yes == System.Windows.Forms.MessageBox.Show("Une mise à jour du jeu est disponible, voulez vous là télécharger?", "Mise à jour - TRODS " + VERSION_NUMBER, System.Windows.Forms.MessageBoxButtons.YesNo))
                        System.Diagnostics.Process.Start(url);
                }
                else
                    System.Windows.Forms.MessageBox.Show("Vous disposez de la dernière version du jeu.\nAucune mise à jour n'est nécessaire.", "Mise à jour - TRODS " + VERSION_NUMBER, System.Windows.Forms.MessageBoxButtons.OK);
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message + "\nVeuillez vérifier votre connexion internet puis réessayez.");
            }
        }
        ////////////////////////////////////////////////////////
    }
}
