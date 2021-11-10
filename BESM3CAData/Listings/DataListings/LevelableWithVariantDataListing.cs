using BESM3CAData.Listings.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BESM3CAData.Listings
{
    public class LevelableWithVariantDataListing : LevelableDataListing
    {
        //Only things with Variants:
        public bool RequiresVariant { get; private set; }
        public List<VariantListing> Variants { get; set; }


        public LevelableWithVariantDataListing(DataListingSerialized data) : base(data)
        {
            RequiresVariant = data.RequiresVariant;

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

        public override DataListingSerialized Serialize()
        {
            DataListingSerialized result=base.Serialize();
            
            result.RequiresVariant = this.RequiresVariant;

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
