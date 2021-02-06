﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Text.Json;
using System.Linq;
using System.Windows.Forms;


namespace BESM3CA
{
    class JSONyStuff
    {

        public static void JSONLoader(out List<AttributeListing> AttributeList, List<VariantListing> VariantList, List<TypeListing> TypeList)
        {
            string input = System.IO.File.ReadAllText(@"C:\Users\Mike\Documents\TestBESM.json");
                        
            //Use system.JSON instead of Newtonsoft?:
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

        }


        public static void createJSON(List<AttributeListing> AttributeList)
        {
            string output = System.Text.Json.JsonSerializer.Serialize<List<AttributeListing>>(AttributeList);

            System.IO.File.WriteAllText(@"C:\Users\Mike\Documents\TestBESM.json", output);

        }

    }
}
