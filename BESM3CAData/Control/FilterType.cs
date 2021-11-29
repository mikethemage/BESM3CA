using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BESM3CAData.Control
{
    public class FilterType : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string TypeName { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }

        public FilterType()
        {
        }

        public FilterType(string typeName)
        {
            TypeName = typeName;
            IsSelected = false;
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this,
                    new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
