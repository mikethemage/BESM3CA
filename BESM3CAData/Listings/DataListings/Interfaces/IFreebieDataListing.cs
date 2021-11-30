namespace BESM3CAData.Listings
{
    public interface IFreebieDataListing
    {
        DataListing SubAttribute { get; set; }
        int SubAttributeLevel { get; set; }
        int SubAttributePointsAdj { get; set; }
    }
}