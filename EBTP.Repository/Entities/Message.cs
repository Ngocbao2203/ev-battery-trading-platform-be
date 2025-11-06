using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Repository.Entities
{
    public class Message : BaseEntity
    {
        public Guid ChatThreadId { get; set; }
        public Guid SenderId { get; set; }
        public string MessageText { get; set; }
        public bool IsRead { get; set; }
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public DateTime ReadAt { get; set; }
        public ChatThread ChatThread { get; set; }
    }
}
