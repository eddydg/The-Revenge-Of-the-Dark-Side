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
        public Personnage(Rectangle winsize, Vector2 position)
            : base(winsize, position, 140, 190, @"game\perso", 5, 8, new Vector2(0.7f, 1f))
        {
            _graphicalBounds = new GraphicalBounds(new Dictionary<Actions, Vector3>());
            _graphicalBounds.set(Actions.WalkRight, 3, 15, 15);
            _graphicalBounds.set(Actions.WalkLeft, 20, 25, 30);
            _graphicalBounds.set(Actions.StandRight, 1, 1, 2);
            _graphicalBounds.set(Actions.StandLeft, 16, 16, 17);
            _graphicalBounds.set(Actions.JumpRight, 31, 31, 35);
            _graphicalBounds.set(Actions.JumpLeft, 36, 36, 40);
            _action = Actions.StandRight;
            _maxJumpHeight = 400;
            actualizeSpriteGraphicalBounds();
            actualizeSpritePosition();
        }

        public override void HandleInput(KeyboardState newKeyboardState, MouseState newMouseState, Game1 parent)
        {
            if (oldKeyboardState != newKeyboardState)//gestion des entrées clavier.
            {
                if (_canMove)
                {
                    if (newKeyboardState.IsKeyDown(Keys.Right) && oldKeyboardState.IsKeyUp(Keys.Right))//apuie a droite
                        Move(true);
                    else if (newKeyboardState.IsKeyDown(Keys.Left) && oldKeyboardState.IsKeyUp(Keys.Left))//appuie a gauche
                        Move(false);
                    else
                    {
                        if (!_jumping)
                        {
                            if (_direction)
                                _action = Actions.StandRight;
                            else
                                _action = Actions.StandLeft;
                            actualizeSpriteGraphicalBounds();
                        }
                    }
                    if (newKeyboardState.IsKeyDown(Keys.Space) && oldKeyboardState.IsKeyUp(Keys.Space))//appuie sur espace.
                    {
                        if (!_jumping)
                            Jump();
                    }
                }
            }
            oldKeyboardState = newKeyboardState;
            oldMouseState = newMouseState;
        }
    }
}
