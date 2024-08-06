using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Xml;
using System.ComponentModel;
using System;
using System.Collections.Specialized;
using Triarch.BusinessLogic.Models.Definitions;


namespace Triarch.BusinessLogic.Models.Definitions;

public abstract class BaseNode
{
    public bool IsFreebie { get; set; } = false;

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
            
        }
    }

    protected virtual void RefreshDisplayText()
    {
        DisplayText = $"{Name} ({Points} Points)";
    }


    //Fields:
    private int _lastChildOrder;


    //Properties:
    public virtual RPGElementDefinition AssociatedElementDefinition { get; protected set; }
    public RPGSystem AssociatedSystem { get; protected set; }

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
                
                RefreshDisplayText();
            }
        }
    }

    public string Notes { get; set; }


    //Tree structure properties:        
    public BaseNode? Parent { get; private set; }
    public int NodeOrder { get; private set; }


    public List<BaseNode> Children { get; private set; } = new();



    public List<RPGElementDefinition> PotentialChildren
    {
        get
        {
            if (AssociatedElementDefinition != null)
            {
                return AssociatedElementDefinition.Children.Where(x => x.Name != "Character").ToList();
            }
            else
            {
                return new();
            }
        }
    }

    public bool Useable { get; private set; }
    

    public BaseNode(RPGElementDefinition element, bool isLoading, string notes = "", bool isFreebie = false)
    {
        //TODO - fix:
        AssociatedSystem = new();

        AssociatedElementDefinition = element;
        Name = element.Name ?? "";

        Useable = true;
        Notes = notes;
        NodeOrder = 1;

        Parent = null;
        _lastChildOrder = 0;
        
        IsFreebie = isFreebie;
        

        if (!isLoading && element.Freebies != null && element.Freebies.Count > 0)
        {
            foreach (Freebie freebie in element.Freebies)
            {
                RPGElementDefinition? subAttribute = freebie.SubAttribute;
                if (subAttribute != null)
                {
                    //Auto create freebie when creating new instance of this attribute:
                    AddChild(subAttribute.CreateNode("", AssociatedSystem, false, freebie.FreeLevels + freebie.RequiredLevels, freebie.FreeLevels, freebie.RequiredLevels, true));
                }
            }
        }

        
    }


    //Methods:
    public List<string> GetTypesForFilter()
    {
        List<string> tempList =  PotentialChildren.OrderBy(x=>x.Type.TypeOrder).Select(x=> x.Type.TypeName).Distinct().ToList();
        
        tempList.Insert(0, "All");
        return tempList;
    }

    public List<RPGElementDefinition> GetFilteredPotentialChildren(string filter)
    {
        List<RPGElementDefinition> selectedAttributeChildren = PotentialChildren;

        //LINQ Version:
        List<RPGElementDefinition> filteredAttList = selectedAttributeChildren
            .Where(a => (filter == "All" || filter == "" || a.Type.TypeName == filter))
            .OrderBy(a => a.Type.TypeOrder)
            .ThenBy(a => a.Name)
            .ToList();

        return filteredAttList;

    }


    public void AddChild(BaseNode child)
    {
        child.Parent = this;
        _lastChildOrder++;
        child.NodeOrder = _lastChildOrder;
        Children.Add(child);
    }

    public void Delete()
    {
        if (Parent != null) //Do not delete root node!
        {
            Parent.Children.Remove(this);
            Parent = null;
        }
    }


    public void MoveUp()
    {

        if (Parent != null)
        {
            int currentIndex = Parent.Children.IndexOf(this);
            if (currentIndex > 0)
            {
                BaseNode temp = Parent.Children[currentIndex - 1];

                int tempNodeOrder = NodeOrder;
                NodeOrder = temp.NodeOrder;
                temp.NodeOrder = tempNodeOrder;

                Parent.Children.RemoveAt(currentIndex);
                Parent.Children.Insert(currentIndex - 1, this);
            }
        }
    }

    public void MoveDown()
    {
        if (Parent != null)
        {
            int currentIndex = Parent.Children.IndexOf(this);
            if (currentIndex < Parent.Children.Count - 1)
            {
                BaseNode temp = Parent.Children[currentIndex + 1];
                int tempNodeOrder = NodeOrder;
                NodeOrder = temp.NodeOrder;
                temp.NodeOrder = tempNodeOrder;

                Parent.Children.RemoveAt(currentIndex);
                Parent.Children.Insert(currentIndex + 1, this);
            }
        }
    }

    public BaseNode AddChildAttribute(RPGElementDefinition element)
    {
        BaseNode Temp = element.CreateNode("", AssociatedSystem, false);
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
}
