using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRODS
{
    class ExperienceCounter
    {
        public ExperienceCounter.Growth _growth;
        private int _toNext;
        private int _last;
        private int _firstLevel;

        public int Experience { get; private set; }

        public int Level { get; private set; }

        public float Percentage
        {
            get
            {
                if (this.Experience == 0)
                    return 0.0f;
                else
                    return (float)(this.Experience - this._last) / (float)(this._toNext - this._last);
            }
        }

        public ExperienceCounter(ExperienceCounter.Growth growthType, int firstLevel = 100)
        {
            this._growth = growthType;
            this._last = 0;
            this._firstLevel = firstLevel;
            this._toNext = firstLevel;
            this.Level = 1;
        }

        public void Add(int amount)
        {
            this.Experience += amount;
            while (this.Experience >= this._toNext)
            {
                ++this.Level;
                this._last = this._toNext;
                switch (this._growth)
                {
                    case ExperienceCounter.Growth.Linear:
                        this._toNext += this._firstLevel;
                        break;
                    case ExperienceCounter.Growth.Cuadratic:
                        this._toNext = (int)(1.29999995231628 * (double)this._toNext);
                        break;
                    case ExperienceCounter.Growth.Exponential:
                        this._toNext = this._toNext * this._toNext;
                        break;
                }
            }
        }

        public void Reset()
        {
            this.Experience = 0;
            this._last = 0;
            this._toNext = this._firstLevel;
            this.Level = 0;
        }

        public enum Growth
        {
            Linear,
            Cuadratic,
            Exponential,
        }
    }
}