using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Service.DTOs.Message
{
    public class SendMessageDTO
    {
        public Guid ChatThreadId { get; set; }
        public Guid SenderId { get; set; }
        public string MessageText { get; set; }
    }
}
