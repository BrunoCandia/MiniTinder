using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")] // https://localhost:5001/api/members
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public MembersController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<User>>> GetMembers()
        {
            var users = await _dataContext.Users.ToListAsync();
            return Ok(users);
        }

        [HttpGet("{id}")] // https://localhost:5001/api/members/A0E8162D-152A-F111-87A6-E8039A9A54C4
        public async Task<ActionResult<User>> GetMember(Guid id)
        {
            var user = await _dataContext.Users.FindAsync(id);
            
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
    }
}
