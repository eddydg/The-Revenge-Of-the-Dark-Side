﻿using System;
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
        public enum KeysActions
        {
            WalkRight, WalkLeft, WalkUp, WalkDown, Jump,
            Attack1
        };

        private InputManager<KeysActions, Keys> _inputManager;

        public Personnage(Rectangle winsize, Vector2 position)
            : base(winsize, position, 140, 190, @"game\perso", 15, 4)
        {
            _graphicalBounds = new GraphicalBounds<CharacterActions>(new Dictionary<CharacterActions, Rectangle>());
            _graphicalBounds.set(CharacterActions.WalkRight, 3, 6, 15);
            _graphicalBounds.set(CharacterActions.WalkLeft, 18, 21, 30);
            _graphicalBounds.set(CharacterActions.StandRight, 1, 1, 2, 4);
            _graphicalBounds.set(CharacterActions.StandLeft, 16, 16, 17, 4);
            _graphicalBounds.set(CharacterActions.JumpRight, 31, 31, 35);
            _graphicalBounds.set(CharacterActions.JumpLeft, 36, 36, 40);
            _graphicalBounds.set(CharacterActions.Attack1Right, 41, 41, 49, 35);
            _graphicalBounds.set(CharacterActions.Attack1Left, 50, 50, 58, 35);
            _action = CharacterActions.StandRight;
            _physics.MaxHeight = 400;
            _physics.TimeOnFlat = 500;
            _inputManager = new InputManager<KeysActions, Keys>();
            _weapon = new Weapon(winsize,@"game/weapon", _sprite.Lignes, _sprite.Colonnes, _sprite.Position.Width, _sprite.Position.Height);
            InitKeys();
            actualizeSpriteGraphicalBounds();
            actualizeSpritePosition();
            Jump();
        }

        public override void HandleInput(KeyboardState newKeyboardState, MouseState newMouseState, Game1 parent)
        {
            if (_canMove)
            {
                if (!_jumping)
                {
                    if (newKeyboardState.IsKeyDown(_inputManager.Get(KeysActions.WalkRight)))
                        Move(true);
                    else if (newKeyboardState.IsKeyDown(_inputManager.Get(KeysActions.WalkLeft)))
                        Move(false);
                    else if (newKeyboardState.IsKeyDown(_inputManager.Get(KeysActions.WalkUp)))
                        Move(_direction);
                    else if (newKeyboardState.IsKeyDown(_inputManager.Get(KeysActions.WalkDown)))
                        Move(_direction);
                    else
                        Stand(_direction);
                    if (newKeyboardState.IsKeyDown(_inputManager.Get(KeysActions.Jump)))
                        Jump();
                }
                if (newKeyboardState.IsKeyDown(_inputManager.Get(KeysActions.Attack1)))
                {
                    _canMove = false;
                    _timer = 400;
                    _action = _direction ? CharacterActions.Attack1Right : CharacterActions.Attack1Left;
                    actualizeSpriteGraphicalBounds();
                }
            }
        }

        public void InitKeys()
        {
            _inputManager.Add(KeysActions.WalkRight, Keys.Right);
            _inputManager.Add(KeysActions.WalkLeft, Keys.Left);
            _inputManager.Add(KeysActions.WalkUp, Keys.Up);
            _inputManager.Add(KeysActions.WalkDown, Keys.Down);
            _inputManager.Add(KeysActions.Jump, Keys.Space);
            _inputManager.Add(KeysActions.Attack1, Keys.X);
        }
    }
}
