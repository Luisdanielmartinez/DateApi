using AutoMapper;
using chatApi.Data;
using chatApi.DTOs;
using chatApi.Entities;
using chatApi.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace chatApi.Controllers
{
    
    public class AccountController : BaseApiController
    {
        public readonly DataContext _context;
        public readonly ITokenService _tokenService;
        public readonly IMapper _mapper;
        public AccountController(DataContext context, ITokenService tokenService, IMapper mapper)
        {
            _context = context;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.UserName)) return BadRequest("UserName is taken");
            var user = _mapper.Map<AppUser>(registerDto);
            //create the sha1
            using var hmac = new HMACSHA512();


            user.UserName = registerDto.UserName.ToLower();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
            user.PasswordSalt = hmac.Key;

            await   _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return new UserDto 
            {
                UserName=user.UserName, 
                Token=_tokenService.CreateToken(user),
                KnownAs=user.KnowAs,
                PhotoUrl=user.Photos.FirstOrDefault(x=>x.IsMain).Url
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>>Login(LoginDto login)
        {

            var user = await _context.Users.SingleOrDefaultAsync(x=> x.UserName==login.UserName);

            if (user == null) return Unauthorized("Invalid userName");

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(login.Password));

            for (int i=0; i<computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password");
            }
            return new UserDto { UserName = user.UserName, Token = _tokenService.CreateToken(user) }; ;
        }

        private async Task<bool> UserExists(string userName)
        {
            return await _context.Users.AnyAsync(x => x.UserName == userName.ToLower());
        }
    }
} 
 