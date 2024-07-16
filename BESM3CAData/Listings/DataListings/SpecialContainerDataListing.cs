﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BESM3CAData.Model;
using BESM3CAData.Control;
using Triarch.Dtos.Definitions;

namespace BESM3CAData.Listings
{
    public class SpecialContainerDataListing : LevelableDataListing, ISpecialContainerDataListing
    {        
        //Properties:
        public int SpecialPointsPerLevel { get; set; }


        //Constructors:
        public SpecialContainerDataListing(RPGElementDefinitionDto data) : base(data)
        {
            SpecialPointsPerLevel = data.LevelableData.SpecialPointsPerLevel ?? 1;
        }


        //Methods:
        public override DataNode CreateNode(string notes, RPGEntity controller, int level = 1, int pointAdj = 0)
        {
            return new SpecialContainerDataNode(this, notes, controller, level, pointAdj);
        }

        public override RPGElementDefinitionDto Serialize()
        {
            RPGElementDefinitionDto result = base.Serialize();

            result.LevelableData.SpecialPointsPerLevel = SpecialPointsPerLevel;
            
            return result;
        }
    }
}
