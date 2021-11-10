﻿using BESM3CAData.Listings.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BESM3CAData.Listings
{
    public class SpecialContainerDataListing : LevelableDataListing
    {        
        public int SpecialPointsPerLevel { get; set; }
        public bool SpecialContainer { get; private set; }

        public SpecialContainerDataListing(DataListingSerialized data) : base(data)
        {
            SpecialPointsPerLevel = data.SpecialPointsPerLevel;
            SpecialContainer = data.SpecialContainer;            
        }

        public override DataListingSerialized Serialize()
        {
            DataListingSerialized result=base.Serialize();
            
            result.SpecialPointsPerLevel = this.SpecialPointsPerLevel;
            result.SpecialContainer = this.SpecialContainer;
            return result;
        }
    }
}