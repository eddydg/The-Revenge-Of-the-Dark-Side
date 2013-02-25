using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRODS
{
    class Physics
    {
        public int MaxHeight { get; set; }
        public int TimeOnFlat { get; set; }
        public int Time;

        /// <summary>
        /// Constructeur.
        /// </summary>
        /// <param name="MaxHeight">Hauteur maximale du saut.</param>
        /// <param name="TimeOnFlat">Duree du saut sur une surface plate. (en ms)</param>
        public Physics(int maxHeight = 200, int timeOnFlat = 1000)
        {
            MaxHeight = maxHeight;
            TimeOnFlat = timeOnFlat;
            Time = 0;
        }

        public void Jump()
        {
            Time = 0;
        }

        public void Fall()
        {
            Time = TimeOnFlat / 2;
        }

        public int Update(float elapsedTime)
        {
            Time += (int)elapsedTime;
            return f(Time) - f(Time - (int)elapsedTime);
        }

        private int f(int x)
        {
            return -(x - TimeOnFlat / 2) * (x - TimeOnFlat / 2) * 4 * MaxHeight / (TimeOnFlat * TimeOnFlat) + MaxHeight;
        }

        public void WindowResized(float yRapt)
        {
            MaxHeight = (int)(yRapt * (float)MaxHeight);
        }
    }
}
