using BESM3CAData.Listings.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BESM3CAData.Model;
using BESM3CAData.Control;

namespace BESM3CAData.Listings
{
    public class LevelableWithVariantDataListing : LevelableDataListing, IVariantDataListing
    {
        //Properties:
        //Only things with Variants:        
        public List<VariantListing> Variants { get; set; }

        public override DataNode CreateNode(string notes, RPGEntity controller, int level = 1, int pointAdj = 0)
        {
            return new LevelableWithVariantDataNode(this, notes, controller, level, pointAdj);
        }        


        //Constructors:
        public LevelableWithVariantDataListing(DataListingSerialized data) : base(data)
        {
            //Add variants and link back:
            if (data.Variants != null)
            {
                Variants = new List<VariantListing>();
                foreach (VariantListingSerialized variant in data.Variants)
                {
                    Variants.Add(new VariantListing { ID = variant.ID, Name = variant.Name, CostperLevel = variant.CostperLevel, DefaultVariant = variant.DefaultVariant, Desc = variant.Desc, Attribute = this });
                }
            }
        }


        //Methods:
        public override DataListingSerialized Serialize()
        {
            DataListingSerialized result = base.Serialize();

            result.RequiresVariant = true;

            if (Variants != null)
            {
                result.Variants = new List<VariantListingSerialized>();
                foreach (VariantListing variant in Variants)
                {
                    result.Variants.Add(variant.Serialize());
                }
            }

            return result;
        }
    }
}
