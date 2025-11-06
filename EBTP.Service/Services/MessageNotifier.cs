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

        public async Task NotifyMessageSentAsync(Guid chatThreadId, object payload, string type)
        {
            await _hubContext.Clients.Group(chatThreadId.ToString())
            .SendAsync("ReceivedMessage", new
            {
                type = type,
                payload = payload
            });

        }
    }
}
