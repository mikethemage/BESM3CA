﻿using BESM3CAData.Listings.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BESM3CAData.Listings
{
    public class SkillDataListing : DataListing
    {
        public List<int> GenrePoints { get; private set; }

        public SkillDataListing(DataListingSerialized data) : base(data)
        {
            GenrePoints = data.GenrePoints;
        }

        
        public override DataListingSerialized Serialize()
        {
            DataListingSerialized result=base.Serialize();
            result.GenrePoints = GenrePoints;
            return result;
        }
    }
}
