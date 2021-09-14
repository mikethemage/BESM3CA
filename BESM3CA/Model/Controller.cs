﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BESM3CA.Control;
using BESM3CA.Templates;

namespace BESM3CA.Model
{
    class Controller
    {
        public string FileName { get; set; }
        public TemplateData SelectedTemplate { get; set; }
        public CharacterData RootCharacter { get; set; }

        public Controller()
        {
            //Load template from file:
            SelectedTemplate = TemplateData.JSONLoader();
            ResetAllTemp();
        }

        public void Load(string fileName)
        {
            RootCharacter = (CharacterData)SaveLoad.DeserializeXML(fileName, SelectedTemplate);
            //Need to check if successful

            FileName = fileName;
        }

        public void ResetAllTemp()
        {
            //Reset root character:
            RootCharacter = new CharacterData(SelectedTemplate);
            FileName = "";
        }

        public void SaveAs(string fileName)
        {
            FileName = fileName;
            Save();
        }

        public void Save()
        {
            SaveLoad.SerializeXML(RootCharacter, FileName, SelectedTemplate);       
        }

        public void Export(string exportFile)
        {
            TextWriter tw;
            try
            {
                tw = new StreamWriter(exportFile);
                SaveLoad.ExportNode(RootCharacter, 0, tw);
            }
            catch
            {
                //MessageBox.Show("Error Opening file: " + saveFileDialog1.FileName);
                return;
            }
            //close file
            tw.Close();
        }
    }
}
