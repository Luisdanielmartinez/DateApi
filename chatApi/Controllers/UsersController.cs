using chatApi.Data;
using chatApi.Entities;
using chatApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chatApi.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        public readonly IUserRepository _userRepository;
        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {

            return Ok(await _userRepository.GetUserAsync(););
        }

        //api/users/3

        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetUser(int id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }
        [HttpGet("{username}")]
        public async Task<ActionResult<AppUser>> GetByUserName(string userName)
        {
            return await _userRepository.GetUserByUserNameAsync(userName);
        }
    }
}
