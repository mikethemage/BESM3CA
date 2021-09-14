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

        public static CalcStats operator +(CalcStats a, CalcStats b)
        {
            return new CalcStats(a.Health + b.Health, a.Energy + b.Energy, a.ACV + b.ACV, a.DCV + b.DCV);
        }
    }
}