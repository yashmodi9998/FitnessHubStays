using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FitnessHubStays.Models
{
    public class Room
    {
        [Key]
        public int RoomID { get; set; }
        public string RoomNumber { get; set; }
        public string RoomType { get; set; }
        public decimal RoomPrice { get; set; }
        public string RoomStatus { get; set; }
    }

    public class RoomDto
    {
        public int RoomID { get; set; }
        public string RoomNumber { get; set; }
        public string RoomType { get; set; }
        public decimal RoomPrice { get; set; }
        public string RoomStatus { get; set; }
    }
}