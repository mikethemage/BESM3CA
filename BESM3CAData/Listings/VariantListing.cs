using BESM3CAData.Listings.Serialization;
using System;
using System.ComponentModel;

namespace BESM3CAData.Listings
{
    public class VariantListing : INotifyPropertyChanged
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int CostperLevel { get; set; }
        public string Desc { get; set; }
        public bool DefaultVariant { get; set; }
        public DataListing Attribute { get; set; }

        public string FullName
        {
            get
            {
                return $"{Attribute.Name} [{Name}]";
            }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                if (value != _isSelected)
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this,
                    new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public VariantListingSerialized Serialize()
        {
            return new VariantListingSerialized { ID = this.ID, Name = this.Name, CostperLevel = this.CostperLevel, Desc = this.Desc, DefaultVariant = this.DefaultVariant };
        }

    }
}
