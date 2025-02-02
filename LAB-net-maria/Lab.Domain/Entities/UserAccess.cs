using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab.Domain.Entities
{
    public class UserAccess
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int DeviceId { get; set; }
        public Guid UserId { get; set; }
        public string DeviceName { get; set; }
        public string IP { get; set; }
        public string Agent { get; set; }
        public DateTime AccessTime { get; set; } = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
        public User User { get; set; }
       
    }
}
