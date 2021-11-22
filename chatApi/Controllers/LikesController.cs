using AutoMapper;
using chatApi.DTOs;
using chatApi.Entities;
using chatApi.Extensions;
using chatApi.Helpers;
using chatApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chatApi.Controllers
{
    [Authorize]
    public class LikesController : BaseApiController
    {
        public readonly ILikesRepository _likesRepository;
        public readonly IMapper _mapper;
        public readonly IUserRepository _userRepository;
        public LikesController(ILikesRepository likesRepository, IMapper mapper, IUserRepository userRepository)
        {
            _likesRepository = likesRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }
       
        [HttpPost("{username}")]
        public async Task<ActionResult>AddLike(string username)
        { 
            //a quien le van a dar el like 
            var sourceUserId = User.GetUserId();
            var likedUser = await _userRepository.GetUserByUserNameAsync(username);
            var sourceUser = await _likesRepository.GetUserWithLikes(sourceUserId);

            if (likedUser == null) return NotFound();

            if (sourceUser.UserName == username) return BadRequest("You cannot like yourself");

            var userLike = await _likesRepository.GetUserLike(sourceUserId, likedUser.Id);

            if (userLike != null) return BadRequest("You already like this user");

            userLike = new UserLike
            {
                SourceUserId = sourceUserId,
                LikedUserId = likedUser.Id
            };
            sourceUser.LikedUsers.Add(userLike);

            if (await _userRepository.SaveAllAsync()) return Ok();

            return BadRequest("Failed to like user");
                 
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LikeDto>>> GetUserLikes([FromQuery]LikesParams likesParams)
        {
            likesParams.UserId = User.GetUserId();
            var users= await _likesRepository.GetUserLikes(likesParams);

            Response.AddPaginationHeader(users.CurrentPage, users.PageSize,
                users.TotalCount, users.TotalPages);

            return Ok(users);
        }
    }
}
