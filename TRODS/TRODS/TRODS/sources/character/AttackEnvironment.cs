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
    class AttackEnvironment : AbstractScene
    {
        public enum Attacks
        {
            SwordAttack, HeadAttack
        }

        private List<Attac> _attackList;
        private Dictionary<Attacks, Texture2D> _textures;
        

        public AttackEnvironment()
        {
            _attackList = new List<Attac>();
            _textures = new Dictionary<Attacks, Texture2D>();
        }

        public void Add(Attacks a, Rectangle pos)
        {
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            foreach (Attac att in _attackList)
                att.Draw(spriteBatch);
        }

        public override void Update(float elapsedTime)
        {
            for (int i = 0; i < _attackList.Count; i++)
            {
                if (_attackList[i].Over())
                {
                    _attackList.RemoveAt(i);
                    i--;
                }
                else
                    _attackList[i].Update(elapsedTime);
            }
        }

        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            base.LoadContent(content);
        }
    }
}
