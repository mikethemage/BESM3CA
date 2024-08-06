namespace Triarch.BusinessLogic.Models.Definitions;

public class CalcStats
{
    public int Health { get; private set; }
    public int Energy { get; private set; }
    public int ACV { get; private set; }
    public int DCV { get; private set; }

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