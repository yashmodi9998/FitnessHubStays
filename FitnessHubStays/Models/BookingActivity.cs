﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FitnessHubStays.Models
{
    public class BookingActivity
    {
        [Key]
        public int BookingActivityID { get; set; }
        [Index("Booking Activity", IsUnique = true, Order = 1)]
        [ForeignKey("Booking")]
        public int BookingID { get; set; }
        public virtual Booking Booking { get; set; }

        [Index("Booking Activity", IsUnique = true, Order = 2)]
        [ForeignKey("Activity")]
        public int ActivityID { get; set; }
        public virtual Activity Activity { get; set; }

    }
    public class BookingActivityDto
    { 
        public int BookingActivityID { get; set; }
        public int BookingID { get; set; }
        public decimal TotalAmount { get;set; }

        public int ActivityID { get; set; }
        public string ActivityName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int ActivityDuration { get; set; }
        public decimal ActivityPrice { get; set; }


    }

    }