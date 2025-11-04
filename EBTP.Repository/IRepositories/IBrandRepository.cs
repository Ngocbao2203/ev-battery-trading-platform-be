using EBTP.Repository.Entities;
using EBTP.Repository.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Repository.IRepositories
{
    public interface IBrandRepository : IGenericRepository<Brand>
    {
        public Task<Brand?> GetByNameAsync(string name);
        public Task<Brand?> GetByNameAndTypeAsync(string name, BrandTypeEnum type);
    }
}
