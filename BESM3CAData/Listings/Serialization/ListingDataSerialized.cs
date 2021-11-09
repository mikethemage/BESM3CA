using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BESM3CAData.Listings.Serialization
{
    public class ListingDataSerialized
    {
        //Properties:
        public List<AttributeListingSerialized> AttributeList { get; set; }
        public List<TypeListingSerialized> TypeList { get; set; }
        public string ListingName { get; set; }
        public List<string> Genres { get; set; }
        public List<ProgressionListingSerialized> ProgressionList { get; set; }

        private static JsonSerializerOptions serializerOptions = new JsonSerializerOptions { DefaultIgnoreCondition=JsonIgnoreCondition.WhenWritingNull, WriteIndented=true };   

        //Member functions:
        public static ListingDataSerialized JSONLoader(ListingLocation listingLocation)
        {
            ListingDataSerialized temp;

            string input = File.ReadAllText(listingLocation.ListingPath);

            //Load listing:
            temp = JsonSerializer.Deserialize<ListingDataSerialized>(input);

            return temp;
        }

        public void CreateJSON(string outputPath)
        {
            //Code to write out JSON data files.   
            //Should not be being called at present - debugging only:
            string output = JsonSerializer.Serialize<ListingDataSerialized>(this, serializerOptions);

            System.IO.File.WriteAllText(outputPath, output);

        }
    }
}
