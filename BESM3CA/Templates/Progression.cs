using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace BESM3CA.Templates
{
    class Progression
    {
        public string ProgressionType { get; set; }

        public List<string> ProgressionsList { get; set; }

        private int MinRank
        { 
            get 
            { 
                return 0;                              
            } 
        }

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
                    return ProgressionsList.Count;
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
                return "0";
            }
            if (rank < MinRank)
            {
                //error: below minimum rank for calculations
                return "0";
            }

            return ProgressionsList[rank];
        }

    }
}
