using BESM3CAData.Listings.Serialization;
using System.Collections.Generic;


namespace BESM3CAData.Listings
{
    public class MasterListing
    {
        public MasterListingSerialized Serialize()
        {
            MasterListingSerialized result = new MasterListingSerialized { ListingName = this.ListingName, Genres = this.Genres, AttributeList = new List<DataListingSerialized>(), TypeList = new List<TypeListingSerialized>(), ProgressionList = new List<ProgressionListingSerialized>() };

            foreach (DataListing attribute in AttributeList)
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
        public List<DataListing> AttributeList { get; set; }

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
        public static MasterListing JSONLoader(ListingLocation listingLocation)
        {
            //Load JSON data to temp object:
            MasterListingSerialized temp = MasterListingSerialized.JSONLoader(listingLocation);

            //Create new listing data object:
            MasterListing result = new MasterListing { ListingName = temp.ListingName, Genres = temp.Genres, ProgressionList = new List<ProgressionListing>(), AttributeList = new List<DataListing>(), TypeList = new List<TypeListing>() };

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
            foreach (DataListingSerialized data in temp.AttributeList)
            {
                DataListing newData;
                switch (data.Type)
                {
                    case "Attribute":
                        newData = new AttributeDataListing(data);
                        break;
                    case "Defect":
                        newData = new DefectDataListing(data);
                        break;
                    case "Skill":
                        newData = new SkillDataListing(data);
                        break;

                    case "Restriction":
                        newData = new RestrictionDataListing(data);
                        break;

                    case "Variable":
                        newData = new VariableDataListing(data);
                        break;

                    default:
                        newData = new DataListing(data);
                        break;
                }
                
                result.AttributeList.Add(newData);
            }

            //Link children:
            foreach (DataListingSerialized attribute in temp.AttributeList)
            {
                string[] Children = attribute.ChildrenList.Split(',');
                if (Children.Length > 0)
                {
                    DataListing Parent = result.AttributeList.Find(x => x.ID == attribute.ID);
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
            MasterListingSerialized output = Serialize();
            output.CreateJSON(outputPath);
        }
    }
}
