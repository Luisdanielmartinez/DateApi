using chatApi.Data;
using chatApi.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chatApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        public readonly DataContext _context;
        public UsersController(DataContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task< ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            return await _context.Users.ToListAsync(); 
        }

        //api/users/3
        [HttpGet("{id}")]
        public async Task< ActionResult<AppUser>> GetUser(int id)
        {
            return await _context.Users.FindAsync(id); ;
        }
    }
}
