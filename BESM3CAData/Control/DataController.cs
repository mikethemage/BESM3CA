using BESM3CAData.Listings;
using BESM3CAData.Model;
using System.IO;

namespace BESM3CAData.Control
{
    public class DataController
    {
        //Properties:
        public string FileName { get; set; }
        public MasterListing SelectedListingData { get; set; }
        public CharacterNode RootCharacter { get; set; }
        public int SelectedGenreIndex { get; set; }

        //Fields:
        public ListingDirectory ListingDirectory;

        //Constructor:
        public DataController()
        {
            //Temp code:
            ListingDirectory = ListingDirectory.JSONLoader(Path.Combine("Datafiles", "ListingDirectory.json"));

            ListingLocation DefaultListing = ListingDirectory.AvailableListings.Find(x => (x.ListingName == "BESM3E"));

            //Load listing from file:
            SelectedListingData = DefaultListing.LoadListing();
            ResetAll();
        }


        //Public Methods:
        public void Load(string fileName)
        {
            SelectedGenreIndex = -1;  //Needs changing to load Genre
            RootCharacter = (CharacterNode)SaveLoad.DeserializeXML(fileName, this);
            //Need to check if successful

            FileName = fileName;
        }

        public void ResetAll()
        {
            //Reset root character:
            RootCharacter = new CharacterNode(this);
            FileName = "";
            SelectedGenreIndex = -1;
        }

        public void SaveAs(string fileName)
        {
            FileName = fileName;
            Save();
        }

        public void Save()
        {
            SaveLoad.SerializeXML(RootCharacter, FileName, this);
        }

        public void ExportToText(string exportFile)
        {
            TextWriter tw;

            tw = new StreamWriter(exportFile);

            tw.WriteLine("BESM3CA Character Export");
            tw.WriteLine("Using points listings: " + SelectedListingData.ListingName);
            if (SelectedGenreIndex > -1)
            {
                tw.WriteLine("Genre: " + SelectedListingData.Genres[SelectedGenreIndex]);
            }
            tw.WriteLine();

            SaveLoad.ExportNode(RootCharacter, 0, tw);

            //close file
            tw.Close();
        }

        public void ExportToHTML(string exportFile)
        {
            TextWriter tw;

            tw = new StreamWriter(exportFile);

            tw.Write("<!DOCTYPE html>\n<html>\n<head>\n<title></title>\n<style type = \"text/css\">\n@page\n {\n size: A4; \n}\n@page :left\n {\n margin-left: 2cm;\n }\n@page :right\n {\n margin-right: 2cm;\n }\n</style>\n</head>\n<body>\n<div class=\"CharacterExport\">\n<div class=\"CharacterExportHeader\">\n");
            tw.Write("<h1>BESM3CA Character Export</h1>\n");

            tw.Write("<p>Using points listings: ");
            tw.Write(SelectedListingData.ListingName);
            tw.Write("</p>\n");

            if (SelectedGenreIndex > -1)
            {
                tw.Write("<p>Genre: ");
                tw.Write(SelectedListingData.Genres[SelectedGenreIndex]);
                tw.Write("</p>\n");
            }
            tw.Write("</div>\n");
            tw.Write("<ul>\n");

            SaveLoad.ExportHTMLNode(RootCharacter, 0, tw);

            tw.Write("</ul>\n");

            tw.WriteLine("</div>");
            tw.WriteLine("</body>");
            tw.WriteLine("</html>");

            tw.Close();

        }

    }
}