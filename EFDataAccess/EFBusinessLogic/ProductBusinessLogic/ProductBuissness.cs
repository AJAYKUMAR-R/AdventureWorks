using EFDataAccess.EFBusinessInterface.ProductBusinessInterface;
using EFDataAccess.EFDataInterface.ProductsDataInterface;
using EFDataAccess.EFModelSet.ProductsManagement;
using EFDataAccess.Entities;
using Microsoft.Extensions.Logging;


namespace EFDataAccess.EFBusinessLogic.ProductBusinessLogic
{
    public class ProductBuissness : IProductBuissness
    {
        private readonly IProductMangementDataInterface _prodcutDataProvider;
        private readonly ILogger<ProductBuissness> _logger;
        public ProductBuissness(IProductMangementDataInterface productDataProvider ,ILogger<ProductBuissness> logger)
        {
            this._prodcutDataProvider = productDataProvider;
            this._logger = logger;
        }
        public void AddProductDetails(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product), "Product cannot be null.");

            var affectedRow = this._prodcutDataProvider.InsertProductDetails(product);

            if(affectedRow == 0)
            {
                throw new InvalidOperationException("Product couldn't be updated");
            }
        }

        IEnumerable<ProductModelSet> IProductBuissness.GetPaginatedProductsAsync(string productName, string productModelName, string prductCategoryName, bool columnDirection, string sortColumnName, decimal? startPrice, decimal? endPrice, int page, int pageSize, ref int totalRecord)
        {
            IEnumerable<ProductModelSet> pageList = null;
            if (this._prodcutDataProvider is not null)
            {
                try
                {
                    pageList = _prodcutDataProvider.GetProductListByPage(
                           productName,
                           productModelName, 
                           prductCategoryName, 
                           columnDirection,
                           sortColumnName, 
                           startPrice,
                           endPrice, 
                           page, 
                           pageSize, 
                           ref totalRecord
                       );
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }

            return pageList;
        }

        public async void RemoveProduct(int productId)
        {
            if (productId == 0)
                throw new ArgumentNullException(nameof(productId), "ProductID cann't be there.");

            var affectedRows = await _prodcutDataProvider.DeleteProductDetails(productId);

            if (affectedRows == false)
                throw new InvalidOperationException($"No product found with ID {productId} to delete.");
        }

        public void UpdateProduct(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product), "Product cannot be null.");

            var affectedRows = _prodcutDataProvider.UpdateProductDetauks(product);

            if (affectedRows == 0)
                throw new InvalidOperationException($"No product found with ID {product.ProductId} to update.");
        }

    }
}
