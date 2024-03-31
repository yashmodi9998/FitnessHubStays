using FitnessHubStays.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace FitnessHubStays.Controllers
{
    public class BookingDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Retrieves a list of all bookings.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: All bookings in the database, including their details such as room id,user name,check in date,check out date.
        /// </returns>

        // GET: api/BookingData/ListBookings
        [HttpGet]
        [Route("api/BookingData/ListBookings")]
        public IEnumerable<BookingDto> ListBookings()
        {
            List<Booking> Bookings = db.Bookings.ToList();
            List<BookingDto> BookingDtos = new List<BookingDto>();

            Bookings.ForEach(booking => BookingDtos.Add(new BookingDto()
            {
                BookingID = booking.BookingID,
                RoomNumber = booking.Room.RoomNumber,
                UserID = booking.UserID,
                RoomID = booking.RoomID,
                CheckInDate = booking.CheckInDate,
                CheckOutDate = booking.CheckOutDate,
                TotalAmount = booking.TotalAmount
            }));

            return BookingDtos;
        }

        /// <summary>
        /// Adds a new booking to the system.
        /// </summary>
        /// <param name="booking">The booking object containing the information to add.</param>
        /// <returns>
        /// HEADER: 200 (OK) if the booking is added successfully.
        /// </returns>

        // POST: api/BookingData/AddBooking
        [ResponseType(typeof(Booking))]
        [HttpPost]
        [Route("api/BookingData/AddBooking")]
        public IHttpActionResult AddBooking(Booking booking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Bookings.Add(booking);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Retrieves information about a booking by its ID.
        /// </summary>
        /// <param name="bookingId">The ID of the room to retrieve.</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: The room information, including its details such as room number, type, price, status.
        /// </returns>

        // GET: api/BookingData/FindBooking/2
        [ResponseType(typeof(Booking))]
        [HttpGet]
        [Route("api/BookingData/FindBooking/{bookingId}")]
        public IHttpActionResult FindBooking(int bookingId)
        {
            Booking booking = db.Bookings.Find(bookingId);

            BookingDto bookingDto = new BookingDto()
            {
                BookingID = booking.BookingID,
                UserID = booking.UserID,
                RoomID = booking.RoomID,
                RoomNumber = booking.Room.RoomNumber,
                RoomType = booking.Room.RoomType,
                CheckInDate = booking.CheckInDate,
                CheckOutDate = booking.CheckOutDate,
                TotalAmount = booking.TotalAmount
            };

            if (bookingDto == null)
            {
                return NotFound(); // HTTP Status Code 404
            }
            return Ok(bookingDto); // return Booking Object
        }

        /// <summary>
        /// Updates the information of an existing booking in the system.
        /// </summary>
        /// <param name="id">The ID of the booking to update.</param>
        /// <param name="booking">The updated booking object with new information.</param>
        /// <returns>
        /// HEADER: 204 (No Content) if the booking is updated successfully.
        /// </returns>
        
        // POST: api/BookingData/UpdateBooking/2
        [ResponseType(typeof(Booking))]
        [HttpPost]
        [Route("api/BookingData/UpdateBooking/{id}")]
        public IHttpActionResult UpdateBooking(int id, Booking booking)
        {
            Debug.WriteLine("Check!! Is It Reached to the update method or not!!");

            // curl -H "Content-Type:application/json" -d @booking.json https://localhost:44302/api/bookingdata/updatebooking/6

            if (!ModelState.IsValid)
            {
                Debug.WriteLine("State is not valid for the Booking Model");
                return BadRequest(ModelState);
            }

            if (id != booking.BookingID)
            {
                Debug.WriteLine("ID is not match!!");
                Debug.WriteLine("Fetch ID: " + id);
                Debug.WriteLine("POST Parameter - Room ID: " + booking.RoomID);
                Debug.WriteLine("POST Parameter - User ID: " + booking.UserID);
                Debug.WriteLine("POST Parameter - Check In Date: " + booking.CheckInDate);
                Debug.WriteLine("POST Parameter - Check Out Date: " + booking.CheckOutDate);
                Debug.WriteLine("POST Parameter - Total Amount: " + booking.TotalAmount);
            }

            db.Entry(booking).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (!BookingExists(id))
                {
                    Debug.WriteLine("Booking not found");
                    return NotFound();
                }
                else
                {
                    Debug.WriteLine(e);
                    throw;
                }
            }
            Debug.WriteLine("Nothing is trigger in booking update function !!");
            return (StatusCode(HttpStatusCode.NoContent));
        }

        // Check Is Booking Already Exists and return boolean value either true or false
        private bool BookingExists(int id)
        {
            return db.Bookings.Count(e => e.BookingID == id) > 0;
        }

        /// <summary>
        /// Delete a booking from the system by its ID.
        /// </summary>
        /// <param name="id">The ID of the booking to delete.</param>
        /// <returns>
        /// HEADER: 200 (OK) if the booking is deleted successfully.
        /// HEADER: 404 (Not Found) if the booking with the given ID is not found.
        /// </returns>

        // POST: api/BookingData/DeleteBooking/2
        [ResponseType(typeof(Booking))]
        [HttpPost]
        [Route("api/BookingData/DeleteBooking/{id}")]
        public IHttpActionResult DeleteBooking(int id)
        {
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return NotFound();
            }
            db.Bookings.Remove(booking);
            db.SaveChanges();

            return Ok();
        }
    }
}
