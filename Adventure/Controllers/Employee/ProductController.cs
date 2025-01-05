using Adventure.Controllers.IdentityCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Adventure.Controllers.Employee
{
    [Route("[controller]")]
    [ApiController]
    public class ProductController : IdentityController
    {
        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger) : base (logger) 
        {
            this._logger = logger;
        }

        [HttpGet("GetProudctList")]
        public async Task<int> GetProudctList()
        {
            return await Task.FromResult(0);
        }
    }
}
