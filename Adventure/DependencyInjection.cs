using NuGet.Protocol.Core.Types;
using EFDataAccess.Utlis.ImageHandling;
using EFDataAccess.EFDataProviders.ProductDataProvider;
using EFDataAccess.EFDataInterface.ProductsDataInterface;

namespace Adventure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
        {
            // Register your application services here
            services.AddScoped<IImageProcessingHelper, ImageProcessingHelper>();     
            services.AddScoped<IProductMangementDataInterface, ProdutMangentDataProvider > ();     

            return services;
        }
    }
}
