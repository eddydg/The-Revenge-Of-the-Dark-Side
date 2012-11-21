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
    abstract class AbstractScene
    {
        public virtual void LoadContent(ContentManager content)
        {
        }
        public virtual void HandleInput(KeyboardState newKeyboardState, MouseState newMouseState, Game1 parent)
        {
        }
        public virtual void Update(float elapsedTime)
        {
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
        }
    }
}
