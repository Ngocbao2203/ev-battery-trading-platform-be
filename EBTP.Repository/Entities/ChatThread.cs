using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Repository.Entities
{
    public class ChatThread : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid ParticipantId { get; set; }
        public Guid ListingId { get; set; }
        public string Status { get; set; }

        public User User { get; set; }
        public ICollection<Message> Messages { get; set; }
        public Listing Listing { get; set; }
    }
}
