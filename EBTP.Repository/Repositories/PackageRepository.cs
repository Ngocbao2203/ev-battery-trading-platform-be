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
    public class PackageRepository : GenericRepository<Package>, IPackageRepository
    {
        private readonly AppDbContext _context;
        public PackageRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Package>> GetAllPackageAsync()
        {
            return await _context.Package.Where(p => !p.IsDeleted)
                .ToListAsync();
        }

        public async Task<Package> GetPackageByIdAsync(Guid id)
        {
            return await _context.Package.Where(p => !p.IsDeleted && p.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Package?> GetByNameAsync(string name)
        {
            var result = await _context.Package.Where(p => p.Name == name).FirstOrDefaultAsync();
            return result;
        }
    }
}
