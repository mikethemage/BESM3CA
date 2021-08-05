namespace BESM3CA.Model
{
    public class CalcStats
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
    }
}