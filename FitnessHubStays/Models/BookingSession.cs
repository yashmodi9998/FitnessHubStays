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
        [ForeignKey("Booking")]
        public int BookingID { get; set; }
        public virtual Booking Booking { get; set; }

        [Index("Booking Activity", IsUnique = true, Order = 2)]
        [ForeignKey("Activity")]
        public int ActivityID { get; set; }
        public virtual Activity Activity { get; set; }

    }
}