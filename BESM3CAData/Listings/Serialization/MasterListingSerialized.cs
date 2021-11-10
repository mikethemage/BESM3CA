using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BESM3CAData.Listings.Serialization
{
    public class MasterListingSerialized
    {
        //Properties:
        public List<DataListingSerialized> AttributeList { get; set; }
        public List<TypeListingSerialized> TypeList { get; set; }
        public string ListingName { get; set; }
        public List<string> Genres { get; set; }
        public List<ProgressionListingSerialized> ProgressionList { get; set; }

        private static readonly JsonSerializerOptions serializerOptions = new JsonSerializerOptions { DefaultIgnoreCondition=JsonIgnoreCondition.WhenWritingNull, WriteIndented=true };   

        //Member functions:
        public static MasterListingSerialized JSONLoader(ListingLocation listingLocation)
        {
            MasterListingSerialized temp;

            string input = File.ReadAllText(listingLocation.ListingPath);

            //Load listing:
            temp = JsonSerializer.Deserialize<MasterListingSerialized>(input);

            return temp;
        }

        public void CreateJSON(string outputPath)
        {
            //Code to write out JSON data files.   
            //Should not be being called at present - debugging only:
            string output = JsonSerializer.Serialize<MasterListingSerialized>(this, serializerOptions);

            System.IO.File.WriteAllText(outputPath, output);

        }
    }
}
