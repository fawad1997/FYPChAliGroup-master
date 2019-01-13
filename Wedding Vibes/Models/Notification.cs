using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeddingVibes.Models
{
    public class Notification
    {
        public Notification()
        {
//            MarkAsRead = false;
        }

        public int Id { get; set; }
        public string Message { get; set; }
        public int RervationId { get; set; }
        public bool MarkAsRead { get; set; }
    }
}
