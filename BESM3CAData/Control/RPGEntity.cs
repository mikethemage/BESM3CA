using BESM3CAData.Listings;
using BESM3CAData.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BESM3CAData.Control
{
    public class RPGEntity : INotifyPropertyChanged
    {

        public ObservableCollection<GenreEntry> GenreList { get; set; } = new ObservableCollection<GenreEntry>();

        private GenreEntry _genreEntry;
        public GenreEntry SelectedGenreEntry
        {
            get
            {
                return _genreEntry;
            }
            set
            {
                //if (value != _genreEntry)
                //{
                    _genreEntry = value;
                    OnPropertyChanged(nameof(SelectedGenreEntry));
                //}
            }
        }

        public ObservableCollection<FilterType> Filters { get; set; } = new ObservableCollection<FilterType>();
        public string FileNameAndPath { get; set; }
        public string FileName { get; set; }

        private MasterListing _selectedListingData;
        public MasterListing SelectedListingData
        {
            get
            {
                return _selectedListingData;
                
            }
            set
            {
                if (value != _selectedListingData)
                {
                    _selectedListingData = value;

                    foreach (GenreEntry oldGenre in GenreList)
                    {
                        oldGenre.PropertyChanged -= GenrePropertyChanged;
                    }

                    GenreList.Clear();
                    foreach (string newListings in SelectedListingData.Genres)
                    {
                        GenreEntry newGenre = new GenreEntry(newListings, SelectedListingData.Genres.IndexOf(newListings));
                        GenreList.Add(newGenre);
                        newGenre.PropertyChanged += GenrePropertyChanged;
                    }
                    OnPropertyChanged(nameof(SelectedListingData));
                }
            }
        }

        public BaseNode RootCharacter { get; set; }

        public BaseNode SelectedNode
        {
            get { return selectedNode; }
            set
            {
                selectedNode = value;
                OnPropertyChanged(nameof(SelectedNode));
                OnPropertyChanged(nameof(SelectedCharacter));
                OnPropertyChanged(nameof(SelectedLevelable));
                OnPropertyChanged(nameof(SelectedVariantNode));
            }
        }

        public CharacterNode SelectedCharacter
        {
            get
            {
                return selectedNode as CharacterNode;
            }
        }

        public LevelableDataNode SelectedLevelable
        {
            get
            {
                return selectedNode as LevelableDataNode;
            }
        }

        public IVariantDataNode SelectedVariantNode
        {
            get
            {
                return selectedNode as IVariantDataNode;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this,
                    new PropertyChangedEventArgs(propertyName));
            }
        }

        public ObservableCollection<BaseNode> Root { get; private set; } = new ObservableCollection<BaseNode>();

        

        private string selectedType ;
        public string SelectedType
        {
            get
            {
                return selectedType;
            }

            private set
            {
                //if (value != selectedType)
                //{
                    selectedType = value;
                    OnPropertyChanged(nameof(SelectedType));
                //}

            }
        }

        private BaseNode selectedNode;

        public void ImportOldXml(string fileName, BaseNode newRoot)
        {           
            
            RootCharacter = newRoot;
            Root.Clear();
            Root.Add(newRoot);
            newRoot.IsSelected = true;
            newRoot.RefreshAll();

            //Need to check if successful

            //FileNameAndPath = fileName;
        }


        public void ChildPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is BaseNode baseNode)
            {
                if (e.PropertyName == nameof(BaseNode.IsSelected) && baseNode.IsSelected == true)
                {
                    SelectedNode = baseNode;
                }
                //else if (sender == SelectedNode)
                //{
                //    OnPropertyChanged(nameof(SelectedNode));
                //}
            }
        }

        public void FilterPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is FilterType filterType)
            {
                if (e.PropertyName == nameof(FilterType.IsSelected) && filterType.IsSelected == true)
                {
                    SelectedType = filterType.TypeName;
                    if (SelectedNode != null)
                    {
                        SelectedNode.AssociatedListing.RefreshFilteredPotentialChildren(SelectedType);
                    }

                }
            }
        }

        public void GenrePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is GenreEntry genreEntry)
            {
                if (e.PropertyName == nameof(GenreEntry.IsSelected) && genreEntry.IsSelected == true)
                {
                    SelectedGenreEntry = genreEntry;
                    if (SelectedNode != null)
                    {
                        
                        RootCharacter.InvalidateGenrePoints();
                    }

                }
            }
        }

        public void ResetAll()
        {
            //Reset root character:

            //Get Character data listing:
            if (SelectedListingData.AttributeList.Find(x => x.Name == "Character") is CharacterDataListing characterDataListing)
            {
                RootCharacter = characterDataListing.CreateNode("", this);
                Root.Clear();
                Root.Add(RootCharacter);
                RootCharacter.IsSelected = true;
                FileNameAndPath = "";
                
            }
            else
            {
                throw new InvalidDataException("No valid Character data");

            }

        }

        public RPGEntity()
        {
            ResetAll();

        }

        public RPGEntity(MasterListing selectedListing)
        {
            SelectedListingData = selectedListing;
            ResetAll();

        }

        public void SaveAs(string fileNameAndPath, string fileName)
        {
            FileNameAndPath = fileNameAndPath;
            FileName = fileName;
            Save();
        }

        public void Save()
        {
            SaveLoad.SerializeJSON(RootCharacter, FileNameAndPath, FileName, this);
        }

        public void ExportToText(string exportFile)
        {
            TextWriter tw;

            tw = new StreamWriter(exportFile);

            tw.WriteLine("BESM3CA Character Export");
            tw.WriteLine("Using points listings: " + SelectedListingData.ListingName);
            if (SelectedGenreEntry !=null)
            {
                tw.WriteLine("Genre: " + SelectedGenreEntry.GenreName);
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

            if (SelectedGenreEntry != null)
            {
                tw.Write("<p>Genre: ");
                tw.Write(SelectedGenreEntry.GenreName);
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
