namespace BESM3CA
{
    class ListItems
    {        
        //Properties:
        public string DisplayMember { get; set; }
        public int ValueMember { get; set; }

        public string OptionalMember { get; set; }

        //Constructors:
        public ListItems(string data, int key)
        {
            DisplayMember = data;
            ValueMember = key;
            OptionalMember = null;
        }

        public ListItems(string data, int key, string optional)
        {
            DisplayMember = data;
            ValueMember = key;
            OptionalMember = optional;
        }

    }
}
