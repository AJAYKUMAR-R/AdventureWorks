using EFDataAccess.EFModelSet.ProductsManagement;
using EFDataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace EFDataAccess.EFDataInterface.ProductsDataInterface
{
    public interface IProductMangementDataInterface
    {
        public int InsertProductDetails(Product product);

        public int UpdateProductDetauks(Product product);

        public int DeleteProductDetails(int productId);

        public IEnumerable<ProductModelSet> GetProductListByPage(
            string productName,
            string productModelName,
            string prductCategoryName,
            bool columnDirection,
            string sortColumnName,
            decimal? startPrice,
            decimal? endPrice,
            int page,
            int pageSize,
            ref int totalRecord
            );


    }
}
