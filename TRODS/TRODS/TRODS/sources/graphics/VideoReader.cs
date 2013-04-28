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
        private String _assetName;
        private Rectangle _windowSize;
        private Scene _next;

        public VideoReader(string assetname, Scene next = Scene.Titre)
        {
            _assetName = assetname;
            _player = new VideoPlayer();
            _next = next;
            _windowSize = new Rectangle();
        }

        public override void LoadContent(ContentManager content)
        {
            _video = content.Load<Video>(_assetName);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            if (_player.State == MediaState.Playing)
            {
                Texture2D tex = _player.GetTexture();
                if (tex != null)
                    spriteBatch.Draw(tex, _windowSize, Color.White);
                if (_player.PlayPosition == _video.Duration)
                {
                    _player.Stop();
                }
            }
            spriteBatch.End();
        }

        public override void HandleInput(KeyboardState newKeyboardState, MouseState newMouseState, Game1 parent)
        {
            if (newKeyboardState.GetPressedKeys().Length > 0 || _player.State != MediaState.Playing)
            {
                _player.Stop();
                parent.SwitchScene(_next);
            }
            _windowSize = new Rectangle(0, 0, parent.Window.ClientBounds.Width, parent.Window.ClientBounds.Height);
        }

        public override void Activation(Game1 parent = null)
        {
            if (_video != null)
                _player.Play(_video);
            if (parent != null)
                _windowSize = new Rectangle(0, 0, parent.Window.ClientBounds.Width, parent.Window.ClientBounds.Height);
        }
    }
}
