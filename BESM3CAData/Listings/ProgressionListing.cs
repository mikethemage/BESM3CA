﻿using BESM3CAData.Listings.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace BESM3CAData.Listings
{
    public class ProgressionListing
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

        public ProgressionListingSerialized Serialize()
        {
            return new ProgressionListingSerialized { ProgressionType = this.ProgressionType, ProgressionsList = this.ProgressionsList };
        }

        public static ProgressionListing Deserialize(ProgressionListingSerialized progression)
        {
            ProgressionListing result = new ProgressionListing { ProgressionsList = progression.ProgressionsList, ProgressionType = progression.ProgressionType };

            return result;
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
