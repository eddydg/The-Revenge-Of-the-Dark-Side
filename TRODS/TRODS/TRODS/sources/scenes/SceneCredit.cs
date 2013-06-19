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
    class SceneCredit : AbstractScene
    {
        private KeyboardState _keyboardState;
        private Rectangle _windowSize;
        private List<Sprite> animations;
        private ParticleEngine _particles;
        private MultipleTextSprite _authors;

        public SceneCredit(Rectangle windowSize, KeyboardState keyboardState, MouseState mouseState)
        {
            _windowSize = windowSize;
            _keyboardState = keyboardState;

            animations = new List<Sprite>();
            animations.Add(new TextSprite("SpriteFont1", _windowSize, new Rectangle(80, 250, 300, 75), Color.Red, "Team BEEN"));
            _authors = new MultipleTextSprite("SpriteFont1", _windowSize, new Rectangle(394, _windowSize.Height, 500, 130), Color.Red);
            _authors.Direction = new Vector2(0, -1);
            _authors.Vitesse = 0.1f;
            foreach (string s in EugLib.IO.FileStream.readFileLines("Content/AUTHORS"))
                _authors.Add(s);
            animations.Add(_authors);
            animations.Add(new AnimatedSprite(new Rectangle(0, 0, _windowSize.Width, 2 * _windowSize.Height / 8), _windowSize, "menu/credit"));
            animations.Add(new AnimatedSprite(new Rectangle(-300, _windowSize.Height - 100, _windowSize.Width + 300, 100), _windowSize, "menu/lueur1_10x4r21r40", 10, 4, 15, 21, 40, 1));
            _particles = new ParticleEngine(
                _windowSize,
                new DecimalRectangle(0, 0, _windowSize.Width, 0),
                new Vector3(0.2f, 10f, 10f),
                new List<string>() { "particle/ash" },
                10, 0.3f, 1.2f, -90f, 25, 0, 180, -2, 2, 500f, 700f);
            _particles.SetColorRange(80, 80, 80, 80, 80, 80);
        }

        public override void LoadContent(ContentManager content)
        {
            foreach (Sprite s in animations)
                s.LoadContent(content);
            _authors.LoadContent(content);
            _particles.LoadContent(content);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            _particles.Draw(spriteBatch);
            foreach (Sprite s in animations)
                s.Draw(spriteBatch);
            //_authors.Draw(spriteBatch);
            //animations[0].Draw(spriteBatch,_authors.Position);
            spriteBatch.End();
        }

        public override void Update(float elapsedTime)
        {
            foreach (Sprite s in animations)
                s.Update(elapsedTime);
            //_authors.Update(elapsedTime);
            if (_authors.Position.Y < -_authors.Position.Height)
                ResetAuthors();
            _particles.Update(elapsedTime);
        }

        public override void HandleInput(KeyboardState newKeyboardState, MouseState newMouseState, Game1 parent)
        {
            if (parent.Window.ClientBounds != _windowSize)
            {
                _windowSize = parent.Window.ClientBounds;
                windowResized(_windowSize);
            }
            if (newKeyboardState.IsKeyDown(Keys.Escape) && !_keyboardState.IsKeyDown(Keys.Escape))
                parent.SwitchScene(Scene.MainMenu);

            _keyboardState = newKeyboardState;
        }

        public override void Activation(Game1 parent)
        {
            for (int i = 0; i < animations.Count; i++)
            {
                try
                {
                    ((AnimatedSprite)animations[i]).ActualPicture = 1;
                }
                catch (Exception)
                {
                }
            }
            parent.son.Play(Musiques.CreditMusic);
        }

        public void ResetAuthors()
        {
            _authors.setRelatvePos(new Rectangle(_authors.Position.X, _windowSize.Height, _authors.Position.Width, _authors.Position.Height));
        }

        public override void EndScene(Game1 parent)
        {
            parent.son.Stop();
        }

        private void windowResized(Rectangle rect)
        {
            foreach (Sprite s in animations)
                s.windowResized(rect);
            _authors.windowResized(rect);
            _particles.WindowResized(rect);
        }
    }
}
