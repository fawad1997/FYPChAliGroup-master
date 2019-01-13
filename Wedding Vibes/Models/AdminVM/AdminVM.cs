using System.Collections.Generic;

namespace WeddingVibes.Models.AdminVM
{
        public class AdminVM
    {
        public List<Reservation.Reservation> Reservations { get; set; }
        public Reservation.Reservation Reservation { get; set; }
        public ChartVM ChartVm { get; set; }
    }
}
