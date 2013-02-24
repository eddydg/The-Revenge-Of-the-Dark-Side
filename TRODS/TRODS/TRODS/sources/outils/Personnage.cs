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
        private KeyboardState oldKeyboardState;
        private MouseState oldMouseState;
        public Personnage(Rectangle winsize,Vector2 position)
            :base(winsize,position,140,190,@"game\perso",5,8,new Vector2(0.7f,1f))
        {
            _graphicalBounds = new GraphicalBounds(new Dictionary<Actions, Vector3>());
            _graphicalBounds.set(Actions.WalkRight, 1, 6, 15);
            _graphicalBounds.set(Actions.WalkLeft, 16, 21, 30);
            _graphicalBounds.set(Actions.StandRight, 1, 1, 2);
            _graphicalBounds.set(Actions.StandLeft, 16, 16, 17);
            _action = Actions.StandRight;
        }

        public override void HandleInput(KeyboardState newKeyboardState, MouseState newMouseState, Game1 parent)
        {
            if (oldKeyboardState != newKeyboardState)//gestion des entrées clavier.
            {
                if (newKeyboardState.IsKeyDown(Keys.Right) && oldKeyboardState.IsKeyUp(Keys.Right))//apuie a droite
                {
                    _action = Actions.WalkRight;
                }
                if (newKeyboardState.IsKeyDown(Keys.Left) && oldKeyboardState.IsKeyUp(Keys.Left))//appuie a gauche
                {
                    _action = Actions.WalkLeft;
                }
                if (newKeyboardState.IsKeyUp(Keys.Right) && newKeyboardState.IsKeyUp(Keys.Left)) //appuie ni a droite ni a gauche(on s'arrete)
                {
                    _action = _action == Actions.WalkRight ? Actions.StandRight : Actions.StandLeft;
                }
                if (newKeyboardState.IsKeyDown(Keys.Space) && oldKeyboardState.IsKeyUp(Keys.Space))//appuie sur espace.
                {
                    _action = Actions.Jump;
                }
                Move(true);//effectue le mouvement en fonction de _action
            }
            if (oldMouseState != newMouseState)//gestion des entrées souris.
            {
            }
            oldKeyboardState = newKeyboardState;
            oldMouseState = newMouseState;
        }
    }
}
