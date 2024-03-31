using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitnessHubStays.Models.ViewModels
{
    public class EditBookingModel
    {
        public IEnumerable<RoomDto> Rooms { get; set; }
        public BookingDto Booking { get; set; }
    }
}