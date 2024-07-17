using BESM3CAData.Control;
using BESM3CAData.Model;
using System.Collections.Generic;
using System.Linq;
using Triarch.Dtos.Definitions;

namespace BESM3CAData.Listings
{
    public class CompanionDataListing : LevelableDataListing, IFreebieDataListing
    {


        public List<FreebieListing> Freebies
        {
            get
            ;

            set
            ;
        } = new List<FreebieListing>();

        public CompanionDataListing(RPGElementDefinitionDto data) : base(data)
        {

            foreach(FreebieDto freebie in data.Freebies)
            {
                Freebies.Add(new FreebieListing
                {
                    SubAttributeName = freebie.FreebieElementDefinitionName,
                    FreeLevels = freebie.FreeLevels,
                    RequiredLevels = freebie.RequiredLevels
                });                
            }            
        }

        public override RPGElementDefinitionDto Serialize()
        {
            RPGElementDefinitionDto result = base.Serialize();

            result.Freebies = 
                Freebies.Select(x=> new FreebieDto
                {
                    FreebieElementDefinitionName = x.SubAttributeName,
                    FreeLevels = x.FreeLevels,
                    RequiredLevels = x.RequiredLevels
                }).ToList();    
            

            return result;
        }

        public override DataNode CreateNode(string notes, RPGEntity controller, int level = 1, int freeLevels = 0, int requiredLevels = 0, bool isFreebie = false)
        {
            return new CompanionDataNode(this, notes, controller, level, freeLevels, requiredLevels, isFreebie);
        }
    }
}