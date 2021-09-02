using System;
using System.Collections.Generic;
using System.Text.Json;

namespace BESM3CA.Templates
{
    static class JSONyStuff
    {
        public static void JSONLoader(out List<AttributeListing> AttributeList, out List<VariantListing> VariantList, out List<TypeListing> TypeList)
        {
            string input = System.IO.File.ReadAllText(@"Datafiles\Attributes.json");
                       
            AttributeList = System.Text.Json.JsonSerializer.Deserialize<List<AttributeListing>>(input);

            using (JsonDocument document = JsonDocument.Parse(input))
            {
                JsonElement root = document.RootElement;

                foreach (JsonElement attrib in root.EnumerateArray())
                {
                    if (attrib.TryGetProperty("ChildrenList", out JsonElement ChildrenListE))
                    {

                        string ChildrenList = ChildrenListE.GetString();
                        string[] Children = ChildrenList.Split(',');
                        int ParentID = attrib.GetProperty("ID").GetInt32();
                        AttributeListing Parent = AttributeList.Find(x => x.ID == ParentID);

                        foreach (string Child in Children)
                        {
                            int ChildID;
                            Int32.TryParse(Child, out ChildID);
                            Parent.AddChild(AttributeList.Find(x => x.ID == ChildID));
                        }
                    }
                }
            }
            
            input = System.IO.File.ReadAllText(@"Datafiles\Variants.json");
            VariantList = System.Text.Json.JsonSerializer.Deserialize<List<VariantListing>>(input);

            input = System.IO.File.ReadAllText(@"Datafiles\Types.json");
            TypeList = System.Text.Json.JsonSerializer.Deserialize<List<TypeListing>>(input);
        }


        public static void createJSON(List<AttributeListing> AttributeList, List<VariantListing> VariantList, List<TypeListing> TypeList)
        {
            //Code to write out JSON data files.   
            //Should not be being called at present - debugging only:
            string output = System.Text.Json.JsonSerializer.Serialize<List<AttributeListing>>(AttributeList);

            System.IO.File.WriteAllText(@"C:\Users\Mike\Documents\TestBESM.json", output);

            output = System.Text.Json.JsonSerializer.Serialize<List<VariantListing>>(VariantList);

            System.IO.File.WriteAllText(@"C:\Users\Mike\Documents\TestBESM2.json", output);

            output = System.Text.Json.JsonSerializer.Serialize<List<TypeListing>>(TypeList);

            System.IO.File.WriteAllText(@"C:\Users\Mike\Documents\TestBESM3.json", output);

        }

        public static void createJSON2(TemplateData templateData)
        {
            //Code to write out JSON data files.   
            //Should not be being called at present - debugging only:
            string output = System.Text.Json.JsonSerializer.Serialize<TemplateData>(templateData);

            System.IO.File.WriteAllText(@"C:\Users\Mike\Documents\TestBESM.json", output);

           

        }

    }
}
