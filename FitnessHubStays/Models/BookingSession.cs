using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FitnessHubStays.Models
{
    public class BookingSession
    {
        [Key]
        public int BookingSessionID {  get; set; }
        [Index("Booking Activity", IsUnique = true, Order = 1)]
        [ForeignKey("Bookings")]
        public int BookingID { get; set; }
        public virtual Booking Bookings { get; set; }

        [Index("Booking Activity", IsUnique = true, Order = 2)]
        [ForeignKey("Activities")]
        public int ActivityID { get; set; }
        public virtual Activity Activities { get; set; }

    }
}