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
    class Personnage : Character
    {
        public Personnage(Rectangle winsize,Vector2 position)
            :base(winsize,position,50,100,@"game\perso",5,6,new Vector2(0.7f,1f))
        {
            _graphicalBounds = new GraphicalBounds(new Dictionary<Actions, Vector3>());
            _graphicalBounds.set(Actions.WalkRight, 1, 5, 15);
            _graphicalBounds.set(Actions.WalkLeft, 16, 20, 30);
            _action = Actions.StandRight;
        }

        public override void HandleInput(KeyboardState newKeyboardState, MouseState newMouseState, Game1 parent)
        {
            if (newKeyboardState.IsKeyDown(Keys.Right))
                Move(true);
            if (newKeyboardState.IsKeyDown(Keys.Left))
                Move(false);
        }
    }
}
