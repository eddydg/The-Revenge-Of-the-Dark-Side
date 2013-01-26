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
    class ParticleEngine : AbstractScene
    {
        private Random random;
        private Rectangle EmitterLocation;
        private List<Particle> particles;
        private List<Texture2D> textures;
        public Color Color { get; set; }
        public int NbNewParticle { get; set; }
        public List<string> AssetNames { get; set; }

        private Vector4 speedRange;
        private Vector2 angleRange;
        private Vector2 angularSpeedRange;
        private Vector2 scaleRange;
        private Vector2 lifeTimeRange;
        private int[] colorRange;
        private Rectangle windowSize;

        /// <summary>
        /// Constructeur.
        /// </summary>
        /// <param name="emitterLocation">définit la zone d'apparition des particules</param>
        public ParticleEngine(Rectangle _windowSize,
                              Rectangle emitterLocation,
                              List<string> assetNames = null,
                              int nbNewParticle = 10,
                              float vitesseMin = 0f, float vitesseMax = 2f, float directionAngle = 0f, float directionAngleVariation = 180f,
                              float initialAngleMin = 0f, float initialAngleMax = 0f,
                              float vitesseRotationMin = -2, float vitesseRotationMax = 2,
                              float sizeMin = 0.2f, float sizeMax = 2f,
                              float lifeTimeMin = 20f, float lifeTimeMax = 100f)
        {
            windowSize = _windowSize;
            random = new Random();
            EmitterLocation = emitterLocation;
            AssetNames = assetNames;
            particles = new List<Particle>();
            textures = new List<Texture2D>();
            NbNewParticle = nbNewParticle;

            this.speedRange = new Vector4(vitesseMin, vitesseMax, directionAngle, directionAngleVariation);
            this.angleRange = new Vector2(initialAngleMin, initialAngleMax);
            this.angularSpeedRange = new Vector2(vitesseRotationMin, vitesseRotationMax);
            this.scaleRange = new Vector2(sizeMin, sizeMax);
            this.lifeTimeRange = new Vector2(lifeTimeMin, lifeTimeMax);
            this.colorRange = new int[8] { 255, 255, 255, 255, 255, 255, 0, 70 };
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (Particle p in particles)
                p.Draw(spriteBatch);
        }
        public override void LoadContent(ContentManager content)
        {
            foreach (string s in AssetNames)
                textures.Add(content.Load<Texture2D>(s));
        }
        public override void Update(float elapsedTime)
        {
            for (int i = 0; i < NbNewParticle; i++)
            {
                particles.Add(GenerateParticle());
            }

            for (int i = 0; i < particles.Count; i++)
            {
                particles[i].Update(elapsedTime);
                if (particles[i].LifeTime <= 0)
                {
                    particles.RemoveAt(i);
                    i--;
                }
            }
        }
        public override void WindowResized(Rectangle rect)
        {
            double xRapport = rect.Width / windowSize.Width;
            double yRapport = rect.Height / windowSize.Height;

            foreach (Particle p in particles)
                p.WindowResized(xRapport, yRapport);

            EmitterLocation.X = (int)(EmitterLocation.X * xRapport);
            EmitterLocation.Y = (int)(EmitterLocation.Y * yRapport);
            EmitterLocation.Width = (int)(EmitterLocation.Width * xRapport);
            EmitterLocation.Height = (int)(EmitterLocation.Height * yRapport);

            speedRange.X *= (float)xRapport;
            speedRange.X *= (float)xRapport;

            windowSize = rect;
        }
        /// <summary>
        /// Génère une nouvelle particule "aléatoire" en fonction des propriété de l'objet.
        /// </summary>
        /// <returns>retourne la particule généré.</returns>
        private Particle GenerateParticle()
        {
            Texture2D texture = textures[random.Next(textures.Count)];
            Vector2 position = new Vector2(random.Next(EmitterLocation.X, EmitterLocation.X + EmitterLocation.Width + 1),
                                         random.Next(EmitterLocation.Y, EmitterLocation.Y + EmitterLocation.Height + 1));
            //definie la vitesse et la direction de la particule
            float v = speedRange.X + (float)random.NextDouble() * (float)(random.Next((int)speedRange.Y - (int)speedRange.X + 1));
            float alpha = -((float)random.Next((int)speedRange.Z - (int)speedRange.W, (int)speedRange.Z + (int)speedRange.W + 1)) * (float)Math.PI / 180f;
            Vector2 speed = new Vector2(v * (float)Math.Cos(alpha), v * (float)Math.Sin(alpha));
            float angle = ((float)random.Next((int)angleRange.X, (int)angleRange.Y + 1)) * (float)Math.PI / 180f;
            float angularSpeed = ((float)random.Next((int)angularSpeedRange.X, (int)angularSpeedRange.Y + 1)) * (float)Math.PI / 180f;
            Color = new Color(
                random.Next(colorRange[0], colorRange[1] + 1),
                random.Next(colorRange[2], colorRange[3] + 1),
                random.Next(colorRange[4], colorRange[5] + 1),
                random.Next(colorRange[6], colorRange[7] + 1));
            float size = (float)random.NextDouble() * (float)(random.Next((int)scaleRange.X, (int)scaleRange.Y + 1));
            int lifeTime = random.Next((int)lifeTimeRange.X, (int)lifeTimeRange.Y + 1);

            return new Particle(texture,new Rectangle((int)position.X,(int)position.Y,20,20), speed, angle, angularSpeed, Color, size, lifeTime);
        }
        /// <summary>
        /// définit l'intervalle de couleur des particules en fonction des valeur RGB et Alpha
        /// </summary>
        /// <param name="rMin">rouge minimum</param>
        /// <param name="rMax">rouge maximum</param>
        /// <param name="gMin">vert minimum</param>
        /// <param name="gMax">vert maximum</param>
        /// <param name="bMin">bleu minimum</param>
        /// <param name="bMax">bleu maximum</param>
        /// <param name="aMin">alpha minimum</param>
        /// <param name="aMax">alpha maximum</param>
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
    }

    class Particle : AbstractScene
    {
        //propriétés des particules
        Texture2D Texture { get; set; }
        private Rectangle position;
        public Rectangle Position
        {
            get { return Position; }
            set { Position = value; }
        }
        private Vector2 speed;
        public Vector2 Speed
        {
            get { return speed; }
            set { speed = value; }
        }
        public float Angle { get; set; }
        public float AngularSpeed { get; set; }
        public Color Color { get; set; }
        private float size;
        public float Size
        {
            get { return size; }
            set { size = value; }
        }
        public int LifeTime { get; set; }

        public Particle(Texture2D texture, Rectangle position, Vector2 speed, float angle, float angularSpeed,
                        Color color, float size, int lifeTime)
        {
            Texture = texture;
            this.position = position;
            this.speed = speed;
            Angle = angle;
            AngularSpeed = angularSpeed;
            Color = color;
            this.size = size;
            LifeTime = lifeTime;
        }
        public override void Update(float elapsedTime)
        {
            LifeTime -= (int)(elapsedTime / 16);
            position.X += (int)(Speed.X * (elapsedTime / 16));
            position.Y += (int)(Speed.Y * (elapsedTime / 16));
            Angle += AngularSpeed * (elapsedTime / 16);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            /*spriteBatch.Draw(Texture, Position, null, Color.FromNonPremultiplied(Color.R,Color.G,Color.B,Color.A), Angle,
                new Vector2(Texture.Width / 2, Texture.Height / 2), Size, SpriteEffects.None, 0f);*/
            spriteBatch.Draw(Texture,new Vector2(position.X,position.Y), null, Color, Angle,
                new Vector2(position.Width / 2, position.Height / 2), Size, SpriteEffects.None, 0f);
        }
        public void WindowResized(double x, double y)
        {
            position.X = (int)(Position.X * x);
            position.Y = (int)(Position.Y * y);
            position.Width = (int)(Position.Width * x);
            position.Height = (int)(Position.Height * y);
            speed.X *= (float)x;
            speed.Y *= (float)y;
            size *= (float)x;
        }
    }
}
