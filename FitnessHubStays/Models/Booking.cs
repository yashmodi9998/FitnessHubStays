using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FitnessHubStays.Models
{
    public class Booking
    {

        [Key]
        public int BookingID { get; set; }
        [Index("Booking", IsUnique = true, Order = 1)]
        [ForeignKey("User")]
        public string UserID { get; set; }
        public virtual ApplicationUser User { get; set; }
        [Index("Booking", IsUnique = true, Order = 2)]
        [ForeignKey("Room")]
        public int RoomID { get; set; }
        public virtual Room Room { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public decimal TotalAmount { get; set; }
       
    }
}