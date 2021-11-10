﻿using BESM3CAData.Listings.Serialization;
using BESM3CAData.Model;
using System.Collections.Generic;
using System.Linq;
using BESM3CAData.Control;

namespace BESM3CAData.Listings
{
    public class PointsContainerDataListing : DataListing
    {
        public int PointScale { get; set; }

        public override DataListingSerialized Serialize()
        {
            DataListingSerialized result = base.Serialize();

            result.PointsContainer = true;
            result.PointScale = PointScale;

            return result;
        }

        public override PointsContainerDataNode CreateNode(string notes, DataController controller, int level = 1, int pointAdj = 0)
        {
            return new PointsContainerDataNode(this, notes, controller, level, pointAdj);
        }

        public PointsContainerDataListing(DataListingSerialized data) : base(data)
        {
            PointScale = data.PointScale;
        }
    }
}
