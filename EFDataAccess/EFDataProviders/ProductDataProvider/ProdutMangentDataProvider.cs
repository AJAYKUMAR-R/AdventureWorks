using EFDataAccess.DBContext;
using EFDataAccess.EFDataInterface.ProductsDataInterface;
using EFDataAccess.EFModelSet.ProductsManagement;
using EFDataAccess.Entities;
using EFDataAccess.Utlis.ImageHandling;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Data;
using Adventure.Models.Product.TableFunctionResult;


namespace EFDataAccess.EFDataProviders.ProductDataProvider
{
    public class ProdutMangentDataProvider : IProductMangementDataInterface
    {
        private readonly IImageProcessingHelper _imageProcessor; 
        public ProdutMangentDataProvider(IImageProcessingHelper imageProcessor)
        {
            _imageProcessor = imageProcessor;
        }
        public async Task<bool> DeleteProductDetails(int productId)
        {
            using (var context = new AdventureWorks())
            {
                try
                {
                    var product = await context.Products.FindAsync(productId);
                    if (product is null)
                    {
                        throw new InvalidOperationException("No such as product out there in the database");
                    }

                    context.Products.Remove(product);

                    await context.SaveChangesAsync();

                    return true;
                }
                catch(Exception ex)
                {
                    //Just re throwing exception keep the stack Traces
                    throw;
                }

            }
        }

        public IEnumerable<ProductModelSet> GetProductListByPage(string productName, string productModelName, string prductCategoryName, bool columnDirection, string sortColumnName, decimal? startPrice, decimal? endPrice, int page, int pageSize, ref int totalRecord)
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

