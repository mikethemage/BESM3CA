namespace BESM3CAData.Model
{
    public interface IPointsDataNode
    {
        //Properties:
        string DisplayText { get; }
        int PointAdj { get; }

        //Methods:
        int Points { get; }
    }
}