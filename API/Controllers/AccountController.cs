using API.Data;
using API.DTO;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using API.Interfaces;
using API.Extensions;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _dataContext;
        private readonly ITokenService _tokenService;

        public AccountController(DataContext dataContext, ITokenService tokenService)
        {
            _dataContext = dataContext;
            _tokenService = tokenService;
        }

        [HttpPost("register")]  // https://localhost:5001/api/account/register
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExistsAsync(registerDto.Email))
            {
                return BadRequest("Email is already taken");
            }

            using var hmac = new HMACSHA512();

            var user = new User
            {
                Email = registerDto.Email,
                DisplayName = registerDto.DisplayName,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };

            await _dataContext.Users.AddAsync(user);

            await _dataContext.SaveChangesAsync();

            var userDto = user.ToDto(_tokenService);

            //var userDto = new UserDto
            //{
            //    Id = user.Id,
            //    DisplayName = user.DisplayName,
            //    Email = user.Email,
            //    Token = _tokenService.CreateToken(user)
            //};

            return userDto;            
        }

        [HttpPost("login")]  // https://localhost:5001/api/account/login
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _dataContext.Users.SingleOrDefaultAsync(x => x.Email.ToLower() == loginDto.Email.ToLower());

            if (user is null)
            {
                return Unauthorized("Invalid email");
            }

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                {
                    return Unauthorized("Invalid password");
                }
            }

            var userDto = user.ToDto(_tokenService);

            //var userDto = new UserDto
            //{
            //    Id = user.Id,
            //    DisplayName = user.DisplayName,
            //    Email = user.Email,
            //    Token = _tokenService.CreateToken(user)                
            //};

            return userDto;
        }

        private async Task<bool> UserExistsAsync(string email)
        {
            return await _dataContext.Users.AnyAsync(x => x.Email.ToLower() == email.ToLower());
        }
    }
}
