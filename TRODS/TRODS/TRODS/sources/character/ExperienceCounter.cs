using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRODS
{
    class ExperienceCounter
    {
        public enum Growth
        {
            Linear, Cuadratic, Exponential
        }

        public Growth _growth;
        public int Experience { get; private set; }
        public int Level { get; private set; }
        private int _toNext;
        private int _last;
        private int _firstLevel;
        public float Percentage
        {
            get
            {
                if (Experience == 0)
                    return 0;
                else
                    return (float)(Experience - _last) / (float)(_toNext - _last);
            }
        }

        public ExperienceCounter(Growth growthType, int firstLevel = 100)
        {
            _growth = growthType;
            _last = 0;
            _firstLevel = firstLevel;
            _toNext = firstLevel;
            Level = 1;
        }

        public void Add(int amount)
        {
            Experience += amount;
            while (Experience >= _toNext)
            {
                Level++;
                _last = _toNext;
                switch (_growth)
                {
                    case Growth.Linear:
                        _toNext += _firstLevel;
                        break;
                    case Growth.Cuadratic:
                        _toNext = (int)(1.3f * (float)_toNext);
                        break;
                    case Growth.Exponential:
                        _toNext = _toNext * _toNext;
                        break;
                }
            }
        }
        public void Reset()
        {
            Experience = 0;
            _last = 0;
            _toNext = _firstLevel;
            Level = 0;
        }
    }
}
