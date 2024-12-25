using Microsoft.AspNetCore.Mvc;

namespace Adventure.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IdentityController : ControllerBase
{

    private readonly ILogger<IdentityController> _logger;

    public IdentityController(ILogger<IdentityController> logger)
    {
        _logger = logger;
    }
}
