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
    class Attac
    {
        private Rectangle _portee;
        public Rectangle Portee
        {
            get { return _portee; }
            set { _portee = value; }
        }
        public int _duree { get; set; }

        public Attac(int duree = 0, Rectangle portee = new Rectangle())
        {
            _duree = duree;
            _portee = portee;
        }
        public void Update(float elapsedTime)
        {
            _duree -= (int)elapsedTime;
        }
    }
}
