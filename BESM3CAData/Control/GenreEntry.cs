using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BESM3CAData.Control
{
    public class GenreEntry : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public string GenreName { get; set; }

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

        public int Index { get; set; }

        public GenreEntry()
        {
        }

        public GenreEntry(string genreName, int index)
        {
            GenreName = genreName;
            Index = index;
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
