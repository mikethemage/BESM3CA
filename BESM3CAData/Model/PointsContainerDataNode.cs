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

        protected override void Children_CollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            RefreshChildPoints();
        }

        //Properties:
        public int PointAdj
        {
            get
            {
                return 0;
            }
        }

        //Constructors:
        public PointsContainerDataNode(RPGEntity controller, string Notes = "") : base(controller, Notes)
        {
            //Default constructor for data loading only
        }

        public PointsContainerDataNode(PointsContainerDataListing attribute, string notes, RPGEntity controller) : base(attribute, notes, controller)
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
            BaseNode temp = FirstChild;
            while (temp != null)
            {
                if (temp is DataNode tempAttribute)
                {
                    if (tempAttribute.AttributeType == "Restriction" || tempAttribute.AttributeType == "Variable")
                    {
                        tempVariablesOrRestrictions += temp.Points;
                    }
                }
                temp = temp.Next;
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

            BaseNode temp = FirstChild;
            while (temp != null)
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

                temp = temp.Next;
            }
            ChildPoints = tempChildPoints;
        }

        public override void ChildPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is BaseNode baseNode)
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
            int tempPoints = VariablesOrRestrictions;

            //container point cost calc:
            if (((PointsContainerDataListing)AssociatedListing).PointScale == 0 || ChildPoints < ((PointsContainerDataListing)AssociatedListing).PointScale)
            {
                //PointScale should be set for containers - if not set points to 0 to avoid divide by 0 errors:
                tempPoints += 0;
            }
            else
            {
                //Scale points e.g. 3E items valued at 1/2 total child points:
                tempPoints += ChildPoints / ((PointsContainerDataListing)AssociatedListing).PointScale;
            }

            Points = tempPoints;
        }




        //Methods:
        

        public override void SaveAdditionalXML(XmlTextWriter textWriter)
        {
            //Todo: remove method
        }
    }
}
