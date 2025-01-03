using Adventure.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Adventure.Controllers.IdentityCore;

[ApiController]
[Route("[controller]")]
public class AdminController : IdentityController
{

    private readonly ILogger<AdminController> _logger;

    public AdminController(ILogger<AdminController> logger) : base(logger)
    {
        _logger = logger;
    }

    [Authorize(Roles = RoleConstants.Admin)]
    [HttpGet("GetAdmin")]
    public string GetAdmin()
    {
        return "Admin has been authorized";
    }
}
