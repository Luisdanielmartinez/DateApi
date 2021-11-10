using AutoMapper;
using chatApi.Data;
using chatApi.DTOs;
using chatApi.Entities;
using chatApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace chatApi.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        public readonly IUserRepository _userRepository;
        public readonly IMapper _mapper;
        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            return Ok(await _userRepository.GetMembersAsync());
        }

        //api/users/3

        [HttpGet("{id}")]
        public async Task<ActionResult<MemberDto>> GetUser(int id)
        {

            var user= await _userRepository.GetUserByIdAsync(id);

            return _mapper.Map<MemberDto>(user);
        }
        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetByUserName(string userName)
        {
            return await _userRepository.GetMemberAsync(userName);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userRepository.GetUserByUserNameAsync(username);

            _mapper.Map(memberUpdateDto, user);
            _userRepository.Update(user);

            if (await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to update user");
            
        }
    }
}
