using EBTP.Repository.Entities;
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
    }
}
