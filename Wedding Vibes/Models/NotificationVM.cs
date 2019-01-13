using System.Collections.Generic;

namespace WeddingVibes.Models
{
    public class NotificationVM
    {
        public int Count { get; set; }
        public List<Notification> Notifications { get; set; }
    }
}