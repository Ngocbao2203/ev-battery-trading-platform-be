using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Service.DTOs.ChatThread
{
    public class ChatThreadDTO
    {
        public Guid UserId { get; set; }
        public Guid ParticipantId { get; set; }
    }
}
