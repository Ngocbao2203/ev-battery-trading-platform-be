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
    public class MessageRepository : GenericRepository<Message>, IMessageRepository
    {
        private readonly AppDbContext _context;
        public MessageRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Message?> GetMessageByIdAsync(Guid messageId)
        {
            return await _context.Message
                .FirstOrDefaultAsync(m => m.Id == messageId && !m.IsDeleted);
        }
    }
}
