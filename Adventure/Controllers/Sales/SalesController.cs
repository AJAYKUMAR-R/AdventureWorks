using Microsoft.AspNetCore.Mvc;

namespace Adventure.Controllers.IdentityCore;

[ApiController]
[Route("api/[controller]")]
public class SalesController : ControllerBase
{

    private readonly ILogger<SalesController> _logger;

    public SalesController(ILogger<SalesController> logger)
    {
        _logger = logger;
    }
}
