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
    class VideoReader : AbstractScene
    {
        private VideoPlayer _player;
        private Video _video;
        private string _assetName;
        private Rectangle _windowSize;
        private Scene _next;
        private int _timer;

        public VideoReader(string assetname, Scene next = Scene.Titre)
        {
            this._assetName = assetname;
            this._player = new VideoPlayer();
            this._next = next;
            this._windowSize = new Rectangle();
            this._timer = 0;
        }

        public override void LoadContent(ContentManager content)
        {
            this._video = content.Load<Video>(this._assetName);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            if (this._player.State == MediaState.Playing)
            {
                Texture2D texture = this._player.GetTexture();
                if (texture != null)
                    spriteBatch.Draw(texture, this._windowSize, Color.White);
                if (this._player.PlayPosition == this._video.Duration)
                    this._player.Stop();
            }
            spriteBatch.End();
        }

        public override void HandleInput(KeyboardState newKeyboardState, MouseState newMouseState, Game1 parent)
        {
            if (this._timer < 0 && (newKeyboardState.GetPressedKeys().Length > 0 || this._player.State != MediaState.Playing))
            {
                this._player.Stop();
                parent.SwitchScene(this._next);
            }
            this._windowSize = new Rectangle(0, 0, parent.Window.ClientBounds.Width, parent.Window.ClientBounds.Height);
        }

        public override void Update(float elapsedTime)
        {
            this._timer -= (int)elapsedTime;
        }

        public override void Activation(Game1 parent = null)
        {
            if (this._video != null)
                this._player.Play(this._video);
            if (parent != null)
                this._windowSize = new Rectangle(0, 0, parent.Window.ClientBounds.Width, parent.Window.ClientBounds.Height);
            this._timer = 400;
        }
    }
}
