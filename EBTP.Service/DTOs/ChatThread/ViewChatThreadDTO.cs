using EBTP.Service.DTOs.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Service.DTOs.ChatThread
{
    public class ViewChatThreadDTO
    {
        public Guid Id { get; set; }
        public Guid ParticipantId { get; set; }
        public Guid UserId { get; set; }
        public string Status { get; set; }
        public List<ViewMessageDTO> Messages { get; set; }
    }
}
