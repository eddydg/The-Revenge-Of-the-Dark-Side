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
        private Rectangle emitterLocation;
        private List<Particle> particles;
        private List<Texture2D> textures;
        private int nbNewParticle;
        private List<string> assetNames;

        private Vector3 sizeRange;
        private Vector2 speedRange;
        private Vector2 directionRange;
        private Vector2 angleRange;
        private Vector2 angularSpeedRange;
        private Vector2 lifeTimeRange;
        private byte[] colorRange;
        private Rectangle windowSize;

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="_windowSize">Taille actuelle de la fenêtre</param>
        /// <param name="emitterLocation">Zone d'apparition des nouvelles particules</param>
        /// <param name="assetNames">Liste des noms des textures à utiliser</param>
        /// <param name="nbNewParticle">Nombre de nouvelle particules à générer à chaque Update</param>
        /// <param name="vitesseMin">Vitesse minimal d'une particule</param>
        /// <param name="vitesseMax">Vitesse maximale d'une particule</param>
        /// <param name="directionAngle">Direction de propagation des particules (en degré)</param>
        /// <param name="directionAngleVariation">Variation (en degré) autour de l'axe de propagation</param>
        /// <param name="initialAngleMin">Angle initiale minimal de la particule</param>
        /// <param name="initialAngleMax">Angle initiale maximal de la particule</param>
        /// <param name="SizeRange">X : minWidth - Width : maxWidth - Y : minHeight - Height : maxHeight</param>
        /// <param name="vitesseRotationMin">Vitesse de rotation minimale (en degré/Update())</param>
        /// <param name="vitesseRotationMax">Vitesse de rotation maximale (en degré/Update())</param>
        /// <param name="lifeTimeMin">Temps de vie minimal de la particule</param>
        /// <param name="lifeTimeMax">Temps de vie maximal de la particule</param>
        public ParticleEngine(Rectangle _windowSize,
                              Rectangle emitterLocation,
                              Vector3 SizeRange,
                              List<string> assetNames = null,
                              int nbNewParticle = 10,
                              float vitesseMin = 0f, float vitesseMax = 2f,
                              float directionAngle = 0f, float directionAngleVariation = 180f,
                              float initialAngleMin = 0f, float initialAngleMax = 0f,
                              float vitesseRotationMin = -2, float vitesseRotationMax = 2,
                              float lifeTimeMin = 20f, float lifeTimeMax = 100f)
        {
            windowSize = _windowSize;
            random = new Random();
            this.emitterLocation = emitterLocation;
            this.assetNames = assetNames;
            particles = new List<Particle>();
            textures = new List<Texture2D>();
            this.nbNewParticle = nbNewParticle;

            this.speedRange = new Vector2(vitesseMin, vitesseMax);
            this.directionRange = new Vector2(directionAngle, directionAngleVariation);
            this.angleRange = new Vector2(initialAngleMin, initialAngleMax);
            this.angularSpeedRange = new Vector2(vitesseRotationMin, vitesseRotationMax);
            this.lifeTimeRange = new Vector2(lifeTimeMin, lifeTimeMax);
            this.sizeRange = SizeRange;
            this.colorRange = new byte[6] { 255, 255, 255, 255, 255, 255 };
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (Particle p in particles)
                p.Draw(spriteBatch);
        }

        public override void LoadContent(ContentManager content)
        {
            foreach (string s in assetNames)
                textures.Add(content.Load<Texture2D>(s));
        }

        public override void Update(float elapsedTime)
        {
            for (int i = 0; i < nbNewParticle; i++)
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

            emitterLocation.X = (int)(emitterLocation.X * xRapport);
            emitterLocation.Y = (int)(emitterLocation.Y * yRapport);
            emitterLocation.Width = (int)(emitterLocation.Width * xRapport);
            emitterLocation.Height = (int)(emitterLocation.Height * yRapport);

            speedRange.X *= (float)xRapport;
            speedRange.X *= (float)xRapport;

            sizeRange.X = (int)(sizeRange.X * xRapport);
            sizeRange.Y = (int)(sizeRange.Y * xRapport);
            sizeRange.Z = (int)(sizeRange.Z * yRapport);

            windowSize = rect;
        }

        /// <summary>
        /// Génère une nouvelle particule "aléatoire" en fonction des propriété de l'objet.
        /// </summary>
        /// <returns>Retourne la particule généré.</returns>
        private Particle GenerateParticle()
        {
            float v = speedRange.X + (float)random.NextDouble() * (float)(random.Next((int)speedRange.Y - (int)speedRange.X + 1));
            float alpha = -((float)random.Next((int)directionRange.X - (int)directionRange.Y, (int)directionRange.X + (int)directionRange.Y + 1) * (float)Math.PI / 180);

            float angle = ((float)random.Next((int)angleRange.X, (int)angleRange.Y + 1)) * (float)Math.PI / 180f;
            float angularSpeed = ((float)random.Next((int)angularSpeedRange.X, (int)angularSpeedRange.Y + 1)) * (float)Math.PI / 180f;

            float sizeScale = (float)(random.Next((int)sizeRange.X, (int)sizeRange.Y + 1) / sizeRange.Z);

            return new Particle(
                textures[random.Next(textures.Count)],
                new DecimalRectangle(
                    random.Next(emitterLocation.X, emitterLocation.X + emitterLocation.Width + 1),
                    random.Next(emitterLocation.Y, emitterLocation.Y + emitterLocation.Height + 1),
                    sizeRange.Y * sizeScale,
                    sizeRange.Z * sizeScale),
                new Vector2(v * (float)Math.Cos(alpha), v * (float)Math.Sin(alpha)),
                angle,
                angularSpeed,
                new Color(
                    random.Next(colorRange[0], colorRange[1] + 1),
                    random.Next(colorRange[2], colorRange[3] + 1),
                    random.Next(colorRange[4], colorRange[5] + 1)),
                random.Next((int)lifeTimeRange.X, (int)lifeTimeRange.Y + 1));
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
        public void SetColorRange(byte rMin, byte rMax, byte gMin, byte gMax, byte bMin, byte bMax)
        {
            colorRange[0] = rMin;
            colorRange[1] = rMax;
            colorRange[2] = gMin;
            colorRange[3] = gMax;
            colorRange[4] = bMin;
            colorRange[5] = bMax;
        }
    }

    class Particle : AbstractScene
    {
        private Texture2D texture;
        private DecimalRectangle position;
        private Vector2 speed;
        private float angle;
        private float angularSpeed;
        private Color color;
        private int lifeTime;
        public int LifeTime
        {
            get { return lifeTime; }
        }
        private int totalLifeTime;
        private int alphaTransparency;

        public Particle(Texture2D texture, DecimalRectangle position, Vector2 speed, float angle, float angularSpeed, Color color, int lifeTime)
        {
            this.texture = texture;
            this.position = position;
            this.speed = speed;
            this.angle = angle;
            this.angularSpeed = angularSpeed;
            this.color = color;
            this.color.A = 0;
            this.lifeTime = lifeTime;
            this.totalLifeTime = lifeTime;
            this.alphaTransparency = 255;
        }

        public override void Update(float elapsedTime)
        {
            lifeTime -= (int)(elapsedTime / 16);
            position.X += (int)(speed.X * (elapsedTime / 16));
            position.Y += (int)(speed.Y * (elapsedTime / 16));
            angle += angularSpeed * (elapsedTime / 16);
            if (lifeTime <= totalLifeTime / 2)
                alphaTransparency = 255 * (lifeTime / (totalLifeTime / 2));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (lifeTime > totalLifeTime / 2)
                spriteBatch.Draw(texture, position.ToRectangle(), null, color, angle,
                new Vector2(position.Width / 2, position.Height / 2), SpriteEffects.None, 0f);
            else
                spriteBatch.Draw(texture, position.ToRectangle(), null, Color.FromNonPremultiplied(color.R, color.G, color.B, alphaTransparency), angle,
                new Vector2(position.Width / 2, position.Height / 2), SpriteEffects.None, 0f);
        }

        public void WindowResized(double x, double y)
        {
            position.X = (int)(position.X * x);
            position.Y = (int)(position.Y * y);
            position.Width = (int)(position.Width * x);
            position.Height = (int)(position.Height * y);

            speed.X *= (float)x;
            speed.Y *= (float)y;
        }
    }
}
