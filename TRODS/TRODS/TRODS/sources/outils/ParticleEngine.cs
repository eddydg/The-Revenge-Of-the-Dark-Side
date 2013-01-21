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
            this.colorRange = new int[8] { 255, 255, 255, 255, 255, 255, 0, 70 };

            this.Color = Color.White;
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
        public void LoadContent(ContentManager content, List<string> assetNames)
        {
            foreach (string s in assetNames)
                textures.Add(content.Load<Texture2D>(s));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nbNewParticle">Nombre de nouvelles particules à créer à chaque appel de la fonction</param>
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
        /// définit l'intervalle de vitesse et l'intervalle de direction des particules.
        /// ex: Pour une vitesse entre 0.2 et 2 et une direction de 90° +/- 10°,
        /// on à vMin=0.2f  vMax=2f  angle=90f  angleVariation=10f.
        /// </summary>
        /// <param name="vMin">Vitesse minimum possible pour une particule</param>
        /// <param name="vMax">Vitesse maximum possible pour une particule</param>
        /// <param name="angle">angle en degré qui defini la direction des particules (0° représente une direction horizontale vers la droite.</param>
        /// <param name="angleVariation">degré de variation possible autour de l'angle(paramètre precédent)</param>
        public void SetSpeedRange(float vMin, float vMax, float angle, float angleVariation)
        {
            speedRange = new Vector4(vMin, vMax, angle, angleVariation);
        }
        /// <summary>
        /// définit l'intervalle de l'angle d'inclinaison des particules au moments de leur création.
        /// càd l'angle initial des particules.
        /// </summary>
        /// <param name="min">angle minimum</param>
        /// <param name="max">angle maximun</param>
        public void SetAngleRange(float min, float max)
        {
            angleRange = new Vector2(min, max);
        }
        /// <summary>
        /// définit l'intervalle de vitesse de rotation des particules
        /// </summary>
        /// <param name="min">vitesse de rotation minimum</param>
        /// <param name="max">vitesse de rotation maximum</param>
        public void SetAngularSpeedRange(float min, float max)
        {
            angularSpeedRange = new Vector2(min, max);
        }
        /// <summary>
        /// définit l'intervalle de l'échelle de chaque particules.
        /// ex: 2 affichera la particule 2 fois plus grosse que sa taille d'origine.
        /// </summary>
        /// <param name="min">échelle minimum</param>
        /// <param name="max">échelle maximum</param>
        public void SetScaleRange(float min, float max)
        {
            scaleRange = new Vector2(min, max);
        }
        /// <summary>
        /// définit l'intervalle de temps de vie de chaque particule.
        /// </summary>
        /// <param name="min">temps de vie minimum</param>
        /// <param name="max">temps de vie maximum</param>
        public void SetLifeTimeRange(float min, float max)
        {
            lifeTimeRange = new Vector2(min, max);
        }
        /// <summary>
        /// définit l'intervalle de couleur des particules en fonction des valeur RGB et Alpha)
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
        public void Update()
        {
            LifeTime--;
            Position += Speed;
            Angle += AngularSpeed;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color, Angle,
                new Vector2(Texture.Width / 2, Texture.Height / 2), Size, SpriteEffects.None, 0f);
        }
    }
}
