﻿using EFDataAccess.EFModelSet.ProductsManagement;
using EFDataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDataAccess.EFBusinessInterface.ProductBusinessInterface
{
    public interface  IProductBuissness
    {
        public int AddProductDetails(Product product);

        public int UpdateProduct(Product product);

        public int RemoveProduct(int productId);

        public IEnumerable<ProductModelSet> GetPaginatedProductsAsync(
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
