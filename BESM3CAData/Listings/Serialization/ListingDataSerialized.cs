using System.Collections.Generic;
using System.IO;
using System.Text.Json;


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
            string output = System.Text.Json.JsonSerializer.Serialize<ListingDataSerialized>(this);

            System.IO.File.WriteAllText(outputPath, output);

        }
    }
}
