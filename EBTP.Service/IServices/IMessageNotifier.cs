using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Service.IServices
{
    public interface IMessageNotifier
    {
        Task NotifyMessageSentAsync(Guid chatThreadId, Guid userId, Guid participantId, object payload, string type);
    }
}
