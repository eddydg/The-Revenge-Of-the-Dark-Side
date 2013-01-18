using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TRODS
{
    class Particle
    {
        //propriétés des particules
        Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Speed { get; set; }
        public float Angle { get; set; }
        public float AngularSpeed { get; set; }
        public Color Color { get; set; }
        public float Size { get; set; }
        public int LifeTime { get; set; }

        public Particle(Texture2D texture, Vector2 position, Vector2 speed, float angle, float angularSpeed,
                        Color color, float size, int lifeTime)
        {
            Texture = texture;
            Position = position;
            Speed = speed;
            Angle = angle;
            AngularSpeed = angularSpeed;
            Color = color;
            Size = size;
            LifeTime = lifeTime;
        }


        public void Update()
        {
            LifeTime--;
            Position += Speed;
            Angle += AngularSpeed;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color, Angle,
                new Vector2(Texture.Width / 2, Texture.Height / 2), Size, SpriteEffects.None,0f);
        }
    }
}
