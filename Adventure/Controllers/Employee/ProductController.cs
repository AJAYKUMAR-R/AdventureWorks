using Adventure.Controllers.IdentityCore;
using Microsoft.AspNetCore.Mvc;
using Adventure.Models.Product;
using EFDataAccess.EFModelSet.ProductsManagement;
using EFDataAccess.EFBusinessInterface.ProductBusinessInterface;
using EFDataAccess.Entities;
using System.Diagnostics;
using Microsoft.Build.Tasks.Deployment.Bootstrapper;


namespace Adventure.Controllers.Employee
{
    [Route("[controller]")]
    [ApiController]
    public class ProductController : IdentityController
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductBuissness _productMangement;

        public ProductController(ILogger<ProductController> logger, IProductBuissness productMangement) : base (logger) 
        {
            this._logger = logger;
            this._productMangement = productMangement;
        }

        [HttpPost("GetProductPerPage")]
        public IActionResult GetProudctList([FromBody] ProductPerPage productPerPage)
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
                     pageList = _productMangement.GetPaginatedProductsAsync(productPerPage.ProductName,
                        productPerPage.ProductModelName,productPerPage.ProductCategoryName,productPerPage.ColumnDirection,
                        productPerPage.SortColumnName,productPerPage.StartPrice
                        ,productPerPage.EndPrice,productPerPage.Page,productPerPage.PageSize, ref totalRecord
                        );
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex.Message);

                    return base.CreateInternalServerRequest("Pagination Data Successfully", ex.Message);
                }
            }

            return base.CreateOkRequest("Pagination Data Successfully", new
            {
                Changes = pageList,
                TotalRecord = totalRecord
            });

        }

        [HttpPost("InsertProduct")]
        public IActionResult InsertProduct([FromBody] RequestProductModel requestProduct)
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

            EFDataAccess.Entities.Product product = new EFDataAccess.Entities.Product();

            if (_productMangement is not null)
            {
                try
                {

                    product.ProductId = requestProduct.ProductId;
                    product.ProductNumber = requestProduct.ProductNumber;
                    product.Name = requestProduct.Name;
                    product.MakeFlag = Convert.ToBoolean(requestProduct.MakeFlag.Value);
                    product.FinishedGoodsFlag = Convert.ToBoolean(requestProduct.FinishedGoodsFlag);
                    product.Color = requestProduct.Color;
                    product.SafetyStockLevel = requestProduct.SafetyStockLevel;
                    product.ReorderPoint = requestProduct.ReorderPoint;
                    product.StandardCost = requestProduct.StandardCost;
                    product.ListPrice = requestProduct.ListPrice;
                    product.Size = requestProduct.Size;
                    product.SizeUnitMeasureCode = requestProduct.SizeUnitMeasureCode;
                    product.WeightUnitMeasureCode = requestProduct.WeightUnitMeasureCode;
                    product.Weight = requestProduct.Weight;
                    product.DaysToManufacture = requestProduct.DaysToManufacture;
                    product.ProductLine = requestProduct.ProductLine.ToString();
                    product.Class = requestProduct.Class.ToString();
                    product.Style = requestProduct.Style.ToString();
                    product.ProductSubcategoryId = requestProduct.ProductSubcategoryId;
                    product.ProductModelId = requestProduct.ProductModelId;
                    product.SellStartDate = requestProduct.SellStartDate;
                    product.SellEndDate = requestProduct.SellEndDate;
                    product.DiscontinuedDate = requestProduct.DiscontinuedDate;
                    product.Rowguid = requestProduct.Rowguid;
                    product.ModifiedDate = requestProduct.ModifiedDate;

                    _productMangement.AddProductDetails(product);

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return base.CreateInternalServerRequest("Issue in inserting the product", ex.Message);
                }
            }


            return base.CreateOkRequest("Product Got Inserted successfully", new
            {
                Changes = product
            });

        }


        [HttpPut("UpdateProduct")]
        public IActionResult UpdateProduct([FromBody] RequestProductModel requestProduct)
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

            EFDataAccess.Entities.Product product = new EFDataAccess.Entities.Product();

            if (_productMangement is not null)
            {
                try
                {
                   
                    product.ProductId = requestProduct.ProductId;
                    product.ProductNumber = requestProduct.ProductNumber;
                    product.Name = requestProduct.Name;
                    product.MakeFlag = Convert.ToBoolean(requestProduct.MakeFlag);
                    product.FinishedGoodsFlag = Convert.ToBoolean(requestProduct.FinishedGoodsFlag);
                    product.Color = requestProduct.Color;
                    product.SafetyStockLevel = requestProduct.SafetyStockLevel;
                    product.ReorderPoint = requestProduct.ReorderPoint;
                    product.StandardCost = requestProduct.StandardCost;
                    product.Size = requestProduct.Size;
                    product.ListPrice = requestProduct.ListPrice;
                    product.SizeUnitMeasureCode =requestProduct.SizeUnitMeasureCode;
                    product.WeightUnitMeasureCode = requestProduct.WeightUnitMeasureCode;
                    product.Weight = requestProduct.Weight;
                    product.DaysToManufacture = requestProduct.DaysToManufacture;
                    product.ProductLine = requestProduct.ProductLine.ToString();
                    product.Class = requestProduct.Class.ToString();
                    product.Style = requestProduct.Style.ToString();
                    product.ProductSubcategoryId = requestProduct.ProductSubcategoryId;
                    product.ProductModelId = requestProduct.ProductModelId;
                    product.SellStartDate = requestProduct.SellStartDate;
                    product.SellEndDate = requestProduct.SellEndDate;
                    product.DiscontinuedDate = requestProduct.DiscontinuedDate;
                    product.Rowguid = requestProduct.Rowguid;
                    product.ModifiedDate = requestProduct.ModifiedDate;

                    _productMangement.UpdateProduct(product);

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return base.CreateInternalServerRequest("Issue in updating the product", ex.Message);
                }
            }


            return base.CreateOkRequest("Product Got updated successfully", new
            {
                Changes = product
            });

        }

        [HttpDelete("RemoveProduct")]
        
        public  IActionResult DeleteProduct(int productID)
        {
            try
            {
                _productMangement.RemoveProduct(productID);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return base.CreateInternalServerRequest("Issue in deleting the product", ex.Message);
            }

            return base.CreateOkRequest("ProductDeleted successfully", new
            {
                Changes = productID
            });
        }

    }
}
