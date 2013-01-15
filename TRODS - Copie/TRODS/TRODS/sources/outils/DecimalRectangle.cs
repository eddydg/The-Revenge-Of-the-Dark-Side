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

namespace TRODS.sources.outils
{
    class DecimalRectangle
    {
        public float X, Y, W, H;

        public DecimalRectangle(float x = 0, float y = 0, float w = 0, float h = 0)
        {
            X = x;
            Y = y;
            W = w;
            H = h;
        }

        public void SetValues(float x = 0, float y = 0, float w = 0, float h = 0)
        {
            X = x;
            Y = y;
            W = w;
            H = h;
        }

        public Vector2 Position()
        {
            return new Vector2(X, Y);
        }
    }
}
