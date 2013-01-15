using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;


namespace TRODS
{
    class Character
    {
        public AnimatedSprite _character;
        Vector3[] action;
        public State _state { get; set; }
        public int Life;
        private const int NB_ELEMENTS_IN_ENUM_ACTION = 6;

        public enum Action
        {
            StopR,StopL,RunR,RunL,JumpR,JumpL
        }
        public enum State
        {
            Stop,IsRunningR,IsRunningL, IsJumpingR, IsJumpingL
        }

        public Character(AnimatedSprite s,
            Vector3 stopR = new Vector3(), Vector3 stopL = new Vector3(),
            Vector3 runR = new Vector3(), Vector3 runL = new Vector3(),
            Vector3 jumpR = new Vector3(), Vector3 jumpL = new Vector3())
        {
            _character = new AnimatedSprite(s);
            action = new Vector3[NB_ELEMENTS_IN_ENUM_ACTION];
            action[(int)Action.StopR] = stopR;
            action[(int)Action.StopL] = stopL;
            action[(int)Action.RunR] = runR;
            action[(int)Action.RunL] = runL;
            action[(int)Action.JumpR] = jumpR;
            action[(int)Action.JumpL] = jumpL;
            _state = State.Stop;
            Life = 100;
        }

        public void Update(float elapsedTime)
        {
            _character.Update(elapsedTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _character.Draw(spriteBatch);
        }

        public void LoadContent(ContentManager content)
        {
            _character.LoadContent(content);
        }

        public void RunR(int vitesse)
        {
            _character.SetPictureBounds((int)action[(int)Action.RunR].X, (int)action[(int)Action.RunR].Y, (int)action[(int)Action.RunR].Z, true);
            _character.Speed = vitesse;
            _state = State.IsRunningR;
        }

        public void RunL(int vitesse)
        {
            _character.SetPictureBounds((int)action[(int)Action.RunL].X, (int)action[(int)Action.RunL].Y, (int)action[(int)Action.RunL].Z, true);
            _character.Speed = vitesse;
            _state = State.IsRunningL;
        }

        public void JumpR(int vitesse)
        {
            _character.SetPictureBounds((int)action[(int)Action.JumpR].X, (int)action[(int)Action.JumpR].Y, (int)action[(int)Action.JumpR].Z, true);
            _character.Speed = vitesse;
            _state = State.IsJumpingR;
        }
        public void JumpL(int vitesse)
        {
            _character.SetPictureBounds((int)action[(int)Action.JumpL].X, (int)action[(int)Action.JumpL].Y, (int)action[(int)Action.JumpL].Z, true);
            _character.Speed = vitesse;
            _state = State.IsJumpingL;
        }

        public void Stop(int vitesse)
        {
            if (_state == State.IsRunningL)
                _character.SetPictureBounds((int)action[(int)Action.StopL].X, (int)action[(int)Action.StopL].Y, (int)action[(int)Action.StopL].Z, true);
            else
                _character.SetPictureBounds((int)action[(int)Action.StopR].X, (int)action[(int)Action.StopR].Y, (int)action[(int)Action.StopR].Z, true);
            _character.Speed = vitesse;
            _state = State.Stop;
        }

        public void Heart(int lifePOint)
        {
            Life -= lifePOint;
        }
    }
}
