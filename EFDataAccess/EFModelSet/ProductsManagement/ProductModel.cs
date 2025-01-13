

namespace EFDataAccess.EFModelSet.ProductsManagement
{
    public class ProductModelSet
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; } = String.Empty;
        public string ProductNumber { get; set; } = String.Empty;
        public int ProductModelID { get; set; }
        public string ModelName { get; set; } = String.Empty;
        public string ProductDescription { get; set; } = String.Empty;
        public int ProductCategoryID { get; set; }
        public string CategoryName { get; set; } = String.Empty;
        public decimal ListPrice { get; set; }
        public int ProductPhotoId { get; set; }
        public byte[]? ThumbNailPhoto { get; set; }
        public string? ThumbnailPhotoFileName { get; set; }
        public byte[]? LargePhoto { get; set; }
        public string? LargePhotoFileName { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
