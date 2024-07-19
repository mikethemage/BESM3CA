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
        private RPGEntity? _currentEntity;
        public RPGEntity? CurrentEntity
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
        public MasterListing? SelectedListingData { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this,
                    new PropertyChangedEventArgs(propertyName));
            }
        }

        //Fields:
        public ListingDirectory? ListingDirectory { get; set; }

        

        //Constructor:
        public DataController()
        {
            //Temp code:
            ListingDirectory = ListingDirectory.JSONLoader(Path.Combine("Datafiles", "ListingDirectory.json"));

            if (ListingDirectory != null && ListingDirectory.AvailableListings!=null)
            {               
                ListingLocation? DefaultListing = ListingDirectory.AvailableListings.Find(x => (x.ListingName == "BESM3E"));

                if (DefaultListing != null)
                {
                    //Load listing from file:
                    SelectedListingData = DefaultListing.LoadListing();
                    if (SelectedListingData != null)
                    {
                        CurrentEntity = new RPGEntity(SelectedListingData);
                    }
                }
            }
        }

        //Public Methods:
        public void ResetAll()
        {            
            if(SelectedListingData!=null)
            {
                CurrentEntity = new RPGEntity(SelectedListingData);
            }                
        }

        public void ImportOldXml(string fileName)
        {
            if (SelectedListingData != null)
            {
                CurrentEntity = new RPGEntity(SelectedListingData);
                BaseNode? deserialized = SaveLoad.DeserializeXML(fileName, this);
                if (deserialized != null)
                {
                    CurrentEntity.ImportOldXml(fileName, deserialized);
                }
            }           


            //Root.Clear();
            //Root.Add(RootCharacter);
            //RootCharacter.IsSelected = true;
            //RootCharacter.RefreshAll();

            ////Need to check if successful

            //FileName = fileName;
        }

        public void Load(string fileName)
        {
            string loadedText = File.ReadAllText(fileName);
            EntityDto? loadedEntity = JsonSerializer.Deserialize<EntityDto>(loadedText);

            if(loadedEntity != null && (SelectedListingData == null || SelectedListingData.ListingName != loadedEntity.RPGSystemName))
            {
                ListingDirectory = ListingDirectory.JSONLoader(Path.Combine("Datafiles", "ListingDirectory.json"));

                if (ListingDirectory != null && ListingDirectory.AvailableListings != null)
                {
                    ListingLocation? DefaultListing = ListingDirectory.AvailableListings.Find(x => x.ListingName == loadedEntity.RPGSystemName);
                    if (DefaultListing != null)
                    {
                        //Load listing from file:
                        SelectedListingData = DefaultListing.LoadListing();
                    }
                }
            }

            if (SelectedListingData != null)
            {
                CurrentEntity = new RPGEntity(SelectedListingData);
                if (loadedEntity! != null)
                {
                    var selectedGenre = CurrentEntity.GenreList.Where(x => x.GenreName == loadedEntity.GenreName).FirstOrDefault();
                    if (selectedGenre != null)
                    {
                        selectedGenre.IsSelected = true;
                    }

                    CurrentEntity.RootCharacter = LoadNode(loadedEntity.RootElement);


                    CurrentEntity.FileNameAndPath = fileName;
                    CurrentEntity.FileName = loadedEntity.EntityName;
                }

                CurrentEntity.Root.Clear();

                if(CurrentEntity.RootCharacter!=null)
                {
                    CurrentEntity.Root.Add(CurrentEntity.RootCharacter);

                    CurrentEntity.RootCharacter.RefreshAll();
                }                
            }

        }

        public BaseNode? LoadNode(RPGElementDto input)
        {
            BaseNode? output = null;
            if (SelectedListingData != null && SelectedListingData.AttributeList!= null)
            {

                DataListing? ElementDefinition = SelectedListingData.AttributeList.Where(x => x.Name == input.ElementName).FirstOrDefault();
                if (CurrentEntity != null && ElementDefinition != null)
                {
                    if (input.LevelableData != null)
                    {
                        output = ElementDefinition.CreateNode(input.Notes, CurrentEntity, true, input.LevelableData.Level, input.LevelableData.FreeLevels ?? 0, input.LevelableData.RequiredLevels ?? 0, input.LevelableData.FreeLevels != null && input.LevelableData.FreeLevels != 0);
                        if (input.LevelableData.VariantName != null && output is LevelableDataNode variantOutput && variantOutput.VariantList != null && variantOutput.VariantList.Count > 0)
                        {
                            VariantListing? variant = variantOutput.VariantList.Where(x => x.Name == input.LevelableData.VariantName).FirstOrDefault();
                            if (variant != null)
                            {
                                variantOutput.Variant = variant;
                            }
                        }
                    }
                    else
                    {
                        output = ElementDefinition.CreateNode(input.Notes, CurrentEntity, true);
                    }
                }

                if (output != null && input.Children != null && input.Children.Count > 0)
                {
                    foreach (RPGElementDto child in input.Children)
                    {
                        BaseNode? childNode = LoadNode(child);
                        if (childNode != null)
                        {
                            output.Children.Add(childNode);
                        }
                    }
                }
            }
            return output;
        }
    }
}