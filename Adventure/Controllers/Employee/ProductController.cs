using Adventure.Controllers.IdentityCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EFDataAccess.EFDataInterface.ProductsDataInterface;
using EFDataAccess.EFDataProviders.ProductDataProvider;
using Adventure.Models.Product;
using EFDataAccess.EFModelSet.ProductsManagement;

namespace Adventure.Controllers.Employee
{
    [Route("[controller]")]
    [ApiController]
    public class ProductController : IdentityController
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductMangementDataInterface _productMangement;

        public ProductController(ILogger<ProductController> logger, IProductMangementDataInterface productMangement) : base (logger) 
        {
            this._logger = logger;
            this._productMangement = productMangement;
        }

        [HttpPost("GetProductPerPage")]
        public async Task<IActionResult> GetProudctList([FromBody] ProductPerPage productPerPage)
        {
            if (!ModelState.IsValid)
            {
                return base.CreateBadRequest("Validation failed", ModelState.ToDictionary(
                       kvp => kvp.Key,
                       kvp => kvp.Value?.Errors
                           .Select(e => e.ErrorMessage)
                           .ToList()
                   ));
            }

            IEnumerable<ProductModelSet> pageList = null;
            int totalRecord = 0;

            if (_productMangement is not null)
            {
               totalRecord = 0;
                try
                {
                     pageList = _productMangement.GetProductListByPage(productPerPage.ProductName,
                        productPerPage.ProductModelName,productPerPage.ProductCategoryName,productPerPage.ColumnDirection,
                        productPerPage.SortColumnName,productPerPage.StartPrice
                        ,productPerPage.EndPrice,productPerPage.Page,productPerPage.PageSize, ref totalRecord
                        );
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }

            return base.CreateOkRequest("Login succesfully", new
            {
                Changes = pageList,
                TotalRecord = totalRecord
            });

        }
    }
}
