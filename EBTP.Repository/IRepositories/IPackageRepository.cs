using EBTP.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Repository.IRepositories
{
    public interface IPackageRepository : IGenericRepository<Package>
    {
        public Task<Package?> GetByNameAsync(string name);
        Task<List<Package>> GetAllPackageAsync();
        Task<Package> GetPackageByIdAsync(Guid id);
    }
}
