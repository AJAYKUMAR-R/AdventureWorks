using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;

namespace Adventure.Models.Product.CustomAnnotationValidation
{
    public class ForeignKeyExistsAttribute : ValidationAttribute
    {
        private readonly EFDataAccess.DBContext.AdventureWorks _dbContextType;
        //private readonly string _tableName;

        public ForeignKeyExistsAttribute()
        {
            //_dbContextType = dbContextType;
            //_tableName = tableName;
        }

        protected virtual ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult($"The field {validationContext.DisplayName} is required.");
            }

            if (_dbContextType == null)
            {
                throw new InvalidOperationException("DbContext is not available in the validation context.");
            }

            //converting the object to int
            var productString = value as string;
            var productModelID = Convert.ToInt32(value);

            var exists = _dbContextType.ProductModels.Where(obj => obj.ProductModelId == productModelID);

            if (exists is null)
            {
                return new ValidationResult($"The value '{value}' for {validationContext.DisplayName} does not exist in the productModel table.");
            }

            return ValidationResult.Success;
        }
    }
}
