using Adventure.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Adventure.Utlis.Response
{
    public static class ResponseHelper
    {
        // Helper method for creating bad request responses
        public static IActionResult CreateBadRequest(string description, object data)
        {
            return new BadRequestObjectResult(new ResponseDetailsStatus
            {
                Success = false,
                Description = description,
                Data = data
            });
        }

        public static IActionResult CreateOkRequest(string description, object data)
        {
            return new OkObjectResult(new ResponseDetailsStatus
            {
                Success = true,
                Description = description,
                Data = data
            });
        }
    }
}
