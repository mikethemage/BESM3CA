using BESM3CAData.Listings.Serialization;
using System.Collections.Generic;


namespace BESM3CAData.Listings
{
    public class ListingData
    {
        public ListingDataSerialized Serialize()
        {
            ListingDataSerialized result = new ListingDataSerialized { ListingName = this.ListingName, Genres = this.Genres, AttributeList = new List<AttributeListingSerialized>(), TypeList = new List<TypeListingSerialized>(), ProgressionList = new List<ProgressionListingSerialized>() };

            foreach (AttributeListing attribute in AttributeList)
            {
                result.AttributeList.Add(attribute.Serialize());
            }

            foreach (TypeListing typeListing in TypeList)
            {
                result.TypeList.Add(typeListing.Serialize());
            }

            foreach (ProgressionListing progressionListing in ProgressionList)
            {
                result.ProgressionList.Add(progressionListing.Serialize());
            }

            return result;
        }

        //Properties:
        public List<AttributeListing> AttributeList { get; set; }

        public List<TypeListing> TypeList { get; set; }
        public string ListingName { get; set; }

        public List<string> Genres { get; set; }

        public List<ProgressionListing> ProgressionList { get; set; }



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
            //Load JSON data to temp object:
            ListingDataSerialized temp = ListingDataSerialized.JSONLoader(listingLocation);

            //Create new listing data object:
            ListingData result = new ListingData { ListingName = temp.ListingName, Genres = temp.Genres, ProgressionList = new List<ProgressionListing>(), AttributeList = new List<AttributeListing>(), TypeList = new List<TypeListing>() };

            //Deserialize Type Listings:
            foreach (TypeListingSerialized typeListing in temp.TypeList)
            {
                result.TypeList.Add(TypeListing.Deserialize(typeListing));
            }

            //Deserialize Progression Listings:
            foreach (ProgressionListingSerialized progression in temp.ProgressionList)
            {
                result.ProgressionList.Add(ProgressionListing.Deserialize(progression));
            }

            //Deserialize raw attribute data:
            foreach (AttributeListingSerialized attribute in temp.AttributeList)
            {
                AttributeListing newAttribute = AttributeListing.Deserialize(attribute);
                result.AttributeList.Add(newAttribute);
            }

            //Link children:
            foreach (AttributeListingSerialized attribute in temp.AttributeList)
            {
                string[] Children = attribute.ChildrenList.Split(',');
                if (Children.Length > 0)
                {
                    AttributeListing Parent = result.AttributeList.Find(x => x.ID == attribute.ID);
                    foreach (string Child in Children)
                    {
                        if (int.TryParse(Child, out int ChildID))
                        {
                            Parent.AddChild(result.AttributeList.Find(x => x.ID == ChildID));
                        }
                    }
                }
            }

            return result;
        }

        public void CreateJSON(string outputPath)
        {
            //Code to write out JSON data files.   
            //Should not be being called at present - debugging only:
            ListingDataSerialized output = Serialize();
            output.CreateJSON(outputPath);
        }
    }
}
