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

        private Vector4 speedRange;
        private Vector2 angleRange;
        private Vector2 angularSpeedRange;
        private Vector2 scaleRange;
        private Vector2 lifeTimeRange;

        public ParticleEngine(Rectangle emitterLocation, Vector4 speedRange, Vector2 angleRange,
                                Vector2 angularSpeedRange, Vector2 scaleRange, Vector2 lifeTimeRange)
        {
            random = new Random();
            EmitterLocation = emitterLocation;
            particles = new List<Particle>();
            textures = new List<Texture2D>();

            this.speedRange = speedRange;
            this.angleRange = angleRange;
            this.angularSpeedRange = angularSpeedRange;
            this.scaleRange = scaleRange;
            this.lifeTimeRange = lifeTimeRange;
        }

        private Particle GenerateParticle()
        {
            Texture2D texture = textures[random.Next(textures.Count)];
            Vector2 position = new Vector2(random.Next(EmitterLocation.X, EmitterLocation.X + EmitterLocation.Width + 1),
                                         random.Next(EmitterLocation.Y, EmitterLocation.Y + EmitterLocation.Height + 1));
            /*Vector2 speed = new Vector2(
                    (float)(random.NextDouble() * random.Next((int)speedRange.X, (int)speedRange.Y+1)),
                    (float)(random.NextDouble() * random.Next((int)speedRange.Z, (int)speedRange.W + 1)));*/
            Vector2 speed = new Vector2(
                            1f * (float)(random.NextDouble() * 2 - 1),
                            1f * (float)(random.NextDouble() * 2 - 1));
            float angle = (float)random.NextDouble() * (float)random.Next((int)angleRange.X, (int)angleRange.Y + 1);
            float angularSpeed = 0.1f * (float)(random.NextDouble() * 1.25f - 1);
            Color color = new Color(
                random.Next(0, 256),
                random.Next(0, 256),
                random.Next(0, 256),
                0);
            float size = (float)random.NextDouble() * (float)(random.Next((int)scaleRange.X, (int)scaleRange.Y + 1));
            int lifeTime = random.Next((int)lifeTimeRange.X, (int)lifeTimeRange.Y + 1);

            return new Particle(texture, position, speed, angle, angularSpeed, color, size, lifeTime);
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
