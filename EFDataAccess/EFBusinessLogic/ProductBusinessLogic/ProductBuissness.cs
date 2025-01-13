using EFDataAccess.EFBusinessInterface.ProductBusinessInterface;
using EFDataAccess.EFDataInterface.ProductsDataInterface;
using EFDataAccess.EFModelSet.ProductsManagement;
using EFDataAccess.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public int AddProductDetails(Product product)
        {
            throw new NotImplementedException();
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

        public int RemoveProduct(int productId)
        {
            throw new NotImplementedException();
        }

        public int UpdateProduct(Product product)
        {
            throw new NotImplementedException();
        }

    }
}
