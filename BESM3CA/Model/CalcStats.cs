namespace BESM3CA.Model
{
    class CalcStats
    {
        public int Health;
        public int Energy;
        public int ACV;
        public int DCV;

        public CalcStats(int h, int e, int a, int d)
        {
            Health = h;
            Energy = e;
            ACV = a;
            DCV = d;
        }

        public static CalcStats GetStats(NodeData Node)
        {
            CalcStats stats;

            if (Node.GetType() == typeof(AttributeData))
            {
                stats = new CalcStats(0, 0, 0, 0);
            }
            else if (Node.GetType() == typeof(CharacterData))
            {
                stats = new CalcStats(((CharacterData)Node).BaseHealth,
                    ((CharacterData)Node).BaseEnergy,
                    ((CharacterData)Node).BaseCV,
                    ((CharacterData)Node).BaseCV);
            }
            else
            {
                //error!
                return null;
            }

            CalcStats temp;
            NodeData current = Node.Children;
            while (current!=null)
            {
                if (current.GetType() == typeof(AttributeData))
                {
                    switch (((AttributeData)current).Name)
                    {
                        case "Tough":
                            temp = new CalcStats(((AttributeData)current).Level * 5, 0, 0, 0);
                            break;
                        case "Energy Bonus":
                            temp = new CalcStats(0, ((AttributeData)current).Level * 5, 0, 0);
                            break;
                        case "Attack Combat Mastery":
                            temp = new CalcStats(0, 0, ((AttributeData)current).Level, 0);
                            break;
                        case "Defence Combat Mastery":
                            temp = new CalcStats(0, 0, 0, ((AttributeData)current).Level);
                            break;
                        default:
                            temp = new CalcStats(0, 0, 0, 0);
                            break;
                    }

                    if (temp.ACV > 0 || temp.DCV > 0 || temp.Energy > 0 || temp.Health > 0)
                    {
                        NodeData child = current.Children;
                        while(child!=null)
                        {
                            if (child.GetType() == typeof(AttributeData))
                            {   
                                if (((AttributeData)child).AttributeType == "Restriction")
                                {
                                    temp = new CalcStats(0, 0, 0, 0);
                                    break;
                                }
                            }
                            child = child.Next;
                        }

                        stats.Health += temp.Health;
                        stats.Energy += temp.Energy;
                        stats.ACV += temp.ACV;
                        stats.DCV += temp.DCV;
                    }
                }
                current = current.Next;
            }
            return stats;
        }
    }
}