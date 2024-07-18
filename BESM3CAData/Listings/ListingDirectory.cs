using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace BESM3CAData.Listings
{
    public class ListingDirectory
    {
        public List<ListingLocation> AvailableListings { get; set; }

        public ListingDirectory()
        {
        }

        public void CreateJSON(string outputPath)
        {
            //Code to write out JSON data files.   
            //Should not be being called at present - debugging only:
            string output = System.Text.Json.JsonSerializer.Serialize<ListingDirectory>(this);

            System.IO.File.WriteAllText(outputPath, output);

        }

        public static ListingDirectory? JSONLoader(string directoryPath)
        {
            ListingDirectory? temp;

            string input = File.ReadAllText(directoryPath);

            //Load listing:
            temp = JsonSerializer.Deserialize<ListingDirectory>(input);
            return temp;
        }

    }
}
