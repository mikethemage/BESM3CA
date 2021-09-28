//using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.IO;


namespace BESM3CAData.Templates
{
    public class TemplateData
    {
        //Properties:
        public List<AttributeListing> AttributeList { get; set; }
        public List<VariantListing> VariantList { get; set; }
        public List<TypeListing> TypeList { get; set; }
        public string TemplateName { get; set; }

        public List<string> Genres { get; set; }

        public List<Progression> ProgressionList { get; set; }


        //Constructors:
        public TemplateData()
        {
            //Default Constructor for loading
        }

        public string GetProgression(string progressionType, int rank)
        {
            Progression SelectedProgression = ProgressionList.Find(n => n.ProgressionType == progressionType);
            if(SelectedProgression==null)
            {
                return "";
            }
            else
            {
                return SelectedProgression.GetProgressionValue(rank);
            }
        }

        //Member functions:
        public List<string> GetTypesForFilter()
        {
            //LINQ Version:
            List<string> tempList =  (from AttType in TypeList
                                                   orderby AttType.Name
                                                   select AttType.Name).ToList();
            tempList.Insert(0, "All");
            return tempList;
        }

        public static TemplateData JSONLoader()
        {
            TemplateData temp;

            string input = File.ReadAllText(@"Datafiles\BESM3E.json");
            

            temp = JsonSerializer.Deserialize<TemplateData>(input);

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
            string output = System.Text.Json.JsonSerializer.Serialize<TemplateData>(this);

            System.IO.File.WriteAllText(outputPath, output);

        }
    }
}
