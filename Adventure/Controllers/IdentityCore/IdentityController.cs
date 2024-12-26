using Microsoft.AspNetCore.Mvc;

namespace Adventure.Controllers.IdentityCore;

[ApiController]
[Route("[controller]")]
public class IdentityController : ControllerBase
{

    private readonly ILogger<IdentityController> _logger;

    public IdentityController(ILogger<IdentityController> logger)
    {
        _logger = logger;
    }
}
