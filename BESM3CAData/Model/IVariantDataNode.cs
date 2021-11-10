using BESM3CAData.Listings;

namespace BESM3CAData.Model
{
    public interface IVariantDataNode
    {
        bool HasVariants { get; }
        VariantListing Variant { get; set; }
    }
}