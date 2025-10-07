using EBTP.Repository.Data;
using EBTP.Repository.Entities;
using EBTP.Repository.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Repository.Repositories
{
    public class AuthRepository : GenericRepository<User>, IAuthRepository
    {
        private readonly AppDbContext _context;

        public AuthRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> DeleteRefreshToken(Guid userId)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.Id == userId);
            user.RefreshToken = "";
            return await SaveChange();
        }

        public async Task<User> GetRefreshToken(string refreshToken)
        {
            return await _context.User.Include(r => r.Role).FirstOrDefaultAsync(r => r.RefreshToken == refreshToken);
        }
        public async Task<bool> UpdateRefreshToken(Guid userId, string refreshToken)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.Id == userId);
            user.RefreshToken = refreshToken;
            return await SaveChange();
        }

        public async Task<bool> SaveChange()
        {
            var save = await _context.SaveChangesAsync();
            return save > 0 && true;
        }

    }
}
