using Dating_App.Data;
using Dating_App.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace Dating_App.Controllers
{
    
    public class UsersController : CoreApiController
    {
        private readonly DataContext _dataContext;

        public UsersController(DataContext dataContext)
        {
            this._dataContext = dataContext;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            return await _dataContext.Users.ToListAsync<AppUser>();
        }
        [HttpGet("{id}")]

        public async Task<ActionResult<AppUser>> GetUser(int id)
        {
            return await _dataContext.FindAsync<AppUser>(id);
        }

    }
}
