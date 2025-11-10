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
    public class ChatThreadRepository : GenericRepository<ChatThread>, IChatThreadRepository
    {
        private readonly AppDbContext _context;

        public ChatThreadRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ChatThread?> GetChatThreadByIdAsync(Guid chatThreadId)
        {
            return await _context.ChatThread
                .Where(ct => ct.Id == chatThreadId && !ct.IsDeleted)
                .Select(ct => new ChatThread
                {
                    Id = ct.Id,
                    UserId = ct.UserId,
                    ParticipantId = ct.ParticipantId,
                    Status = ct.Status,
                    User = ct.User,
                    Messages = ct.Messages
                        .Where(m => !m.IsDeleted)
                        .Select(m => new Message
                        {
                            Id = m.Id,
                            SenderId = m.SenderId,
                            MessageText = m.MessageText,
                            IsRead = m.IsRead,
                            SentAt = m.SentAt,
                            ReadAt = m.ReadAt
                        })
                        .ToList(),
                    IsDeleted = ct.IsDeleted
                })
                .FirstOrDefaultAsync();
        }

        public async Task<List<ChatThread?>> GetChatThreadByUserIdAsync(Guid userId)
        {
            return await _context.ChatThread
                .Include(ct => ct.Messages)
                .Include(ct => ct.User)
                .Where(ct => (ct.ParticipantId == userId || ct.UserId == userId) && !ct.IsDeleted)
                .Select(ct => new ChatThread
                {
                    Id = ct.Id,
                    UserId = ct.UserId,
                    ParticipantId = ct.ParticipantId,
                    Status = ct.Status,
                    User = ct.User,
                    Messages = ct.Messages
                        .Where(m => !m.IsDeleted)
                        .Select(m => new Message
                        {
                            Id = m.Id,
                            SenderId = m.SenderId,
                            MessageText = m.MessageText,
                            IsRead = m.IsRead,
                            SentAt = m.SentAt,
                            ReadAt = m.ReadAt
                        })
                        .ToList(),
                    IsDeleted = ct.IsDeleted
                })
                .ToListAsync();
        }

        public async Task<ChatThread?> GetChatThreadByListingAsync(Guid listingId)
        {
            return await _context.ChatThread
                .Include(ct => ct.Messages)
                .Include(ct => ct.User)
                .Include(ct => ct.Listing).FirstOrDefaultAsync(ct => ct.ListingId == listingId && !ct.IsDeleted);
        }


        public async Task<ChatThread?> GetExistingChatThreadByIdAsync(Guid userId, Guid participantId, Guid listingId)
        {
            return await _context.ChatThread
                .Include(ct => ct.User)
                .FirstOrDefaultAsync(ct => ct.UserId == userId
                                        && ct.ParticipantId == participantId
                                        && ct.ListingId == listingId
                                        && !ct.IsDeleted);
        }
    }
}
