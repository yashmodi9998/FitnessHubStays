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
        [ForeignKey("Bookings")]
        public int BookingID { get; set; }
        public virtual Booking Bookings { get; set; }
        [ForeignKey("WorkoutSessions")]
        public int WorkoutSessionID { get; set; }
        public virtual WorkoutSession WorkoutSessions { get; set; } = null;

    }
}