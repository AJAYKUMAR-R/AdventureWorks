using Adventure.Controllers.IdentityCore;
using Adventure.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Adventure.Controllers.Customer
{
    [Route("[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ILogger<CustomerController> logger)
        {
            this._logger = logger;
        }

        [Authorize(Roles = RoleConstants.Customer)]
        [HttpGet("GetCutomer")]
        public string GetCutomer()
        {
            return "Cutomer has been authorized";
        }

    }
}
