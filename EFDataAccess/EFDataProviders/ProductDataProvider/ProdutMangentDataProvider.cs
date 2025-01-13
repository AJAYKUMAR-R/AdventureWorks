using EFDataAccess.DBContext;
using EFDataAccess.EFDataInterface.ProductsDataInterface;
using EFDataAccess.EFModelSet.ProductsManagement;
using EFDataAccess.Entities;
using EFDataAccess.Utlis.ImageHandling;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace EFDataAccess.EFDataProviders.ProductDataProvider
{
    public class ProdutMangentDataProvider : IProductMangementDataInterface
    {
        private readonly IImageProcessingHelper _imageProcessor; 
        public ProdutMangentDataProvider(IImageProcessingHelper imageProcessor)
        {
            _imageProcessor = imageProcessor;
        }
        int IProductMangementDataInterface.DeleteProductDetails(int productId)
        {
            throw new NotImplementedException();
        }

        IEnumerable<ProductModelSet> IProductMangementDataInterface.GetProductListByPage(string productName, string productModelName, string prductCategoryName, bool columnDirection, string sortColumnName, decimal? startPrice, decimal? endPrice, int page, int pageSize, ref int totalRecord)
        {
            //Outparam meter
            var outputParam = new SqlParameter()
            {
                ParameterName = "@TotalRecord",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output
            };

            //Definig the parameter
            var sqlproductName = new SqlParameter("@ProductName", productName) { SqlDbType = System.Data.SqlDbType.NVarChar , IsNullable = true};
            var sqlproductModelName = new SqlParameter("@productModelName", productModelName) { SqlDbType = System.Data.SqlDbType.NVarChar, IsNullable = true }; ;
            var sqlprductCategoryName = new SqlParameter("@ProductCategoryName", prductCategoryName) { SqlDbType = System.Data.SqlDbType.NVarChar, IsNullable = true }; ;
            var sqlcolumnDirection = new SqlParameter("@ColumnDirection", columnDirection) { SqlDbType = System.Data.SqlDbType.Bit, IsNullable = false }; ;
            var sqlsortColumnName = new SqlParameter("@SortColumnName", sortColumnName) { SqlDbType = System.Data.SqlDbType.NVarChar, IsNullable = true }; ;
            var sqlstartPrice = new SqlParameter("@StartPrice", startPrice) { SqlDbType = System.Data.SqlDbType.BigInt, IsNullable = true }; ;
            var sqlendPrice = new SqlParameter("@EndPrice", endPrice) { SqlDbType = System.Data.SqlDbType.BigInt, IsNullable = true }; ;
            var sqlpage = new SqlParameter("@Page", page) { SqlDbType = System.Data.SqlDbType.Int, IsNullable = false }; ;
            var sqlpageSize = new SqlParameter("@PageSize", pageSize) { SqlDbType = System.Data.SqlDbType.Int, IsNullable = false }; ;

            // Use raw ADO.NET to execute the stored procedure
            var resultList = new List<ProductModelSet>();

            using (var context = new AdventureWorks())
            {
                using (var command = context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "Production.uspGetProductListPerPage";
                    command.CommandType = System.Data.CommandType.StoredProcedure;


                    command.Parameters.Add(sqlproductName);
                    command.Parameters.Add(sqlproductModelName);
                    command.Parameters.Add(sqlprductCategoryName);
                    command.Parameters.Add(sqlcolumnDirection);
                    command.Parameters.Add(sqlsortColumnName);
                    command.Parameters.Add(sqlstartPrice);
                    command.Parameters.Add(sqlendPrice);
                    command.Parameters.Add(sqlpage);
                    command.Parameters.Add(sqlpageSize);

                    //Adding the output parameter
                    command.Parameters.Add(outputParam);

                    // Open the connection if it's not already open
                    if (command != null && command?.Connection?.State != System.Data.ConnectionState.Open)
                    {
                        command.Connection.Open();
                    }

                    // Execute the command
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            
                            // Manually map the result to the DTO
                            resultList.Add(new ProductModelSet
                            {
                               ProductID = reader.GetInt32(0),
                               ProductName = reader.GetString(1),
                               ProductNumber = reader.GetString(2),
                               ProductModelID = reader.GetInt32(3),
                               ModelName = reader.GetString(4),
                               ProductDescription = reader.GetString(5),
                               ProductCategoryID = reader.GetInt32(6),
                               CategoryName = reader.GetString(7),
                               ListPrice = reader.GetDecimal(8),
                               ProductPhotoId = reader.GetInt32(9),
                               ThumbNailPhoto = _imageProcessor.GetImageData(reader,10),
                               ThumbnailPhotoFileName = reader.GetString(11),
                               LargePhoto = _imageProcessor.GetImageData(reader,12),
                               LargePhotoFileName = reader.GetString(13),
                               ModifiedDate = reader.GetDateTime(14)

                            });
                        }
                    }
                }

                
            }

            totalRecord = (int)outputParam.Value;

            return resultList;
            
        }

        int IProductMangementDataInterface.InsertProductDetails(Product product)
        {
            throw new NotImplementedException();
        }

        int IProductMangementDataInterface.UpdateProductDetauks(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
