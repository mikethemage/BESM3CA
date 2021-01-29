﻿using System;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Windows.Forms;

//Currently not in use

namespace BESM3CA
{
    class JSONyStuff
    {

        public static void JSONLoader(List<AttributeListing> AttributeList, List<VariantListing> VariantList, List<TypeListing> TypeList)
        {
            string input = System.IO.File.ReadAllText(@"C:\Users\Mike\Documents\TestBESM.json");

            AttributeList = JsonConvert.DeserializeObject<List<AttributeListing>>(input);

            JArray Ja = JArray.Parse(input) as JArray;

            //JObject Jo = Ja[0] as JObject;

            foreach (JObject Jo in Ja)
            {
                string ChildrenList;
                ChildrenList = Jo.Value<string>("ChildrenList");

                

                if (ChildrenList != "")
                {
                    string[] Children= ChildrenList.Split(',');
                    int ParentID = Jo.Value<int>("ID");

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



        public static void createJSON(List<AttributeListing> AttributeList)
        {
            string output = JsonConvert.SerializeObject(AttributeList, Formatting.Indented);

            System.IO.File.WriteAllText(@"C:\Users\Mike\Documents\TestBESM.json", output);

        }



    }
}