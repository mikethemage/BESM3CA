using BESM3CAData.Control;
using BESM3CAData.Templates;
using System.IO;

namespace BESM3CAData.Model
{
    public class Controller
    {
        //Properties:
        public string FileName { get; set; }
        public TemplateData SelectedTemplate { get; set; }
        public CharacterData RootCharacter { get; set; }
        public int SelectedGenreIndex { get; set; }


        //Constructor:
        public Controller()
        {
            //Load template from file:
            SelectedTemplate = TemplateData.JSONLoader();
            ResetAll();
        }


        //Public Methods:
        public void Load(string fileName)
        {
            SelectedGenreIndex = -1;  //Needs changing to load Genre
            RootCharacter = (CharacterData)SaveLoad.DeserializeXML(fileName, this);
            //Need to check if successful

            FileName = fileName;
        }

        public void ResetAll()
        {
            //Reset root character:
            RootCharacter = new CharacterData(this);
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
            tw.WriteLine("Template: " + SelectedTemplate.TemplateName);
            if (SelectedGenreIndex > -1)
            {
                tw.WriteLine("Genre: " + SelectedTemplate.Genres[SelectedGenreIndex]);
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
            
            tw.Write("<p>Template: ");
            tw.Write(SelectedTemplate.TemplateName);
            tw.Write("</p>\n");

            if (SelectedGenreIndex > -1)
            {
                tw.Write("<p>Genre: ");
                tw.Write(SelectedTemplate.Genres[SelectedGenreIndex]);
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