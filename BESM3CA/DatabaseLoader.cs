using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace BESM3CA
{
    class DatabaseLoader
    {

        public static void LoadDatabase(List<AttributeListing> AttributeList, List<VariantListing> VariantList, List<TypeListing> TypeList)
        {
            string source;

            SqlConnection conn;

            //Config File:
            //source = Properties.Settings.Default.BESM3CAConnectionString;

#if DEBUG
            //Development:
            source = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"C:\\Users\\Mike\\Documents\\Visual Studio 2019\\Projects\\BESM3CA\\DB\\BESM3CA.mdf\";Integrated Security=True;Connect Timeout=30;";
#else
            //App Folder:
            //Initial Catalog=BESM3Release;
                source="Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"" + Application.StartupPath + "\\BESM3CA.mdf\";Integrated Security=True;Connect Timeout=30;";            
#endif

            conn = new SqlConnection(source);
            conn.Open();

            
            
            
            SqlCommand cmd;
            SqlDataReader reader;
            
            cmd = new SqlCommand("Select * from Attribute, Types where Attribute.Type=Types.TypeName Order by TypeOrder, AttributeName;", conn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                AttributeListing temp = new AttributeListing();
                temp.ID = (int)reader["AttributeID"];
                temp.Name = (string)reader["AttributeName"];
                temp.Container = (bool)reader["Container"];
                if (reader["CostperLevel"].GetType() != typeof(DBNull))
                {
                    temp.CostperLevel = (int)reader["CostperLevel"];
                }
                else
                {
                    temp.CostperLevel = 0;
                }
                temp.CostperLevelDesc = (string)reader["CostperLevelDesc"];
                temp.EnforceMaxLevel = (bool)reader["EnforceMaxLevel"];
                temp.Human = (bool)reader["Human"];

                if (reader["MaxLevel"].GetType() != typeof(DBNull))
                {
                    temp.MaxLevel = (int)reader["MaxLevel"];
                }
                else
                {
                    temp.MaxLevel = int.MaxValue;
                }

                temp.Page = (string)reader["Page"];


                if (reader["Progression"].GetType() != typeof(DBNull))
                {
                    temp.Progression = (string)reader["Progression"];
                }
                else
                {
                    temp.Progression = "";
                }

                temp.RequiresVariant = (bool)reader["RequiresVariant"];
                temp.SpecialContainer = (bool)reader["SpecialContainer"];
                temp.SpecialPointsPerLevel = (int)reader["SpecialPointsPerLevel"];
                temp.Stat = (string)reader["Stat"];
                temp.Type = (string)reader["Type"];

                if (reader["Description"].GetType() != typeof(DBNull))
                {
                    temp.Description = (string)reader["Description"];
                }
                else
                {
                    temp.Description = "";
                }

                AttributeList.Add(temp);

            }
            reader.Close();
            cmd = new SqlCommand("Select * from AttChildren;", conn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                AttributeListing Parent = AttributeList.Find(n => n.ID == (int)reader["ParentID"]);
                AttributeListing Child = AttributeList.Find(n => n.ID == (int)reader["ChildID"]);

                Parent.AddChild(Child);


            }
            reader.Close();

            

            cmd = new SqlCommand("Select * from Variant order by AttributeID, VariantName;", conn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                VariantListing temp = new VariantListing();
                temp.ID = (int)reader["VariantID"];
                temp.Name = (string)reader["VariantName"];
                if (reader["CostperLevel"].GetType() != typeof(DBNull))
                {
                    temp.CostperLevel = (int)reader["CostperLevel"];
                }
                else
                {
                    temp.CostperLevel = 0;
                }
                temp.AttributeID = (int)reader["AttributeID"];

                if (reader["VariantDesc"].GetType() != typeof(DBNull))
                {
                    temp.Desc = (string)reader["VariantDesc"];
                }
                else
                {
                    temp.Desc = "";
                }

                temp.DefaultVariant = (bool)reader["DefaultVariant"];

                VariantList.Add(temp);
            }
            reader.Close();


            

            cmd = new SqlCommand("Select * from Types order by TypeOrder;", conn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                TypeListing temp = new TypeListing();
                temp.ID = (int)reader["TypeID"];
                temp.Name = (string)reader["TypeName"];
                if (reader["TypeOrder"].GetType() != typeof(DBNull))
                {
                    temp.TypeOrder = (int)reader["TypeOrder"];
                }
                else
                {
                    temp.TypeOrder = 0;
                }

                TypeList.Add(temp);

            }

            reader.Close();

            try
            {
                conn.Close();
            }
            catch
            { }
        }
    }
}
