//using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;


namespace BESM3CAData.Listings
{
    public class ListingData
    {
        //Properties:
        public List<AttributeListing> AttributeList { get; set; }

        public List<TypeListing> TypeList { get; set; }
        public string ListingName { get; set; }

        public List<string> Genres { get; set; }

        public List<ProgressionListing> ProgressionList { get; set; }


        //Constructors:
        public ListingData()
        {
            //Default Constructor for loading
        }

        public string GetProgression(string progressionType, int rank)
        {
            ProgressionListing SelectedProgression = ProgressionList.Find(n => n.ProgressionType == progressionType);
            if (SelectedProgression == null)
            {
                return "";
            }
            else
            {
                return SelectedProgression.GetProgressionValue(rank);
            }
        }

        //Member functions:
        public static ListingData JSONLoader(ListingLocation listingLocation)
        {
            ListingData temp;

            string input = File.ReadAllText(listingLocation.ListingPath);

            //Load listing:
            temp = JsonSerializer.Deserialize<ListingData>(input);

            //Linkback for Variants:
            foreach (AttributeListing attribute in temp.AttributeList)
            {
                if (attribute.Variants != null)
                {
                    foreach (VariantListing variant in attribute.Variants)
                    {
                        variant.Attribute = attribute;
                    }
                }
            }

            //parse attribute children into lists:
            using (JsonDocument document = JsonDocument.Parse(input))
            {
                JsonElement root = document.RootElement;

                foreach (JsonProperty elem in root.EnumerateObject())
                {
                    if (elem.Name == "AttributeList")
                    {
                        foreach (JsonElement attrib in elem.Value.EnumerateArray())
                        {
                            if (attrib.TryGetProperty("ChildrenList", out JsonElement ChildrenListE))
                            {

                                string ChildrenList = ChildrenListE.GetString();
                                string[] Children = ChildrenList.Split(',');
                                int ParentID = attrib.GetProperty("ID").GetInt32();
                                AttributeListing Parent = temp.AttributeList.Find(x => x.ID == ParentID);

                                foreach (string Child in Children)
                                {
                                    if (int.TryParse(Child, out int ChildID))
                                    {
                                        Parent.AddChild(temp.AttributeList.Find(x => x.ID == ChildID));
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return temp;
        }

        public void CreateJSON(string outputPath)
        {
            //Code to write out JSON data files.   
            //Should not be being called at present - debugging only:
            string output = System.Text.Json.JsonSerializer.Serialize<ListingData>(this);

            System.IO.File.WriteAllText(outputPath, output);

        }
    }
}
