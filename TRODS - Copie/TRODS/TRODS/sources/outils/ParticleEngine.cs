using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TRODS
{
    class ParticleEngine
    {
        private Random random;
        public Rectangle EmitterLocation { get; set; }
        private List<Particle> particles;
        private List<Texture2D> textures;
        public Color Color;

        private Vector4 speedRange;
        private Vector2 angleRange;
        private Vector2 angularSpeedRange;
        private Vector2 scaleRange;
        private Vector2 lifeTimeRange;
        private int[] colorRange;

        public ParticleEngine(Rectangle emitterLocation)
        {
            random = new Random();
            EmitterLocation = emitterLocation;
            particles = new List<Particle>();
            textures = new List<Texture2D>();

            this.speedRange = new Vector4(0.2f, 2, 0, 180);
            this.angleRange = new Vector2();
            this.angularSpeedRange = new Vector2();
            this.scaleRange = new Vector2(1, 1);
            this.lifeTimeRange = new Vector2(20, 100);
            this.colorRange = new int[8]{255,255,255,255,255,255,0,70};

            this.Color = Color.White;
        }

        private Particle GenerateParticle()
        {
            Texture2D texture = textures[random.Next(textures.Count)];
            Vector2 position = new Vector2(random.Next(EmitterLocation.X, EmitterLocation.X + EmitterLocation.Width + 1),
                                         random.Next(EmitterLocation.Y, EmitterLocation.Y + EmitterLocation.Height + 1));
            //definie la vitesse et la direction de la particule
            float v = speedRange.X + (float)random.NextDouble() * (float)(random.Next((int)speedRange.Y - (int)speedRange.X + 1));
            float alpha = -((float)random.Next((int)speedRange.Z - (int)speedRange.W, (int)speedRange.Z + (int)speedRange.W + 1)) * (float)Math.PI / 180f;
            Vector2 speed = new Vector2(
                            v * (float)Math.Cos(alpha),
                            v * (float)Math.Sin(alpha));
            float angle = ((float)random.Next((int)angleRange.X, (int)angleRange.Y + 1)) * (float)Math.PI / 180f;
            float angularSpeed = ((float)random.Next((int)angularSpeedRange.X, (int)angularSpeedRange.Y + 1)) * (float)Math.PI / 180f;
            Color = new Color(
                random.Next(colorRange[0], colorRange[1] + 1),
                random.Next(colorRange[2], colorRange[3] + 1),
                random.Next(colorRange[4], colorRange[5] + 1),
                random.Next(colorRange[6], colorRange[7] + 1));
            float size = (float)random.NextDouble() * (float)(random.Next((int)scaleRange.X, (int)scaleRange.Y + 1));
            int lifeTime = random.Next((int)lifeTimeRange.X, (int)lifeTimeRange.Y + 1);

            return new Particle(texture, position, speed, angle, angularSpeed, Color, size, lifeTime);
        }

        public void SetSpeedRange(float vMin, float vMax, float angle, float angleVariation)
        {
            speedRange = new Vector4(vMin, vMax, angle, angleVariation);
        }

        public void SetAngleRange(float min, float max)
        {
            angleRange = new Vector2(min, max);
        }

        public void SetAngularSpeedRange(float min, float max)
        {
            angularSpeedRange = new Vector2(min, max);
        }

        public void SetScaleRange(float min, float max)
        {
            scaleRange = new Vector2(min, max);
        }

        public void SetLifeTimeRange(float min, float max)
        {
            lifeTimeRange = new Vector2(min, max);
        }

        public void SetColorRange(int rMin, int rMax, int gMin, int gMax, int bMin, int bMax, int aMin, int aMax)
        {
            colorRange[0] = rMin;
            colorRange[1] = rMax;
            colorRange[2] = gMin;
            colorRange[3] = gMax;
            colorRange[4] = bMin;
            colorRange[5] = bMax;
            colorRange[6] = aMin;
            colorRange[7] = aMax;
        }

        public void LoadContent(ContentManager content, List<string> assetNames)
        {
            foreach (string s in assetNames)
                textures.Add(content.Load<Texture2D>(s));
        }

        public void Update(int nbNewParticle = 10)
        {
            for (int i = 0; i < nbNewParticle; i++)
            {
                particles.Add(GenerateParticle());
            }

            for (int i = 0; i < particles.Count; i++)
            {
                particles[i].Update();
                if (particles[i].LifeTime <= 0)
                {
                    particles.RemoveAt(i);
                    i--;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Particle p in particles)
                p.Draw(spriteBatch);
        }
    }
}
