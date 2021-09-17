using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BESM3CA.Templates
{
    class Progressions
    {
        public const int MinRank = 0;
        public const int MaxRank = 16;
        private string[] _progressionGridFast;
        private string[] _progressionGridMedium;
        private string[] _progressionGridSlow;
        private string[] _progressionGridTime;

        public Progressions()
        {
            _progressionGridFast = new string[MaxRank + 1] { "0","1","10","100","1,000","10,000","100,000","1,000,000","10,000,000","100,000,000","1,000,000,000","10,000,000,000","100,000,000,000","1,000,000,000,000","10,000,000,000,000","100,000,000,000,000","1,000,000,000,000,000"};
            _progressionGridMedium = new string[MaxRank + 1] { "0","1","3","10","30","100","300","1,000","3,000","10,000","30,000","100,000","300,000","1,000,000","3,000,000","10,000,000","100,000,000" };
            _progressionGridSlow = new string[MaxRank + 1] { "0", "1", "2", "4", "8", "15", "30", "60", "125", "250", "500", "1,000", "2,000", "4,000", "8,000", "15,000", "30,000" };
            _progressionGridTime = new string[MaxRank + 1] { "0", "1 round", "5 round", "1 minute", "10 minutes", "1 hour", "4 hours", "12 hours", "1 day", "1 week", "1 month", "1 season", "1 year", "10 years", "100 years", "1000 years", "Permanent" };
        }

        public string GetProgression(string progressionType, int rank)
        {
            if(rank>MaxRank)
            {
                //error: above maximum rank for calculations
                return "0";
            }
            if (rank < MinRank)
            {
                //error: below minimum rank for calculations
                return "0";
            }

            string progressionValue;
            switch (progressionType)
            {
                case "Fast":
                    progressionValue = _progressionGridFast[rank];
                    break;
                case "Medium":
                    progressionValue = _progressionGridMedium[rank];
                    break;
                case "Slow":
                    progressionValue = _progressionGridSlow[rank];
                    break;
                case "Time":
                    progressionValue = _progressionGridTime[rank];
                    break;
                default:
                    progressionValue = "0";
                    break;
            }
            return progressionValue;
        }
    }
}
