using System.Collections.Generic;
using System.Linq;

namespace BESM3CAData.Templates
{
    public class Progression
    {
        public string ProgressionType { get; set; }

        public List<string> ProgressionsList { get; set; }

        private const int MinRank = 0;        

        private int MaxRank
        {
            get
            {
                if (ProgressionsList == null)
                {
                    return 0;
                }
                else
                {
                    return ProgressionsList.Count-1;
                }
            }
        }

        public Progression()
        {
            //Default Constructor for loading
        }

        public Progression(string progressionType, string[] progressionArray)
        {
            ProgressionType = progressionType;
            ProgressionsList = progressionArray.ToList<string>();
        }

        public string GetProgressionValue(int rank)
        {
            if (rank > MaxRank)
            {
                //error: above maximum rank for calculations
                return "ERROR";
            }
            if (rank < MinRank)
            {
                //error: below minimum rank for calculations
                return "ERROR";
            }

            return ProgressionsList[rank];
        }

    }
}
