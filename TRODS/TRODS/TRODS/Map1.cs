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
    class Map1 : AbstractMap
    {
        public Map1(Rectangle windowSize) : base (windowSize)
        {
        }

        public override void LoadContent(ContentManager content)
        {
            _elementsBackground.Add(new Sprite(new Rectangle(0, 0, _windowSize.Width, _windowSize.Height), _windowSize.Width, _windowSize.Height));
            _elementsBackground.Last<Sprite>().LoadContent(content, "black");
            _elementsBackground.Add(new Sprite(new Rectangle(0, 0, _windowSize.Width*2, _windowSize.Height), _windowSize.Width, _windowSize.Height));
            _elementsBackground.Last<Sprite>().LoadContent(content, "menuTextPlay");
            _elementsBackground.Last<Sprite>().Vitesse = 0.01f;
            
            _elementsMainground.Add(new Sprite(new Rectangle(0, 0, _windowSize.Width , _windowSize.Height ), _windowSize.Width, _windowSize.Height));
            _elementsMainground.Last<Sprite>().LoadContent(content, "menuWallpaperText");
            _elementsMainground.Last<Sprite>().Vitesse = 0.1f;
        }
    }
}
