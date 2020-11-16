using System;
using System.Collections.Generic;
using System.Text;

namespace BESM3CA
{
    class ListItems
    {
        public ListItems(string Data, int Key)
        {
            DisplayMember = Data;
            ValueMember = Key;
        }

        public string DisplayMember
        {
            get
            { return _DisplayMember; }
            set
            { _DisplayMember = value; }
        }

        public int ValueMember
        {
            get
            { return _ValueMember; }
            set
            { _ValueMember = value; }
        }

        private string _DisplayMember;
        private int _ValueMember;

    }
}