        public int InsertProductDetails(Product product)
        {
            try
            {
                using (var context = new AdventureWorks())
                {
                    var parameters = new[]
                        {
                        new SqlParameter("@Name", (object?)product.Name ?? DBNull.Value)
                        {
                            SqlDbType = System.Data.SqlDbType.NVarChar,
                            IsNullable = true
                        },
                        new SqlParameter("@ProductNumber", (object?)product.ProductNumber ?? DBNull.Value)
                        {
                            SqlDbType = System.Data.SqlDbType.NVarChar,
                            IsNullable = true
                        },
                        new SqlParameter("@MakeFlag", (object?)product.MakeFlag ?? DBNull.Value)
                        {
                            SqlDbType = System.Data.SqlDbType.Bit,
                            IsNullable = true
                        },
                        new SqlParameter("@FinishedGoodsFlag", (object?)product.FinishedGoodsFlag ?? DBNull.Value)
                        {
                            SqlDbType = System.Data.SqlDbType.Bit,
                            IsNullable = true
                        },
                        new SqlParameter("@Color", (object?)product.Color ?? DBNull.Value)
                        {
                            SqlDbType = System.Data.SqlDbType.NVarChar,
                            IsNullable = true
                        },
                        new SqlParameter("@SafetyStockLevel", (object?)product.SafetyStockLevel ?? DBNull.Value)
                        {
                            SqlDbType = System.Data.SqlDbType.SmallInt,
                            IsNullable = true
                        },
                        new SqlParameter("@ReorderPoint", (object?)product.ReorderPoint ?? DBNull.Value)
                        {
                            SqlDbType = System.Data.SqlDbType.SmallInt,
                            IsNullable = true
                        },
                        new SqlParameter("@StandardCost", (object?)product.StandardCost ?? DBNull.Value)
                        {
                            SqlDbType = System.Data.SqlDbType.Money,
                            IsNullable = true
                        },
                        new SqlParameter("@ListPrice", (object?)product.ListPrice ?? DBNull.Value)
                        {
                            SqlDbType = System.Data.SqlDbType.Money,
                            IsNullable = true
                        },
                        new SqlParameter("@Size", (object?)product.Size ?? DBNull.Value)
                        {
                            SqlDbType = System.Data.SqlDbType.NVarChar,
                            IsNullable = true
                        },
                        new SqlParameter("@SizeUnitMeasureCode", (object?)product.SizeUnitMeasureCode ?? DBNull.Value)
                        {
                            SqlDbType = System.Data.SqlDbType.NVarChar,
                            IsNullable = true
                        },
                        new SqlParameter("@WeightUnitMeasureCode", (object?)product.WeightUnitMeasureCode ?? DBNull.Value)
                        {
                            SqlDbType = System.Data.SqlDbType.NVarChar,
                            IsNullable = true
                        },
                        new SqlParameter("@Weight", (object?)product.Weight ?? DBNull.Value)
                        {
                            SqlDbType = System.Data.SqlDbType.Decimal,
                            IsNullable = true
                        },
                        new SqlParameter("@DaysToManufacture", (object?)product.DaysToManufacture ?? DBNull.Value)
                        {
                            SqlDbType = System.Data.SqlDbType.Int,
                            IsNullable = true
                        },
                        new SqlParameter("@ProductLine", (object?)product.ProductLine ?? DBNull.Value)
                        {
                            SqlDbType = System.Data.SqlDbType.NChar,
                            IsNullable = true
                        },
                        new SqlParameter("@Class", (object?)product.Class ?? DBNull.Value)
                        {
                            SqlDbType = System.Data.SqlDbType.NChar,
                            IsNullable = true
                        },
                        new SqlParameter("@Style", (object?)product.Style ?? DBNull.Value)
                        {
                            SqlDbType = System.Data.SqlDbType.NChar,
                            IsNullable = true
                        },
                        new SqlParameter("@ProductSubcategoryID", (object?)product.ProductSubcategoryId ?? DBNull.Value)
                        {
                            SqlDbType = System.Data.SqlDbType.Int,
                            IsNullable = true
                        },
                        new SqlParameter("@ProductModelID", (object?)product.ProductModelId ?? DBNull.Value)
                        {
                            SqlDbType = System.Data.SqlDbType.Int,
                            IsNullable = true
                        },
                        new SqlParameter("@SellStartDate", (object?)product.SellStartDate ?? DBNull.Value)
                        {
                            SqlDbType = System.Data.SqlDbType.DateTime,
                            IsNullable = true
                        },
                        new SqlParameter("@SellEndDate", (object?)product.SellEndDate ?? DBNull.Value)
                        {
                            SqlDbType = System.Data.SqlDbType.DateTime,
                            IsNullable = true
                        },
                        new SqlParameter("@DiscontinuedDate", (object?)product.DiscontinuedDate ?? DBNull.Value)
                        {
                            SqlDbType = System.Data.SqlDbType.DateTime,
                            IsNullable = true
                        }
                    };

                    // Log parameters for debugging
                    foreach (var param in parameters)
                    {
                        Console.WriteLine($"Parameter: {param.ParameterName}, Value: {param.Value}");
                    }

                    return  context.Database.ExecuteSqlRaw("EXEC [Production].[usp_InsertProduct]   @Name, @ProductNumber, @MakeFlag, @FinishedGoodsFlag, @Color, @SafetyStockLevel, @ReorderPoint, @StandardCost, @ListPrice, @Size, @SizeUnitMeasureCode, @WeightUnitMeasureCode, @Weight, @DaysToManufacture, @ProductLine, @Class, @Style, @ProductSubcategoryID, @ProductModelID, @SellStartDate, @SellEndDate, @DiscontinuedDate", parameters);
                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public int UpdateProductDetails(Product product)
        {
            try
            {
                using (var context = new AdventureWorks())
                {
                    var parameters = new[]
                     {
                            new SqlParameter("@ProductID", (object?)product.ProductId ?? DBNull.Value)
                            {
                                SqlDbType = System.Data.SqlDbType.NVarChar,
                                IsNullable = true
                            },
                            new SqlParameter("@Name", (object?)product.Name ?? DBNull.Value)
                            {
                                SqlDbType = System.Data.SqlDbType.NVarChar,
                                IsNullable = true
                            },
                            new SqlParameter("@ProductNumber", (object?)product.ProductNumber ?? DBNull.Value)
                            {
                                SqlDbType = System.Data.SqlDbType.NVarChar,
                                IsNullable = true
                            },
                            new SqlParameter("@MakeFlag", (object?)product.MakeFlag ?? DBNull.Value)
                            {
                                SqlDbType = System.Data.SqlDbType.Bit,
                                IsNullable = true
                            },
                            new SqlParameter("@FinishedGoodsFlag", (object?)product.FinishedGoodsFlag ?? DBNull.Value)
                            {
                                SqlDbType = System.Data.SqlDbType.Bit,
                                IsNullable = true
                            },
                            new SqlParameter("@Color", (object?)product.Color ?? DBNull.Value)
                            {
                                SqlDbType = System.Data.SqlDbType.NVarChar,
                                IsNullable = true
                            },
                            new SqlParameter("@SafetyStockLevel", (object?)product.SafetyStockLevel ?? DBNull.Value)
                            {
                                SqlDbType = System.Data.SqlDbType.SmallInt,
                                IsNullable = true
                            },
                            new SqlParameter("@ReorderPoint", (object?)product.ReorderPoint ?? DBNull.Value)
                            {
                                SqlDbType = System.Data.SqlDbType.SmallInt,
                                IsNullable = true
                            },
                            new SqlParameter("@StandardCost", (object?)product.StandardCost ?? DBNull.Value)
                            {
                                SqlDbType = System.Data.SqlDbType.Money,
                                IsNullable = true
                            },
                            new SqlParameter("@ListPrice", (object?)product.ListPrice ?? DBNull.Value)
                            {
                                SqlDbType = System.Data.SqlDbType.Money,
                                IsNullable = true
                            },
                            new SqlParameter("@Size", (object?)product.Size ?? DBNull.Value)
                            {
                                SqlDbType = System.Data.SqlDbType.NVarChar,
                                IsNullable = true
                            },
                            new SqlParameter("@SizeUnitMeasureCode", (object?)product.SizeUnitMeasureCode ?? DBNull.Value)
                            {
                                SqlDbType = System.Data.SqlDbType.NVarChar,
                                IsNullable = true
                            },
                            new SqlParameter("@WeightUnitMeasureCode", (object?)product.WeightUnitMeasureCode ?? DBNull.Value)
                            {
                                SqlDbType = System.Data.SqlDbType.NVarChar,
                                IsNullable = true
                            },
                            new SqlParameter("@Weight", (object?)product.Weight ?? DBNull.Value)
                            {
                                SqlDbType = System.Data.SqlDbType.Decimal,
                                IsNullable = true
                            },
                            new SqlParameter("@DaysToManufacture", (object?)product.DaysToManufacture ?? DBNull.Value)
                            {
                                SqlDbType = System.Data.SqlDbType.Int,
                                IsNullable = true
                            },
                            new SqlParameter("@ProductLine", (object?)product.ProductLine ?? DBNull.Value)
                            {
                                SqlDbType = System.Data.SqlDbType.NChar,
                                IsNullable = true
                            },
                            new SqlParameter("@Class", (object?)product.Class ?? DBNull.Value)
                            {
                                SqlDbType = System.Data.SqlDbType.NChar,
                                IsNullable = true
                            },
                            new SqlParameter("@Style", (object?)product.Style ?? DBNull.Value)
                            {
                                SqlDbType = System.Data.SqlDbType.NChar,
                                IsNullable = true
                            },
                            new SqlParameter("@ProductSubcategoryID", (object?)product.ProductSubcategoryId ?? DBNull.Value)
                            {
                                SqlDbType = System.Data.SqlDbType.Int,
                                IsNullable = true
                            },
                            new SqlParameter("@ProductModelID", (object?)product.ProductModelId ?? DBNull.Value)
                            {
                                SqlDbType = System.Data.SqlDbType.Int,
                                IsNullable = true
                            },
                            new SqlParameter("@SellStartDate", (object?)product.SellStartDate ?? DBNull.Value)
                            {
                                SqlDbType = System.Data.SqlDbType.DateTime,
                                IsNullable = true
                            },
                            new SqlParameter("@SellEndDate", (object?)product.SellEndDate ?? DBNull.Value)
                            {
                                SqlDbType = System.Data.SqlDbType.DateTime,
                                IsNullable = true
                            },
                            new SqlParameter("@DiscontinuedDate", (object?)product.DiscontinuedDate ?? DBNull.Value)
                            {
                                SqlDbType = System.Data.SqlDbType.DateTime,
                                IsNullable = true
                            }
                    };

                    return context.Database.ExecuteSqlRaw("EXEC Production.usp_UpdateProduct @ProductID, @Name, @ProductNumber, @MakeFlag, @FinishedGoodsFlag, @Color, @SafetyStockLevel, @ReorderPoint, @StandardCost, @ListPrice, @Size, @SizeUnitMeasureCode, @WeightUnitMeasureCode, @Weight, @DaysToManufacture, @ProductLine, @Class, @Style, @ProductSubcategoryID, @ProductModelID, @SellStartDate, @SellEndDate, @DiscontinuedDate", parameters);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public IEnumerable<ProductClass> GetClass()
        {
            using (var context = new AdventureWorks())
            {
                return context.fn_GetClass().ToList();
            }
        }
    }
}
