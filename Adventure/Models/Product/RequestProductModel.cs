using Adventure.Enums;
using Adventure.Models.Product.CustomAnnotationValidation;
using System.ComponentModel.DataAnnotations;

namespace Adventure.Models.Product
{
    public class RequestProductModel
    {
        /// <summary>
        /// Primary key for Product records.
        /// </summary>

        public int ProductId { get; set; }
        /// <summary>
        /// Name of the product.
        /// </summary>
        [StringLength(100, MinimumLength = 5, ErrorMessage = "ProductName must be between 5 to 50")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "")]
        [Display(Name = "ProductName")]
        public string Name { get; set; } = null!;
        /// <summary>
        /// Unique product identification number.
        /// </summary>
        [StringLength(50, MinimumLength = 5, ErrorMessage = "ProductNumber must be between 5 to 50")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "ProductNumber can't be empty")]
        public string ProductNumber { get; set; } = null!;
        /// <summary>
        /// 0 = Product is purchased, 1 = Product is manufactured in-house.
        /// </summary>
        [EnumDataType(typeof(MakeFlag), ErrorMessage = "MakeFlag status value is Invalid.")]
        [Required]
        public MakeFlag? MakeFlag { get; set; }
        /// <summary>
        /// 0 = Product is not a salable item. 1 = Product is salable.
        /// </summary>
        [EnumDataType(typeof(FinishedGoodsFlag), ErrorMessage = "FinishedGoodsFlag status values is Invalid.")]
        [Required]
        public FinishedGoodsFlag? FinishedGoodsFlag { get; set; }
        /// <summary>
        /// Product color.
        /// </summary>
        [StringLength(30, MinimumLength = 5, ErrorMessage = "Color must be between 5 to 50")]
        [Display(Name = "Color")]
        public string? Color { get; set; }
        /// <summary>
        /// Minimum inventory quantity. 
        /// </summary>
        [Required]
        [Range(1, short.MaxValue , ErrorMessage = "SafetyStockLevel shouldn't be zero")]
        public short SafetyStockLevel { get; set; }
        /// <summary>
        /// Inventory level that triggers a purchase order or work order. 
        /// </summary>
        [Required]
        [Range(1, short.MaxValue, ErrorMessage = "ReorderPoint shouldn't be zero")]
        public short ReorderPoint { get; set; }
        /// <summary>
        /// Standard cost of the product.
        /// </summary>
        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "StandardCost shouldn't be zero")]
        public decimal StandardCost { get; set; }
        /// <summary>
        /// Selling price.
        /// </summary>
        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "ListPrice shouldn't be zero")]
        public decimal ListPrice { get; set; }
        /// <summary>
        /// Product size.
        /// </summary>
        [StringLength(5, ErrorMessage = "Size must be between 5 to 50")]
        public string? Size { get; set; }
        /// <summary>
        /// Unit of measure for Size column.
        /// </summary>
        public string? SizeUnitMeasureCode { get; set; }
        /// <summary>
        /// Unit of measure for Weight column.
        /// </summary>
        public string? WeightUnitMeasureCode { get; set; }
        /// <summary>
        /// Product weight.
        /// </summary>
        [Range(1, double.MaxValue, ErrorMessage = "Weight shouldn't be zero")]
        public decimal? Weight { get; set; }
        /// <summary>
        /// Number of days required to manufacture the product.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "DaysToManufacture shouldn't be zero")]
        public int DaysToManufacture { get; set; }
        /// <summary>
        /// R = Road, M = Mountain, T = Touring, S = Standard
        /// </summary>
        [EnumDataType(typeof(ProductLine), ErrorMessage = "Invalid ProductLine value.")]
        public ProductLine? ProductLine { get; set; }
        /// <summary>
        /// H = High, M = Medium, L = Low
        /// </summary>
        [EnumDataType(typeof(Class), ErrorMessage = "Invalid Class value.")]
        public Class? Class { get; set; }
        /// <summary>
        /// W = Womens, M = Mens, U = Universal
        /// </summary>
        [EnumDataType(typeof(Style), ErrorMessage = "Invalid Style value.")]
        public Style? Style { get; set; }
        /// <summary>
        /// Product is a member of this product subcategory. Foreign key to ProductSubCategory.ProductSubCategoryID. 
        /// </summary>
        public int? ProductSubcategoryId { get; set; }
        /// <summary>
        /// Product is a member of this product model. Foreign key to ProductModel.ProductModelID.
        /// </summary>
        //[ForeignKeyExistsAttribute]
        public int? ProductModelId { get; set; }
        /// <summary>
        /// Date the product was available for sale.
        /// </summary>
        [Required]
        public DateTime SellStartDate { get; set; }
        /// <summary>
        /// Date the product was no longer available for sale.
        /// </summary>
        public DateTime? SellEndDate { get; set; }
        /// <summary>
        /// Date the product was discontinued.
        /// </summary>
        public DateTime? DiscontinuedDate { get; set; }
        /// <summary>
        /// ROWGUIDCOL number uniquely identifying the record. Used to support a merge replication sample.
        /// </summary>
        [Required]
        public Guid Rowguid { get; set; }
        /// <summary>
        /// Date and time the record was last updated.
        /// </summary>
        [Required]
        public DateTime ModifiedDate { get; set; }
    }
}
