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
    public class BookingActivityDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Retrieves a list of all booking activity.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: All booking activity in the database.
        /// </returns>

        [HttpGet]
        [Route("api/BookingActivity/ListBookingActivity")]
        public IEnumerable<BookingActivityDto> ListBookingActivity()
        {
            List<BookingActivity> BookingAcivities = db.BookingActivities.ToList();
            List<BookingActivityDto> BookingActivityDtos = new List<BookingActivityDto>();


            BookingAcivities.ForEach(bookingactivity => BookingActivityDtos.Add(new BookingActivityDto()
            {
                BookingActivityID = bookingactivity.BookingActivityID,
                BookingID = bookingactivity.BookingID,
                ActivityID = bookingactivity.ActivityID,
                ActivityName = bookingactivity.Activity.ActivityName,
                TotalAmount = bookingactivity.Booking.TotalAmount,
               ActivityDuration = bookingactivity.Activity.ActivityDuration,
               ActivityPrice = bookingactivity.Activity.ActivityPrice,
               StartTime = bookingactivity.Activity.StartTime,
               EndTime = bookingactivity.Activity.EndTime,
              
            }));

            return BookingActivityDtos;
        }

        /// <summary>
        /// Adds a new booking activity to the system.
        /// </summary>
        /// <param name="bookingactivity">The booking activity object containing the information to add.</param>
        /// <returns>
        /// HEADER: 200 (OK) if the booking activity is added successfully.
        /// </returns>
        /// 
        // POST: api/BookingActivity/AddBookingActivity
        [ResponseType(typeof(BookingActivity))]
        [HttpPost]
        [Route("api/BookingActivity/AddBookingActivity")]
        public IHttpActionResult AddBookingActivity(BookingActivity bookingactivity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.BookingActivities.Add(bookingactivity);
            db.SaveChanges();

            return Ok();
        }
        /// <summary>
        /// Retrieves information about a booking activity by its ID.
        /// </summary>
        /// <param name="bookingactivityId">The ID of the booking activity to retrieve.</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: Activity information.
        /// </returns>

        // GET: api/BookingActivity/FindBookingActivity/2
        [ResponseType(typeof(BookingActivity))]
        [HttpGet]
        [Route("api/BookingActivity/FindBookingActivity/{bookingactivityId}")]
        public IHttpActionResult FindBookingActivity(int bookingactivityId)
        {
            BookingActivity bookingactivity = db.BookingActivities.Find(bookingactivityId);

            BookingActivityDto bookingActivityDto = new BookingActivityDto()
            {
                BookingActivityID = bookingactivity.BookingActivityID,
                BookingID = bookingactivity.BookingID,
                ActivityID = bookingactivity.ActivityID,
                ActivityName = bookingactivity.Activity.ActivityName,
                ActivityDuration = bookingactivity.Activity.ActivityDuration,
                ActivityPrice = bookingactivity.Activity.ActivityPrice,
                StartTime = bookingactivity.Activity.StartTime,
                EndTime = bookingactivity.Activity.EndTime,

                TotalAmount = bookingactivity.Booking.TotalAmount,
            };

            if (bookingActivityDto == null)
            {
                return NotFound(); // HTTP Status Code 404
            }
            return Ok(bookingActivityDto); // return BookingActivity Object
        }

        /// <summary>
        /// Retrieves information about a booking activity by Booking ID.
        /// </summary>
        /// <param name="bookingId">The bookingID of the booking activity to retrieve.</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: booking information.
        /// </returns>

        // GET: api/BookingActivity/FindBookedActivityFromBooking/2
        [ResponseType(typeof(BookingActivity))]
        [HttpGet]
        [Route("api/BookingActivity/FindBookedActivityFromBooking/{bookingId}")]
        public IHttpActionResult FindBookedActivityFromBooking(int bookingId)
        {
            var bookingActivities = db.BookingActivities.Where(B => B.BookingID == bookingId).ToList();

            if (bookingActivities == null || bookingActivities.Count == 0)
            {
                return NotFound(); // HTTP Status Code 404
            }

            List<BookingActivityDto> bookingActivityDtos = new List<BookingActivityDto>();

            foreach (var bookingactivity in bookingActivities)
            {
                BookingActivityDto bookingActivityDto = new BookingActivityDto()
                {
                    BookingActivityID = bookingactivity.BookingActivityID,
                    BookingID = bookingactivity.BookingID,
                    ActivityID = bookingactivity.ActivityID,
                    ActivityName = bookingactivity.Activity.ActivityName ,
                    ActivityDuration = bookingactivity.Activity.ActivityDuration,
                    ActivityPrice = bookingactivity.Activity.ActivityPrice,
                    StartTime = bookingactivity.Activity.StartTime,
                    EndTime = bookingactivity.Activity.EndTime,

                    TotalAmount = bookingactivity.Booking.TotalAmount,
                };

                bookingActivityDtos.Add(bookingActivityDto);
            }

            return Ok(bookingActivityDtos);  // return BookingActivity Object
        }

        /// <summary>
        /// Updates the information of an existing booked activity in the system.
        /// </summary>
        /// <param name="id">The ID of the booking activity to update.</param>
        /// <param name="bookingactivity">The updated booking activity object with new information.</param>
        /// <returns>
        /// HEADER: 204 (No Content) if the booking activity is updated successfully.
        /// </returns>

        // POST: api/BookingActivity/UpdateBookingActivity/2
        [ResponseType(typeof(BookingActivity))]
        [HttpPost]
        [Route("api/BookingActivity/UpdateBookingActivity/{id}")]
        public IHttpActionResult UpdateBookingActivity(int id, BookingActivity bookingactivity)
        {
            Debug.WriteLine("Check!! Is It Reached to the update method or not!!");
            Debug.WriteLine(bookingactivity);

            // curl -H "Content-Type:application/json" -d @bookingactivity.json https://localhost:44302/api/BookingActivity/UpdateBookingActivity/2

            if (!ModelState.IsValid)
            {
                Debug.WriteLine("State is not valid for the Booking Model");
                return BadRequest(ModelState);
            }

            if (id != bookingactivity.BookingActivityID)
            {
                Debug.WriteLine("ID is not match!!");
                Debug.WriteLine("Fetch ID: " + id);
                Debug.WriteLine("POST Parameter - Booking ID: " + bookingactivity.BookingID);
                Debug.WriteLine("POST Parameter - BookingActivity ID: " + bookingactivity.BookingActivityID);
            }

            db.Entry(bookingactivity).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (!BookingActivityExists(id))
                {
                    Debug.WriteLine("Booking Activity not found");
                    return NotFound();
                }
                else
                {
                    Debug.WriteLine(e);
                    throw;
                }
            }
            Debug.WriteLine("Nothing is trigger in booking activity update function !!");
            return (StatusCode(HttpStatusCode.NoContent));
        }

        // Check Is Booking Activity Already Exists and return boolean value either true or false
        private bool BookingActivityExists(int id)
        {
            return db.BookingActivities.Count(e => e.BookingActivityID == id) > 0;
        }

        /// <summary>
        /// Delete a booking activity from the system for its user.
        /// </summary>
        /// <param name="id">The ID of the booking activity to delete.</param>
        /// <returns>
        /// HEADER: 200 (OK) if the booking activity is deleted successfully.
        /// HEADER: 404 (Not Found) if the booking activity with the given ID is not found.
        /// </returns>

        // POST: api/BookingActivity/DeleteBookingActivity/2
        [ResponseType(typeof(BookingActivity))]
        [HttpPost]
        [Route("api/BookingActivity/DeleteBookingActivity/{id}")]
        public IHttpActionResult DeleteBooking(int id)
        {
            BookingActivity bookingactivity = db.BookingActivities.Find(id);
            if (bookingactivity == null)
            {
                Debug.WriteLine("not found");
                return NotFound();
            }
            Debug.WriteLine(" found");
            db.BookingActivities.Remove(bookingactivity);
            db.SaveChanges();

            return Ok();
        }
    }
}

