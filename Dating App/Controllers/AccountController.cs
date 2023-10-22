using Dating_App.Data;
using Dating_App.DTOs;
using Dating_App.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Dating_App.Controllers
{
    public class AccountController : CoreApiController
    {
        private readonly DataContext _dataContext;

        public AccountController(DataContext dataContext)
        {
            this._dataContext = dataContext;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AppUser>> Register(RegisterDTO registerDTO)
        {
            if(await UserExists(registerDTO.UserName))
            {
                return BadRequest("User Name is already taken");
            }
            using var hmac = new HMACSHA512();

            var user = new AppUser
            {

                UserName = registerDTO.UserName.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password)),
                PasswordSalt = hmac.Key

            };
            await _dataContext.Users.AddAsync(user);
            await _dataContext.SaveChangesAsync();
            return Ok(user);

        }
        [HttpPost("login")]

        public async Task<ActionResult<AppUser>> LoginUser(LoginDTO loginDTO)
        {
            var user = await _dataContext.Users.FirstOrDefaultAsync(x => x.UserName == loginDTO.UserName);
            if (user == null)
            {
                return Unauthorized("Username does not exist");
            }
            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));

            for(int i=0; i<computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                {
                    return Unauthorized("Wrong Password");
                }
            }
            return Ok(user);
        }

        public async Task<bool>UserExists (string userName) {
        return await this._dataContext.Users.AnyAsync(x => x.UserName == userName.ToLower());
        }
    }
}
