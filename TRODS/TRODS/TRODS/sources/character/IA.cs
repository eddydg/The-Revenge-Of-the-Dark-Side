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
    class IA : AbstractScene
    {
        /// <summary>
        /// Liste des comportements que l'IA peut adopter
        /// </summary>
        public enum Behavior
        {
            Passive, Aggressive
        }

        private Behavior _behavior;
        public Behavior Behavior1
        {
            get { return _behavior; }
            set { _behavior = value; }
        }
        private Random _rand;

        public IA(Behavior b = Behavior.Passive)
        {
            _behavior = b;
            _rand = new Random();
        }

        public override void Update(float elapsedTime)
        {
        }
        public override void Activation(Game1 parent)
        {
        }
        public override void EndScene(Game1 parent)
        {
        }
    }
}
