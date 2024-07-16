using System.Collections.Generic;
using System.Linq;
using Triarch.Dtos.Definitions;

namespace BESM3CAData.Listings
{
    public class ProgressionListing
    {
        public string ProgressionType { get; set; }

        public List<string> ProgressionsList { get; set; }

        public bool CustomProgression { get; set; } = false;

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
                    return ProgressionsList.Count - 1;
                }
            }
        }

        public ProgressionListing()
        {
            //Default Constructor for loading
        }

        public ProgressionListing(string progressionType, string[] progressionArray)
        {
            ProgressionType = progressionType;
            ProgressionsList = progressionArray.ToList<string>();
        }

        public ProgressionDto Serialize()
        {
            return new ProgressionDto { ProgressionType = this.ProgressionType, Progressions = this.ProgressionsList.Select(x => new ProgressionEntryDto { Text = x }).ToList() };
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
