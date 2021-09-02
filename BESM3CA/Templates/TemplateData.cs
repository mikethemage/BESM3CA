using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace BESM3CA.Templates
{
    class TemplateData
    {
        //Properties:
        public List<AttributeListing> AttributeList { get; set; }

        public List<VariantListing> VariantList { get; set; }

        public List<TypeListing> TypeList { get; set; }

        public string TemplateName { get; set; }


        //Constructors:
        public TemplateData()
        {
            //Default Constructor for loading
        }


        //Member functions:
        public List<String> GetTypesForFilter()
        {
            //LINQ Version:
            IEnumerable<string> FilteredTypeList = from AttType in TypeList
                                                   orderby AttType.Name
                                                   select AttType.Name;
            return FilteredTypeList.ToList();
        }

        public static TemplateData JSONLoader()
        {
            TemplateData temp;

            string input = System.IO.File.ReadAllText(@"Datafiles\BESM3E.json");

            temp = System.Text.Json.JsonSerializer.Deserialize<TemplateData>(input);

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
                                    int ChildID;
                                    Int32.TryParse(Child, out ChildID);
                                    Parent.AddChild(temp.AttributeList.Find(x => x.ID == ChildID));
                                }
                            }
                        }
                    }
                }
            }
            return temp;
        }
    }
}
