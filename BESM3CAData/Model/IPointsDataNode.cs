namespace BESM3CAData.Model
{
    public interface IPointsDataNode
    {        
        string DisplayText { get; }        
        int PointAdj { get; }
        int GetPoints();
    }
}