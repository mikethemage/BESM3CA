using System;
using System.Collections.Generic;
using System.Text.Json;

namespace BESM3CA.Templates
{
    static class JSONyStuff
    {
        public static void JSONLoader(out List<AttributeListing> attributeList, out List<VariantListing> variantList, out List<TypeListing> typeList)
        {
            string input = System.IO.File.ReadAllText(@"Datafiles\Attributes.json");
                       
            attributeList = System.Text.Json.JsonSerializer.Deserialize<List<AttributeListing>>(input);

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
                        AttributeListing Parent = attributeList.Find(x => x.ID == ParentID);

                        foreach (string Child in Children)
                        {
                            
                            if (Int32.TryParse(Child, out int ChildID))
                            { 
                                Parent.AddChild(attributeList.Find(x => x.ID == ChildID)); 
                            }
                        }
                    }
                }
            }
            
            input = System.IO.File.ReadAllText(@"Datafiles\Variants.json");
            variantList = System.Text.Json.JsonSerializer.Deserialize<List<VariantListing>>(input);

            input = System.IO.File.ReadAllText(@"Datafiles\Types.json");
            typeList = System.Text.Json.JsonSerializer.Deserialize<List<TypeListing>>(input);
        }


        public static void CreateJSON(List<AttributeListing> attributeList, List<VariantListing> variantList, List<TypeListing> typeList)
        {
            //Code to write out JSON data files.   
            //Should not be being called at present - debugging only:
            string output = System.Text.Json.JsonSerializer.Serialize<List<AttributeListing>>(attributeList);

            System.IO.File.WriteAllText(@"C:\Users\Mike\Documents\TestBESM.json", output);

            output = System.Text.Json.JsonSerializer.Serialize<List<VariantListing>>(variantList);

            System.IO.File.WriteAllText(@"C:\Users\Mike\Documents\TestBESM2.json", output);

            output = System.Text.Json.JsonSerializer.Serialize<List<TypeListing>>(typeList);

            System.IO.File.WriteAllText(@"C:\Users\Mike\Documents\TestBESM3.json", output);

        }

        public static void CreateJSON2(TemplateData templateData)
        {
            //Code to write out JSON data files.   
            //Should not be being called at present - debugging only:
            string output = System.Text.Json.JsonSerializer.Serialize<TemplateData>(templateData);

            System.IO.File.WriteAllText(@"C:\Users\Mike\Documents\TestBESM.json", output);           

        }

    }
}
