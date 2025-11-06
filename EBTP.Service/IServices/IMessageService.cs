using EBTP.Service.Abstractions.Shared;
using EBTP.Service.DTOs.ChatThread;
using EBTP.Service.DTOs.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Service.IServices
{
    public interface IMessageService
    {
        Task<Result<ViewChatThreadDTO>> GetChatThreadByIdAsync(Guid chatThreadId);
        Task<Result<List<ViewChatThreadDTO>>> GetChatThreadsByUserIdAsync(Guid userId);
        Task<Result<object>> SendMessageAsync(SendMessageDTO sendMessage);
        Task<Result<object>> StartThreadAsync(CreateChatThreadDTO chatThread);
        Task<Result<bool>> SoftDeleteMessageAsync(Guid messageId);
        Task<Result<bool>> SoftDeleteChatThreadAsync(Guid chatThreadId);
    }
}
