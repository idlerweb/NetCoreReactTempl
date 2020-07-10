using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NetCoreReactTempl.Domain.Models;
using NetCoreReactTempl.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreReactTempl.DAL.Managers
{
    public class AuthManager : IAuthManager
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public AuthManager(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> EmailExist(string email) =>
            await _context.Users.AnyAsync(u =>
                u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)
            );

        public async Task<User> Get(string email) =>
            _mapper.Map<User>(
                await _context.Users.FirstOrDefaultAsync(u =>
                    u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)
                )
            );

        public async Task<User> Get(long id) =>
            _mapper.Map<User>(
                await _context.Users.FirstOrDefaultAsync(u =>
                    u.Id == id
                )
            );

        public async Task<IEnumerable<User>> Get() =>
            _mapper.Map<User[]>(
                await _context.Users.ToArrayAsync()
            );

        public async Task<User> Add(User user)
        {
            var entity = _mapper.Map<Entities.User>(user);
            _context.Users.Add(entity);
            await _context.SaveChangesAsync();

            return new User
            {
                Id = entity.Id,
                Email = entity.Email,
                IsAdmin = entity.IsAdmin,
                IsVerify = entity.IsVerify
            };
        }

        public async Task Delete(long id)
        {
            var entity = _context.Users.Single(u => u.Id == id);

            _context.Users.Remove(entity);

            await _context.SaveChangesAsync();
        }
    }
}
