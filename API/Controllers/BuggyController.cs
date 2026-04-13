using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BuggyController : BaseApiController
    {
        [HttpGet("auth")]  // https://localhost:5001/api/buggy/auth
        public IActionResult GetAuth()
        {
            return Unauthorized("You are not authorized to access this resource");
        }

        [HttpGet("not-found")]  // https://localhost:5001/api/buggy/not-found
        public IActionResult GetNotFound()
        {
            return NotFound("Resource not found");
        }

        [HttpGet("server-error")]  // https://localhost:5001/api/buggy/server-error
        public IActionResult GetServerError()
        {
            throw new Exception("This is a server error");
        }

        [HttpGet("bad-request")]  // https://localhost:5001/api/buggy/bad-request
        public IActionResult GetBadRequest()
        {
            return BadRequest("This is a bad request");
        }
    }
}
