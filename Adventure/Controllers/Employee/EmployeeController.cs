using Adventure.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Adventure.Controllers.IdentityCore;

[ApiController]
[Route("[controller]")]
public class EmployeeController : ControllerBase
{

    private readonly ILogger<EmployeeController> _logger;

    public EmployeeController(ILogger<EmployeeController> logger)
    {
        _logger = logger;
    }

    [Authorize(Roles = RoleConstants.Employee)]
    [HttpGet("GetEmployee")]
    public string GetEmployee()
    {
        return "Employee has been authorized";
    }
}
