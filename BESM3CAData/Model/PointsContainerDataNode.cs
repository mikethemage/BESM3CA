using BESM3CAData.Control;
using BESM3CAData.Listings;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Xml;

namespace BESM3CAData.Model
{
    public class PointsContainerDataNode : DataNode, IPointsDataNode
    {

        public override void RefreshAll()
        {
            foreach (BaseNode item in Children)
            {
                item.RefreshAll();
            }
            RefreshChildPoints();
            RefreshPoints();
            RefreshDisplayText();
        }

        protected override void Children_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs args)
        {
            RefreshChildPoints();
        }

        //Properties:
        

        //Constructors:
        public PointsContainerDataNode(RPGEntity controller, string Notes = "") : base(controller, Notes)
        {
            //Default constructor for data loading only
        }

        public PointsContainerDataNode(PointsContainerDataListing attribute, bool isLoading, string notes, RPGEntity controller, bool isFreebie) : base(attribute, isLoading, notes, controller, isFreebie)
        {

        }

        private int _variablesOrRestrictions;
        public virtual int VariablesOrRestrictions
        {
            get
            {
                return _variablesOrRestrictions;
            }
            set
            {
                int originalVariablesOrRestrictions = _variablesOrRestrictions;
                _variablesOrRestrictions = value;
                if (originalVariablesOrRestrictions != _variablesOrRestrictions)
                {
                    OnPropertyChanged(nameof(VariablesOrRestrictions));
                    RefreshPoints();
                }
            }
        }
        private void RefreshVariablesOrRestrictions()
        {
            int tempVariablesOrRestrictions = 0;
            foreach (var temp in Children)
            {
                if (temp is DataNode tempAttribute)
                {
                    if (tempAttribute.AttributeType == "Restriction" || tempAttribute.AttributeType == "Variable")
                    {
                        tempVariablesOrRestrictions += temp.Points;
                    }
                }             
            }
            VariablesOrRestrictions = tempVariablesOrRestrictions;
        }

        private int _childPoints;

        public virtual int ChildPoints
        {
            get
            {
                return _childPoints;
            }
            set
            {
                int originalChildPoints = _childPoints;
                _childPoints = value;
                if (originalChildPoints != _childPoints)
                {
                    OnPropertyChanged(nameof(ChildPoints));
                    RefreshPoints();
                }
            }
        }
        private void RefreshChildPoints()
        {
            int tempChildPoints = 0;

            foreach (var temp in Children)
            {
                if (temp is DataNode tempAttribute)
                {
                    if (tempAttribute.AttributeType == "Restriction" || tempAttribute.AttributeType == "Variable")
                    {

                    }
                    else
                    {
                        tempChildPoints += temp.Points;
                    }
                }
                else
                {
                    tempChildPoints += temp.Points;
                }             
            }
            ChildPoints = tempChildPoints;
        }

        public override void ChildPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            base.ChildPropertyChanged(sender, e);
            if (sender is BaseNode )
            {
                if (e.PropertyName == nameof(BaseNode.Points))
                {
                    RefreshVariablesOrRestrictions();
                    RefreshChildPoints();
                }
            }
        }

        protected override void RefreshPoints()
        {            
            if (AssociatedListing != null)
            {
                int tempPoints = VariablesOrRestrictions;

                //container point cost calc:
                if (((PointsContainerDataListing)AssociatedListing).PointScale != 0)
                {
                    if (ChildPoints > 0 && ChildPoints < ((PointsContainerDataListing)AssociatedListing).PointScale)
                    {
                        //If child points positive but less that pointscale we should still charge 1 point for the container:
                        tempPoints += 1;
                    }
                    else
                    {
                        //Scale points e.g. 3E items valued at 1/2 total child points:
                        tempPoints += ChildPoints / ((PointsContainerDataListing)AssociatedListing).PointScale;
                    }
                }

                Points = tempPoints;
            }
        }




        //Methods:
        

        //public override void SaveAdditionalXML(XmlTextWriter textWriter)
        //{
        //    //No additional XML for PointsContainers
        //}
    }
}
