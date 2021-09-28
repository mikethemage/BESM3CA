﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BESM3CAData.Control;
using BESM3CAData.Templates;

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
            SelectedGenreIndex=-1;  //Needs changing to load Genre
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
            /*try
            {*/
                tw = new StreamWriter(exportFile);
                SaveLoad.ExportNode(RootCharacter, 0, tw);
            /*}
            catch
            {
                //MessageBox.Show("Error Opening file: " + saveFileDialog1.FileName);
                return;
            }*/
            //close file
            tw.Close();
        }
    }
}
