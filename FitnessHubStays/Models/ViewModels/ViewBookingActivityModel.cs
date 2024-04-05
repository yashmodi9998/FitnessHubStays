using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitnessHubStays.Models.ViewModels
{
    public class ViewBookingActivityModel
    {
        public IEnumerable<BookingActivityDto> BookingActivity { get; set; }
        public BookingDto Booking { get; set; }
    }
}