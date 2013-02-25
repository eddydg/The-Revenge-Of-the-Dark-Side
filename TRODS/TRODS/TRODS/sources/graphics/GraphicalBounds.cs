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
    class GraphicalBounds<T>
    {
        Dictionary<T, Rectangle> _boundList;
        public Dictionary<T, Rectangle> BoundList
        {
            get { return _boundList; }
            private set { _boundList = value; }
        }

        public GraphicalBounds(Dictionary<T, Rectangle> boundList)
        {
            this._boundList = boundList;
        }

        public void set(T ac, int first, int firstRepeat, int last, int speed = 30)
        {
            if (BoundList.ContainsKey(ac))
                BoundList.Remove(ac);
            BoundList.Add(ac, new Rectangle(first, firstRepeat, last, speed));
        }
        public void set(T a, Rectangle r)
        {
            if (BoundList.ContainsKey(a))
                BoundList.Remove(a);
            BoundList.Add(a, r);
        }
        public Rectangle get(T a)
        {
            Rectangle r = new Rectangle();
            BoundList.TryGetValue(a, out r);
            return r;
        }
        public bool Contains(T a)
        {
            return BoundList.ContainsKey(a);
        }
        public void Remove(T a)
        {
            if (Contains(a))
                _boundList.Remove(a);
        }
    }
}
