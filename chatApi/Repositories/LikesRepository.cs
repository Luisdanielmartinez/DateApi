using chatApi.Data;
using chatApi.DTOs;
using chatApi.Entities;
using chatApi.Extensions;
using chatApi.Helpers;
using chatApi.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chatApi.Repositories
{
    public class LikesRepository : ILikesRepository
    {
        private readonly DataContext _context;
        public LikesRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<UserLike> GetUserLike(int sourceUserId, int likedUserId)
        {
            return await _context.Likes.FindAsync(sourceUserId, likedUserId);
        }

        public async Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams)
        {
            var users = _context.Users.OrderBy(u => u.UserName).AsQueryable();
            var likes = _context.Likes.AsQueryable();

            // usuario que me gustan
            if (likesParams.Predicate=="liked")
            {
                likes = likes.Where(like => like.SourceUserId == likesParams.UserId);
                users = likes.Select(like => like.LikedUser);
            }
            //usuarios que le dieron like a mi perfil 
          
            if (likesParams.Predicate == "likedBy")
            {
                likes = likes.Where(like => like.LikedUserId == likesParams.UserId);
                users = likes.Select(like => like.SourceUser);
            }

           var likedUsers= users.Select(user => new LikeDto
            {
                UserName=user.UserName,
                KnownAs=user.KnowAs,
                Age=user.DateOfBirth.CalculateAge(),
                photoUrl=user.Photos.FirstOrDefault(p => p.IsMain).Url,
                City=user.City,
                Id=user.Id
            });

            return await PagedList<LikeDto>.CreateAsync(likedUsers,
                likesParams.PageNumber,likesParams.PageSize);
        }

        public async Task<AppUser> GetUserWithLikes(int userId)
        {
            return await _context.Users
                 .Include(x => x.LikedUsers)
                 .FirstOrDefaultAsync(x => x.Id == userId);
        }
    }
}
