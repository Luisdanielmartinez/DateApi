using AutoMapper;
using AutoMapper.QueryableExtensions;
using chatApi.Data;
using chatApi.DTOs;
using chatApi.Entities;
using chatApi.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chatApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        public readonly DataContext _context;
        public readonly IMapper _mapper;
        public UserRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<MemberDto> GetMemberAsync(string userName)
        {
            return await _context.Users
                .Where(x => x.UserName == userName)
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<MemberDto>> GetMembersAsync()
        {
            return await _context.Users
                
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        //public async Task<IEnumerable<AppUser>> GetUserAsync()
        //{
        //    return await _context.Users
        //        .Include(p => p.Photos)
        //        .ToListAsync();
        //}

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUserNameAsync(string username)
        {
            return await _context.Users
                       .Include(p => p.Photos)
                       .SingleOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }
    }
}
