using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FitnessHubStays.Models
{
    public class WorkoutSession
    {
        [Key]
        public int WorkoutSessionID { get; set; }
        public DateTime SessionDate { get; set; }
        public int Duration{ get; set; }
        public decimal SessionPrice { get; set; }
    }
}