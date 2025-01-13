using System.ComponentModel.DataAnnotations;

namespace Adventure.Models.Product
{
    public class ProductPerPage
    {
        [StringLength(500, ErrorMessage = "ProductName can be a maximum of 500 characters.")]
        public string? ProductName { get; set; }

        [StringLength(300, ErrorMessage = "ProductModelName can be a maximum of 300 characters.")]
        public string? ProductModelName { get; set; }

        [StringLength(300, ErrorMessage = "ProductCategoryName can be a maximum of 300 characters.")]
        public string? ProductCategoryName { get; set; }

        [Range(0, 1, ErrorMessage = "ColumnDirection must be 0 (DESC) or 1 (ASC).")]
        public bool ColumnDirection { get; set; } = true; // Default to ASC

        [StringLength(200, ErrorMessage = "SortColumnName can be a maximum of 200 characters.")]
        public string? SortColumnName { get; set; }

        [Range(0, long.MaxValue, ErrorMessage = "StartPrice must be a non-negative value.")]
        public decimal? StartPrice { get; set; }

        [Range(0, long.MaxValue, ErrorMessage = "EndPrice must be a non-negative value.")]
        public decimal? EndPrice { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Page must be at least 1.")]
        public int Page { get; set; } = 1; // Default to 1

        [Range(1, int.MaxValue, ErrorMessage = "PageSize must be at least 1.")]
        public int PageSize { get; set; } = 10; // Default to 10
    }
}
