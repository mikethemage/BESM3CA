using BESM3CAData.Control;
using BESM3CAData.Listings;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Xml;
using System.ComponentModel;
using System;
using System.Collections.Specialized;


namespace BESM3CAData.Model
{
    public abstract class BaseNode : INotifyPropertyChanged
    {

        //Need view model for selected attribute for adding????
        public void AddSelectedChild()
        {
            if(SelectedAttributeToAdd != null)
            {
                AddChildAttribute(SelectedAttributeToAdd);
            }            
        }

        public bool CanAddSelectedChild()
        {
            if (SelectedAttributeToAdd != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsFreebie { get; set; } = false;

        public DataListing? SelectedAttributeToAdd { get; set; }

        public virtual void ChildPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            //Event handler for changes to children
            if (sender is DataListing dataListing)
            {
                if (e.PropertyName == nameof(DataListing.IsSelected) && dataListing.IsSelected == true)
                {
                    SelectedAttributeToAdd = dataListing;
                    AddCommand?.RaiseCanExecuteChanged();
                }
                //else if (sender == SelectedNode)
                //{
                //    OnPropertyChanged(nameof(SelectedNode));
                //}
            }
        }


        private void CreateAddCommand()
        {
            AddCommand = new RelayCommand(AddSelectedChild, CanAddSelectedChild);
        }

        public RelayCommand? AddCommand
        {
            get; private set;
        }
        //****


        public virtual void RefreshAll()
        {
            foreach (BaseNode item in Children)
            {
                item.RefreshAll();
            }
            RefreshPoints();
            RefreshDisplayText();
        }

        private int _baseCost;
        public virtual int BaseCost
        {
            get
            {
                return _baseCost;
            }
            protected set
            {
                int originalCost = _baseCost;
                _baseCost = value;
                if (originalCost != _baseCost)
                {
                    //Cost has changed
                    OnPropertyChanged(nameof(BaseCost));
                    RefreshPoints();
                }
            }
        }

        private int _points;
        public int Points
        {
            get
            {
                return _points;
            }
            protected set
            {
                int originalPoints = _points;

                _points = value;
                if (originalPoints != _points)
                {
                    OnPropertyChanged(nameof(Points));
                    RefreshDisplayText();
                }
            }
        }
        protected abstract void RefreshPoints();

        private string _displayText = string.Empty;
        public string DisplayText
        {
            get
            {
                return _displayText;
            }
            protected set
            {
                string originalDisplayText = _displayText;
                _displayText = value;
                if (originalDisplayText != _displayText)
                {
                    OnPropertyChanged(nameof(DisplayText));
                }
            }
        }

        protected virtual void RefreshDisplayText()
        {
            DisplayText = $"{Name} ({Points} Points)";
        }


        //Fields:
        private int _lastChildOrder;


        //Properties:
        public RPGEntity AssociatedController { get; private set; }
        public virtual DataListing? AssociatedListing { get; protected set; }
        
        private string _name = string.Empty;

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (value != _name)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                    RefreshDisplayText();
                }
            }
        }

        public string Notes { get; set; }


        //Tree structure properties:
        //public BaseNode? FirstChild { get; private set; }
        public BaseNode? Parent { get; private set; }
        public int NodeOrder { get; private set; }
        public BaseNode? Next { get; private set; }
        public BaseNode? Prev { get; private set; }

        public ObservableCollection<BaseNode> Children { get; private set; } = new ObservableCollection<BaseNode>();

        private bool _isSelected;

        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {

                _isSelected = value;

                if (value == true)
                {
                    AddAttributeSelectionHandlers();

                    if (AssociatedController.SelectedListingData?.AttributeList != null)
                    {
                        //Deselect any invalid options:
                        foreach (DataListing stillSelectedDataListing in AssociatedController.SelectedListingData.AttributeList.Where(x => x.IsSelected))
                        {
                            if (AssociatedListing?.FilteredPotentialChildren == null || !AssociatedListing.FilteredPotentialChildren.Contains(stillSelectedDataListing))
                            {
                                stillSelectedDataListing.IsSelected = false;
                            }
                        }
                    }

                    string temp = AssociatedController.SelectedType ?? "";
                    foreach (FilterType oldFilterType in AssociatedController.Filters)
                    {
                        oldFilterType.IsSelected = false;

                    }
                    AssociatedController.Filters.Clear();
                    foreach (string filterType in GetTypesForFilter())
                    {
                        FilterType newFilterType = new FilterType(filterType);
                        AssociatedController.Filters.Add(newFilterType);
                        newFilterType.PropertyChanged += AssociatedController.FilterPropertyChanged;
                        if (filterType == temp)
                        {
                            newFilterType.IsSelected = true;
                        }
                    }

                    if (AssociatedController.SelectedType == null || AssociatedController.SelectedType == "" || AssociatedController.Filters.FirstOrDefault(x => x.TypeName == temp) == null)
                    {
                        if (AssociatedController.Filters.FirstOrDefault(x => x.TypeName == "All") is FilterType allFilterType2)
                        {
                            allFilterType2.IsSelected = true;
                        }
                    }

                }
                else
                {
                    foreach (FilterType oldFT in AssociatedController.Filters)
                    {
                        oldFT.PropertyChanged -= AssociatedController.FilterPropertyChanged;
                    }
                    RemoveAttributeSelectionHandlers();
                }

                AddCommand?.RaiseCanExecuteChanged();

                OnPropertyChanged(nameof(IsSelected));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            /*if (PropertyChanged != null)
            {
                PropertyChanged(this,
                    new PropertyChangedEventArgs(propertyName));
            }*/
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public List<DataListing>? PotentialChildren
        {
            get
            {
                if (AssociatedListing != null)
                {
                    return AssociatedListing.Children.Where(x => x.Name != "Character").ToList();
                }
                else
                {
                    return null;
                }
            }
        }

        public bool Useable { get; private set; }

        //Constructors:
        public BaseNode(RPGEntity controller, string notes = "")
        {
            //Default constructor for data loading only
            AssociatedController = controller;

            PropertyChanged += AssociatedController.ChildPropertyChanged;

            //Need to set attribute seperately in order to use this!
            Useable = false;
            Notes = notes;
            NodeOrder = 1;
            
            Parent = null;
            _lastChildOrder = 0;
            Next = null;
            Prev = null;

            Children.CollectionChanged += Children_CollectionChanged;

            CreateMoveUpCommand();
            CreateMoveDownCommand();
            CreateDeleteCommand();
            CreateAddCommand();
        }
        public RelayCommand? DeleteCommand
        {
            get; private set;
        }
        public RelayCommand? MoveUpCommand
        {
            get; private set;
        }
        public RelayCommand? MoveDownCommand
        {
            get; private set;
        }
        private void CreateMoveUpCommand()
        {
            MoveUpCommand = new RelayCommand(MoveUp, CanMoveUp);
        }

        private void CreateMoveDownCommand()
        {
            MoveDownCommand = new RelayCommand(MoveDown, CanMoveDown);
        }

        public bool CanMoveUp()
        {
            return Prev != null;
        }

        public bool CanMoveDown()
        {
            return Next != null;
        }

        protected virtual void Children_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs args)
        {
            RefreshPoints();
        }

        public BaseNode(DataListing attribute, RPGEntity controller, bool isLoading, string notes = "", bool isFreebie = false)
        {
            Debug.Assert(controller.SelectedListingData != null);  //Check if we have listing data...

            AssociatedController = controller;
            PropertyChanged += AssociatedController.ChildPropertyChanged;
            AssociatedListing = attribute;
            Name = attribute.Name ?? "";
            
            Useable = true;
            Notes = notes;
            NodeOrder = 1;
            
            Parent = null;
            _lastChildOrder = 0;
            Next = null;
            Prev = null;
            IsFreebie = isFreebie;
            Children.CollectionChanged += Children_CollectionChanged;

            if (!isLoading && attribute.Freebies != null && attribute.Freebies.Count > 0)
            {
                foreach (FreebieListing freebie in attribute.Freebies)
                {
                    DataListing? subAttribute = controller.SelectedListingData.AttributeList?.Where(x => x.Name == freebie.SubAttributeName).FirstOrDefault();
                    if (subAttribute != null)
                    {
                        //Auto create freebie when creating new instance of this attribute:
                        AddChild(subAttribute.CreateNode("", AssociatedController, false, freebie.FreeLevels + freebie.RequiredLevels, freebie.FreeLevels, freebie.RequiredLevels, true));
                    }
                }
            }

            CreateMoveUpCommand();
            CreateMoveDownCommand();
            CreateDeleteCommand();
            CreateAddCommand();
        }


        //Methods:
        public List<string> GetTypesForFilter()
        {
            if(AssociatedController.SelectedListingData?.TypeList==null)
            {
                return new List<string>();
            }

            //LINQ Version:
            List<string> tempList = (

                                     from AttChild in PotentialChildren
                                     join TypeValue in AssociatedController.SelectedListingData.TypeList on AttChild.Type equals TypeValue.Name
                                     orderby TypeValue.TypeOrder
                                     select AttChild.Type

                                     ).Distinct().ToList();
            tempList.Insert(0, "All");
            return tempList;
        }

        public List<DataListing>? GetFilteredPotentialChildren(string filter)
        {
            List<DataListing>? selectedAttributeChildren = PotentialChildren;
            if (selectedAttributeChildren != null)
            {
                //LINQ Version:
                List<DataListing> filteredAttList = selectedAttributeChildren
                    .Where(a => (filter == "All" || filter == "" || a.Type == filter))
                    .OrderBy(a => a.Type)
                    .ThenBy(a => a.Name)
                    .ToList();

                return filteredAttList;
            }
            else
            {
                return null;
            }
        }


        public void AddChild(BaseNode child)
        {
            if (Children.Count > 0)            
            {
                BaseNode temp = Children[0];
                while (temp.Next != null)
                {
                    temp = temp.Next;
                }
                temp.Next = child;
                child.Prev = temp;
            }

            child.Parent = this;
            _lastChildOrder++;
            child.NodeOrder = _lastChildOrder;
            Children.Add(child);
            child.PropertyChanged += this.ChildPropertyChanged;
        }

        public void Delete()
        {
            if (Parent != null) //Do not delete root node!
            {
                
                if (Prev != null) //we have at least one previous entry, so not the first child 
                {
                    Prev.Next = Next;
                    Prev.IsSelected = true;
                }
                if (Next != null) //we are not the last child
                {
                    Next.Prev = Prev;
                    if (Prev == null)
                    {
                        Next.IsSelected = true;
                    }
                }
                if (Prev == null && Next == null)
                {
                    Parent.IsSelected = true;
                }

                Parent.Children.Remove(this);
                PropertyChanged -= AssociatedController.ChildPropertyChanged;
                PropertyChanged -= Parent.ChildPropertyChanged;
                Parent = null;
            }
        }

        public bool CanDelete()
        {
            return Parent != null && IsFreebie == false;  //Do not delete "Freebies"
        }

        private void CreateDeleteCommand()
        {
            DeleteCommand = new RelayCommand(Delete, CanDelete);
        }


        public void MoveUp()
        {
            if (Prev != null)
            {
                BaseNode temp = Prev;

                

                if (Next != null)
                {
                    Next.Prev = temp;
                }
                if (temp.Prev != null)
                {
                    temp.Prev.Next = this;
                }

                Prev = temp.Prev;
                temp.Next = Next;
                Next = temp;
                temp.Prev = this;
                int tempNodeOrder = NodeOrder;
                NodeOrder = temp.NodeOrder;
                temp.NodeOrder = tempNodeOrder;

                Parent?.Children.Move(Parent.Children.IndexOf(this), Parent.Children.IndexOf(this) - 1);
                MoveUpCommand?.RaiseCanExecuteChanged();
                MoveDownCommand?.RaiseCanExecuteChanged();
            }
        }

        public void MoveDown()
        {
            if (Next != null)
            {
                BaseNode temp = Next;

                

                if (Prev != null)
                {
                    Prev.Next = temp;
                }
                if (temp.Next != null)
                {
                    temp.Next.Prev = this;
                }

                Next = temp.Next;
                temp.Prev = Prev;
                Prev = temp;
                temp.Next = this;
                int tempNodeOrder = NodeOrder;
                NodeOrder = temp.NodeOrder;
                temp.NodeOrder = tempNodeOrder;
                Parent?.Children.Move(Parent.Children.IndexOf(this), Parent.Children.IndexOf(this) + 1);
                MoveUpCommand?.RaiseCanExecuteChanged();
                MoveDownCommand?.RaiseCanExecuteChanged();
            }
        }

        public BaseNode AddChildAttribute(DataListing attribute)
        {
            BaseNode Temp = attribute.CreateNode("", AssociatedController, false);
            AddChild(Temp);
            return Temp;
        }

        public virtual void InvalidateGenrePoints()
        {
            foreach (BaseNode child in Children)
            {
                child.InvalidateGenrePoints();
            }            
        }

        //Getting stats:
        public abstract CalcStats GetStats();

        //XML:
        //public void SaveXML(XmlTextWriter textWriter)
        //{
        //    textWriter.WriteStartElement(GetType().Name);
        //    textWriter.WriteAttributeString("Name", Name);
        //    textWriter.WriteAttributeString("ID", ID.ToString());
        //    textWriter.WriteStartElement("AdditionalData");
        //    SaveAdditionalXML(textWriter);
        //    textWriter.WriteEndElement();
        //    textWriter.WriteStartElement("Notes");
        //    textWriter.WriteString(Notes);
        //    textWriter.WriteEndElement();
        //    textWriter.WriteEndElement();
        //}

        //public abstract void SaveAdditionalXML(XmlTextWriter textWriter);

        private void AddAttributeSelectionHandlers()
        {
            if (AssociatedListing != null)
            {
                foreach (DataListing item in AssociatedListing.Children)
                {

                    item.PropertyChanged += ChildPropertyChanged;
                }
            }
        }

        private void RemoveAttributeSelectionHandlers()
        {
            if (AssociatedListing != null)
            {
                foreach (DataListing item in AssociatedListing.Children)
                {
                    item.PropertyChanged -= ChildPropertyChanged;
                }
            }
        }

        public void LoadXML(XmlTextReader reader)
        {
            while (reader.NodeType != XmlNodeType.None)
            {
                // loading node attributes
                int attributeCount = reader.AttributeCount;
                if (attributeCount > 0)
                {
                    for (int i = 0; i < attributeCount; i++)
                    {
                        reader.MoveToAttribute(i);
                        switch (reader.Name)
                        {
                            case "Name":
                                Name = reader.Value;

                                if (AssociatedController.SelectedListingData != null)
                                {
                                    AssociatedListing = AssociatedController.SelectedListingData.AttributeList?.Find(n => n.Name == Name);
                                    Useable = true;                                    
                                }
                                break;

                            //case "ID":
                            //    ID = int.Parse(reader.Value);
                            //    if (AssociatedController.SelectedListingData != null)
                            //    {
                            //        AssociatedListing = AssociatedController.SelectedListingData.AttributeList.Find(n => n.ID == ID);
                            //        Useable = true;
                            //    }
                            //    break;

                            default:
                                break;
                        }
                    }
                }

                reader.Read();

                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.Name == "Notes")
                    {
                        Notes = reader.ReadString();
                    }

                    if (reader.Name == "AdditionalData")
                    {
                        LoadAdditionalXML(reader);
                    }

                }
                else if (reader.NodeType == XmlNodeType.EndElement)
                {
                    if (reader.Name.EndsWith(GetType().Name) || reader.Name.EndsWith("CharacterData") || reader.Name.EndsWith("AttributeData"))
                    {
                        break;
                    }
                }
            }
        }
        public abstract void LoadAdditionalXML(XmlTextReader reader);
    }
}
