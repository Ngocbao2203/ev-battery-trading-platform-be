using EBTP.Repository.Hubs;
using EBTP.Service.IServices;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Service.Services
{
    public class MessageNotifier : IMessageNotifier
    {
        private readonly IHubContext<MessageHub> _hubContext;

        public MessageNotifier(IHubContext<MessageHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotifyMessageSentAsync(
        Guid chatThreadId,
        Guid userId,
        Guid participantId,
        object payload,
        string type)
        {
            var dto = new
            {
                type,
                chatThreadId,
                payload
            };

            // 1) Gửi cho tất cả connection đã JoinThread (nếu có)
            await _hubContext.Clients.Group(chatThreadId.ToString())
                .SendAsync("ReceivedMessage", dto);

            // 2) Gửi trực tiếp theo userId để bên kia nhận được
            if (MessageHub._ConnectionsMap.TryGetValue(userId, out var uConn))
            {
                await _hubContext.Clients.Client(uConn)
                    .SendAsync("ReceivedMessage", dto);
            }

            if (MessageHub._ConnectionsMap.TryGetValue(participantId, out var pConn))
            {
                await _hubContext.Clients.Client(pConn)
                    .SendAsync("ReceivedMessage", dto);
            }
        }
    }
}
