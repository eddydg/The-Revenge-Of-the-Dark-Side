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
        public Rectangle EmitterLocation { get; set; }
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

        /// <summary>
        /// Constructeur...
        /// </summary>
        /// <param name="emitterLocation">définit la zone d'apparition des particules</param>
        public ParticleEngine(Rectangle emitterLocation,
                              List<string> assetNames = null,
                              int nbNewParticle = 10,
                              float vitesseMin = 0f, float vitesseMax = 2f, float directionAngle = 0f, float directionAngleVariation = 180f,
                              float initialAngleMin = 0f, float initialAngleMax = 0f,
                              float vitesseRotationMin = -2, float vitesseRotationMax = 2,
                              float sizeMin = 0.2f, float sizeMax = 2f,
                              float lifeTimeMin = 20f, float lifeTimeMax = 100f)
        {
            random = new Random();
            EmitterLocation = emitterLocation;
            AssetNames = assetNames;
            particles = new List<Particle>();
            textures = new List<Texture2D>();
            NbNewParticle = nbNewParticle;

            this.speedRange = new Vector4(vitesseMin, vitesseMax, directionAngle, directionAngleVariation);
            this.angleRange = new Vector2(initialAngleMin,initialAngleMax);
            this.angularSpeedRange = new Vector2(vitesseRotationMin,vitesseRotationMax);
            this.scaleRange = new Vector2(sizeMin,sizeMax);
            this.lifeTimeRange = new Vector2(lifeTimeMin, lifeTimeMax);
            this.colorRange = new int[8] { 255, 255, 255, 255, 255, 255, 0, 70 };
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (Particle p in particles)
                p.Draw(spriteBatch);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="content">Gestionnaire de contenu XNA</param>
        /// <param name="assetNames">Liste des noms des textures à utiliser. ex:"particule/fire"</param>
        public override void LoadContent(ContentManager content)
        {
            foreach (string s in AssetNames)
                textures.Add(content.Load<Texture2D>(s));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nbNewParticle">Nombre de nouvelles particules à créer à chaque appel de la fonction</param>
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
            /*pas encore géré */
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
        public override void Update(float elapsedTime)
        {
            LifeTime -= (int)(elapsedTime / 16);
            Position += Speed * (elapsedTime / 16);
            Angle += AngularSpeed * (elapsedTime / 16);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color, Angle,
                new Vector2(Texture.Width / 2, Texture.Height / 2), Size, SpriteEffects.None, 0f);
        }
    }
}
