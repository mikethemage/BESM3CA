using BESM3CAData.Listings;
using BESM3CAData.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;

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

        public void Load(string fileName)
        {
            CurrentEntity = new RPGEntity(SelectedListingData);

            CurrentEntity.Load(fileName, SaveLoad.DeserializeXML(fileName, this));


            //Root.Clear();
            //Root.Add(RootCharacter);
            //RootCharacter.IsSelected = true;
            //RootCharacter.RefreshAll();

            ////Need to check if successful

            //FileName = fileName;
        }

    }
}