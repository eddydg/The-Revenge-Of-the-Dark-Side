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
    class InputManager<K, V>
    {
        private Dictionary<K, V> _array;
        public Dictionary<K, V> Array
        {
            get { return _array; }
            private set { _array = value; }
        }

        public InputManager()
        {
            _array = new Dictionary<K, V>();
        }

        public void Add(K k, V a)
        {
            _array.Add(k, a);
        }
        public bool Contain(K k)
        {
            return _array.ContainsKey(k);
        }
        public V Get(K k)
        {
            return _array[k];
        }
        public void  Remove(K k)
        {
            if (Contain(k))
                _array.Remove(k);
        }
    }
}
