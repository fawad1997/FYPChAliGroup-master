using System.Collections.Generic;

namespace WeddingVibes.Models.AdminVM
{
    public class ChartVM
    {
        public List<string> LastSixMonths { get; set; }
        public List<int> ReservationCounts { get; set; }
    }
}
