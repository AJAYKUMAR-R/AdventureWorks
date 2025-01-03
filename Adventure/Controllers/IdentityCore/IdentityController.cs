using Adventure.DTO;
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


    // Helper method for creating bad request responses
    public IActionResult CreateBadRequest(string description, object data)
    {
        return new BadRequestObjectResult(new ResponseDetailsStatus
        {
            Success = false,
            Description = description,
            Data = data
        });
    }

    public  IActionResult CreateOkRequest(string description, object data)
    {
        return new OkObjectResult(new ResponseDetailsStatus
        {
            Success = true,
            Description = description,
            Data = data
        });
    }

    public  IActionResult CreateInternalServerRequest(string description, string data)
    {
        return StatusCode(500, new ProblemDetails
        {
            Status = 500,
            Title = "Internal Server Error",
            Detail = data
        });

    }
}
