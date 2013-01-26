using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRODS.sources.outils
{
    class Physics
    {

        /// <summary>
        /// Fonction qui calcule la trajectoire d'un saut.
        /// </summary>
        /// <param name="maxHeight">Hateur maximale.</param>
        /// <param name="timeOnFlat">Duree du saut sur une surface plate. (en ms)</param>
        /// <param name="time">Temps (0 au debut) (en ms)</param>
        /// <returns>Hauteur en fonction du temps.</returns>
        public static int Jump(int time, int maxHeight = 200, int timeOnFlat = 1000)
        {
            return 0;
        }

        /// <summary>
        /// Fonction qu calcule la trajectoire d'une chute.
        /// </summary>
        /// <param name="speed">Vitesse de la chute.</param>
        /// <returns></returns>
        public static int Fall(int speed = 1)
        {
            return 0;
        }

    }
}
