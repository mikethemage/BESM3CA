namespace BESM3CAData
{
    public class ListItems
    {        
        //Properties:
        public string Name { get; set; }
        public int ID { get; set; }
        public string Type { get; set; }


        //Constructors:
        public ListItems(string name, int id)
        {
            Name = name;
            ID = id;
            Type = null;
        }

        public ListItems(string name, int id, string type)
        {
            Name = name;
            ID = id;
            Type = type;
        }
    }
}
