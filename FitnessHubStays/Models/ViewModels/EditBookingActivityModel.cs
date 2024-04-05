using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitnessHubStays.Models.ViewModels
{
    public class EditBookingActivityModel
    {
        public IEnumerable<ActivityDto> Activities { get; set; }
        public BookingActivityDto BookingActivity { get; set; }
    }
}