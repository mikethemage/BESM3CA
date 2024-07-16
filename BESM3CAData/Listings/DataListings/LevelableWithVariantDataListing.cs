using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BESM3CAData.Model;
using BESM3CAData.Control;
using Triarch.Dtos.Definitions;

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
        public LevelableWithVariantDataListing(RPGElementDefinitionDto data) : base(data)
        {
            //Add variants and link back:
            if (data.LevelableData.Variants != null)
            {
                Variants = new List<VariantListing>();
                foreach (var variant in data.LevelableData.Variants)
                {
                    Variants.Add(new VariantListing { ID = variant.Id, Name = variant.VariantName, CostperLevel = variant.CostPerLevel, DefaultVariant = variant.IsDefault, Desc = variant.Description, Attribute = this });
                }
            }
        }


        //Methods:
        public override RPGElementDefinitionDto Serialize()
        {
            RPGElementDefinitionDto result = base.Serialize();            

            if (Variants != null)
            {
                result.LevelableData.Variants = new List<VariantDefinitionDto>();
                foreach (VariantListing variant in Variants)
                {
                    result.LevelableData.Variants.Add(variant.Serialize());
                }
            }

            return result;
        }
    }
}
