namespace BESM3CA
{
    class ListItems
    {        
        //Properties:
        public string DisplayMember { get; set; }
        public int ValueMember { get; set; }

        public string OptionalMember { get; set; }

        //Constructors:
        public ListItems(string Data, int Key)
        {
            DisplayMember = Data;
            ValueMember = Key;
            OptionalMember = null;
        }

        public ListItems(string Data, int Key, string Optional)
        {
            DisplayMember = Data;
            ValueMember = Key;
            OptionalMember = Optional;
        }

    }
}
