using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BESM3CA.Templates
{
    class TemplateData
    {
        public List<AttributeListing> AttributeList;
        public List<VariantListing> VariantList;
        public List<TypeListing> TypeList;

        public TemplateData()
        {
            //Create new JSON file - debugging only:
            //JSONyStuff.createJSON(AttributeList, VariantList, TypeList);

            //Now loads from JSON files:
            JSONyStuff.JSONLoader(out AttributeList, out VariantList, out TypeList);
        }
    }
}
