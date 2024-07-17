using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BESM3CAData.Listings;
public class FreebieListing
{
    //public DataListing SubAttribute { get; set; } = null;
    public string SubAttributeName { get; set; }
    public int SubAttributeLevel { get; set; } = 0;
    public int SubAttributePointsAdj { get; set; } = 0;
}
