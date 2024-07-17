using BESM3CAData.Listings;
using BESM3CAData.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using Triarch.Dtos.Entities;

namespace BESM3CAData.Control
{
    public class DataController : INotifyPropertyChanged
    {
        private RPGEntity _currentEntity;
        public RPGEntity CurrentEntity
        {
            get { return _currentEntity; }
            set
            {
                if (_currentEntity != value)
                {
                    _currentEntity = value;
                    OnPropertyChanged(nameof(CurrentEntity));
                }
            }
        }

        //Properties:        
        public MasterListing SelectedListingData { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this,
                    new PropertyChangedEventArgs(propertyName));
            }
        }

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

            CurrentEntity = new RPGEntity(SelectedListingData);
        }

        //Public Methods:
        public void ResetAll()
        {
            
                CurrentEntity = new RPGEntity(SelectedListingData);
            
        }

        public void ImportOldXml(string fileName)
        {
            CurrentEntity = new RPGEntity(SelectedListingData);

            CurrentEntity.ImportOldXml(fileName, SaveLoad.DeserializeXML(fileName, this));


            //Root.Clear();
            //Root.Add(RootCharacter);
            //RootCharacter.IsSelected = true;
            //RootCharacter.RefreshAll();

            ////Need to check if successful

            //FileName = fileName;
        }

        public void Load(string fileName)
        {
            var loadedText = File.ReadAllText(fileName);
            var loadedEntity = JsonSerializer.Deserialize<EntityDto>(loadedText);

            if(SelectedListingData == null || SelectedListingData.ListingName != loadedEntity.RPGSystemName)
            {
                ListingDirectory = ListingDirectory.JSONLoader(Path.Combine("Datafiles", "ListingDirectory.json"));

                ListingLocation DefaultListing = ListingDirectory.AvailableListings.Find(x => x.ListingName == loadedEntity.RPGSystemName);

                //Load listing from file:
                SelectedListingData = DefaultListing.LoadListing();
            }
            CurrentEntity = new RPGEntity(SelectedListingData);

            var selectedGenre = CurrentEntity.GenreList.Where(x=>x.GenreName == loadedEntity.GenreName).FirstOrDefault();
            if (selectedGenre != null)
            {
                selectedGenre.IsSelected = true;
            }

            CurrentEntity.RootCharacter = LoadNode(loadedEntity.RootElement);
            CurrentEntity.FileNameAndPath = fileName;
            CurrentEntity.FileName = loadedEntity.EntityName;

            CurrentEntity.Root.Clear();
            CurrentEntity.Root.Add(CurrentEntity.RootCharacter);

            CurrentEntity.RootCharacter.RefreshAll();

        }

        public BaseNode LoadNode(RPGElementDto input)
        {
            BaseNode output = null;

            var ElementDefinition = SelectedListingData.AttributeList.Where(x => x.Name == input.ElementName).FirstOrDefault();
            if (ElementDefinition != null)
            {
                
                if (input.LevelableData != null)
                {
                    output = ElementDefinition.CreateNode(input.Notes, CurrentEntity,input.LevelableData.Level);
                }
                else
                {
                    output = ElementDefinition.CreateNode(input.Notes, CurrentEntity);
                }
            }

            if (input.Children != null && input.Children.Count > 0)
            {
                foreach (RPGElementDto child in input.Children)
                {
                    output.Children.Add(LoadNode(child));
                }
            }

            return output;
        }
    }
}