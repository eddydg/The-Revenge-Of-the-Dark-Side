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
        private List<AnimatedSprite> animations;
        private ParticleEngine _particles;

        public SceneCredit(Rectangle windowSize, KeyboardState keyboardState, MouseState mouseState)
        {
            _windowSize = windowSize;
            _keyboardState = keyboardState;

            animations = new List<AnimatedSprite>();
            animations.Add(new AnimatedSprite(new Rectangle(80, 250, 150, 70), _windowSize, "menu/beenTeam"));
            animations.Add(new AnimatedSprite(new Rectangle(394, _windowSize.Height, 280, 130), _windowSize, "menu/authors"));
            animations.Last<AnimatedSprite>().Direction = new Vector2(0, -1);
            animations.Last<AnimatedSprite>().Vitesse = 0.1f;
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
            foreach (AnimatedSprite s in animations)
                s.LoadContent(content);
            _particles.LoadContent(content);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            _particles.Draw(spriteBatch);
            foreach (AnimatedSprite s in animations)
                s.Draw(spriteBatch);
            spriteBatch.End();
        }

        public override void Update(float elapsedTime)
        {
            foreach (AnimatedSprite s in animations)
                s.Update(elapsedTime);
            Rectangle p = animations.ElementAt<AnimatedSprite>(1).Position;
            if (p.Y < -p.Height)
            {
                p.Y = _windowSize.Height;
                animations.ElementAt<AnimatedSprite>(1).Position = p;
            }
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
            foreach (AnimatedSprite s in animations)
                s.ActualPicture = 1;
            parent.son.Play(Musiques.CreditMusic);
        }

        public override void EndScene(Game1 parent)
        {
            parent.son.Stop();
        }

        /// <summary>
        /// Fonction adaptant les textures au
        /// redimensionnement de la fenetre
        /// </summary>
        /// <param name="rect">Nouvelle dimension de la fenetre obtenue par *Game1*.Window.ClientBounds()</param>
        private void windowResized(Rectangle rect)
        {
            foreach (AnimatedSprite s in animations)
                s.windowResized(rect);
            _particles.WindowResized(rect);
        }
    }
}
