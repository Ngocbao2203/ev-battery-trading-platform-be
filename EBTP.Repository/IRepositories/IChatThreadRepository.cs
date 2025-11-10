using EBTP.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Repository.IRepositories
{
    public interface IChatThreadRepository : IGenericRepository<ChatThread>
    {
        Task<ChatThread?> GetChatThreadByIdAsync(Guid chatThreadId);
        Task<List<ChatThread?>> GetChatThreadByUserIdAsync(Guid userId);
        Task<ChatThread?> GetExistingChatThreadByIdAsync(Guid userId, Guid participantId, Guid listingId);
        Task<ChatThread?> GetChatThreadByListingAsync(Guid listingId);

    }
}
