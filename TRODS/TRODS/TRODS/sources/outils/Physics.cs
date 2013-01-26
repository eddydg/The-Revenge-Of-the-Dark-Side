using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRODS.sources.outils
{
    class Physics
    {
        public int MaxHeight
        {
            get;
            set
            {
                if (value < 0)
                    MaxHeight = 0;
            }
        }
        public int TimeOnFlat
        {
            get;
            set
            {
                if (value < 0)
                    TimeOnFlat = 0;
            }
        }

        /// <summary>
        /// Constructeur.
        /// </summary>
        /// <param name="MaxHeight">Hauteur maximale du saut.</param>
        /// <param name="TimeOnFlat">Duree du saut sur une surface plate. (en ms)</param>
        public Physics(int MaxHeight = 200, int TimeOnFlat = 1000)
        {
        }

        /// <summary>
        /// Calcul de la hauteur du saut.
        /// </summary>
        /// <param name="time">Temps. (en ms, partant de 0)</param>
        /// <returns>Hauteut du saut.</returns>
        public int Jump(int time)
        {
            // f(x) = -(x+timeOnflat)2 + maxHeight
            return 0;
        }

        /// <summary>
        /// Calcul de la hauteur de la chute.
        /// </summary>
        /// <param name="time">Temps. (en ms, partant de 0)</param>
        /// <returns>Hauteur de la chute. (negative)</returns>
        public int Fall(int time)
        {
            // f(x) = -x2
            return 0;
        }

    }
}
