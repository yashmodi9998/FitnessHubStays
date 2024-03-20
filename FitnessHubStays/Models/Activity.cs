using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FitnessHubStays.Models
{
    public class Activity
    {
        [Key]
        public int ActivityID { get; set; }
        public string ActivityName { get; set; }
        public DateTime ActivityDate { get; set; }
        public int ActivityDuration { get; set; }
        public decimal ActivityPrice { get; set; }
    }
}