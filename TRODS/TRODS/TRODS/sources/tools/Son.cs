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
    public class Son
    {
        private Dictionary<Sons, SoundEffectInstance> _sons;
        private Dictionary<Musiques, Song> _musiques;
        private float _sonsVol;
        public float SonsVolume
        {
            get { return _sonsVol; }
            set
            {
                if (value >= 0 && value <= 1)
                {
                    foreach (SoundEffectInstance s in _sons.Values)
                        s.Volume = value;
                    _sonsVol = value;
                }
            }
        }
        private float _musiquesVol;
        public float MusiquesVolume
        {
            get { return _musiquesVol; }
            set
            {
                if (value >= 0 && value <= 1)
                {
                    MediaPlayer.Volume = value;
                    _musiquesVol = value;
                }
            }
        }

        /// <summary>
        /// Constructeur
        /// </summary>
        public Son()
        {
            _sons = new Dictionary<Sons, SoundEffectInstance>();
            _musiques = new Dictionary<Musiques, Song>();
            _sonsVol = 1f;
            _musiquesVol = 1f;
        }

        /// <summary>
        /// Cree une instance d'effect sonore
        /// </summary>
        /// <param name="content">Gestionnaire de contenu XNA</param>
        /// <param name="s">Son</param>
        /// <param name="assetName">Nom du fichier</param>
        public void LoadContent(ContentManager content, Sons s, string assetName)
        {
            _sons.Add(s, content.Load<SoundEffect>(assetName).CreateInstance());
        }
        /// <summary>
        /// Charge une musique
        /// </summary>
        /// <param name="content">Gestionnaire de contenu XNA</param>
        /// <param name="s">Musique</param>
        /// <param name="assetName">Nom du fichier</param>
        public void LoadContent(ContentManager content, Musiques m, string assetName)
        {
            _musiques.Add(m, content.Load<Song>(assetName));
        }

        /// <summary>
        /// Joue un son
        /// </summary>
        /// <param name="s">Son obtenu dans l'enumeration Sons</param>
        public void Play(Sons s)
        {
            try
            {
                _sons[s].Play();
            }
            catch (Exception e)
            {
                EugLib.IO.FileStream.toStdOut(e.ToString());
            }
        }
        /// <summary>
        /// Repete une musique
        /// </summary>
        /// <param name="m">Musique obtenue dans l'enumeration Musiques</param>
        public void Play(Musiques m)
        {
            try
            {
                MediaPlayer.Play(_musiques[m]);
                MediaPlayer.IsRepeating = true;
            }
            catch (Exception e)
            {
                EugLib.IO.FileStream.toStdOut(e.ToString());
            }
        }
        /// <summary>
        /// Arrete la lecture de tous les sons et musiques
        /// </summary>
        public void Stop()
        {
            foreach (SoundEffectInstance s in _sons.Values)
                s.Stop();
            MediaPlayer.Stop();
        }
    }
}
