using FitnessHubStays.Models;
using FitnessHubStays.Models.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace FitnessHubStays.Controllers
{
    public class BookingController : Controller
    {
        private static readonly HttpClient client;
        private ApplicationUserManager userManager;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static BookingController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44302/api/");
        }

        public BookingController()
        {
            userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        }

        public string GetUserEmail(string userId)
        {
            var user = userManager.FindById(userId);
            if (user != null)
            {
                return user.Email;
            }
            return "N/A";
        }

        //only authorised user can view booking
        [Authorize(Roles = "Admin,Guest")]
        // GET: Booking/List
        public ActionResult List()
        {
            // objective: communicate with booking data api to retrieve a list of bookings
            // curl https://localhost:44302/api/bookingdata/listbookings

            string url = "bookingdata/listbookings";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<BookingDto> bookings = response.Content.ReadAsAsync<IEnumerable<BookingDto>>().Result;

            // Retrieve usernames for each booking
            foreach (var booking in bookings)
            {
                booking.UserEmail = GetUserEmail(booking.UserID);
            }

            return View(bookings);
        }
        //only authorised user can view booking
        [Authorize(Roles = "Admin,Guest")]
        // GET: Booking/View/2
        public ActionResult View(int id)
        {
            // objective: communicate with booking data api to retrieve one booking
            // curl https://localhost:44302/api/bookingdata/findbooking/{id}

            // Fetch booking details
            string bookingUrl = "bookingdata/findbooking/" + id;
            HttpResponseMessage bookingResponse = client.GetAsync(bookingUrl).Result;
            BookingDto selectBooking = bookingResponse.Content.ReadAsAsync<BookingDto>().Result;

            // Fetch booking activities
            string bookingActivityUrl = "BookingActivity/FindBookedActivityFromBooking/" + id;
            HttpResponseMessage bookingActivityResponse = client.GetAsync(bookingActivityUrl).Result;
            IEnumerable<BookingActivityDto> bookingActivities = bookingActivityResponse.Content.ReadAsAsync<IEnumerable<BookingActivityDto>>().Result;

            var viewModel = new ViewBookingActivityModel
            {
                Booking = selectBooking,
                BookingActivity = bookingActivities
            };

            return View(viewModel);
        }

        public ActionResult Error()
        {
            return View();
        }
        //only authorised user can add new booking
        [Authorize(Roles = "Guest,Admin")]
        // GET: Booking/Add
        public ActionResult Add()
        {
            // Get the user ID of the currently logged-in user
            string userId = User.Identity.GetUserId();

            // objective: communicate with our room data api to retrieve a list of rooms
            // curl https://localhost:44302/api/roomdata/listrooms

            string url = "roomdata/listrooms";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<RoomDto> rooms = response.Content.ReadAsAsync<IEnumerable<RoomDto>>().Result;

            // Pass the user ID to the view
            ViewBag.UserId = userId;

            return View(rooms);
        }
        //only authorised user can add new booking
        [Authorize(Roles = "Guest,Admin")]
        // POST: Booking/Create
        [HttpPost]
        public ActionResult Create(Booking booking)
        {
            string url = "bookingdata/addbooking";
            string jsonpayload = jss.Serialize(booking);

            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage responseMessage = client.PostAsync(url, content).Result;
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        //only authorised user can edit booking
        [Authorize(Roles = "Guest,Admin")]
        // GET: Booking/Edit/2
        public ActionResult Edit(int id)
        {
            // Get Particular Booking information

            // objective: communicate with booking data api to retrieve one booking
            // curl https://localhost:44302/api/bookingdata/findbooking/{id}

            string url = "bookingdata/findbooking/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            BookingDto selectBooking = response.Content.ReadAsAsync<BookingDto>().Result;

            // Get the user ID of the currently logged-in user
            string userId = User.Identity.GetUserId();

            // objective: communicate with our room data api to retrieve a list of rooms
            // curl https://localhost:44302/api/roomdata/listrooms

            string roomurl = "roomdata/listrooms";
            HttpResponseMessage roomresponse = client.GetAsync(roomurl).Result;
            IEnumerable<RoomDto> rooms = roomresponse.Content.ReadAsAsync<IEnumerable<RoomDto>>().Result;

            // Pass the user ID to the view
            ViewBag.UserId = userId;

            var viewModel = new EditBookingModel
            {
                Rooms = rooms,
                Booking = selectBooking
            };

            return View(viewModel);
        }

        //only authorised user can update booking
        [Authorize(Roles = "Guest,Admin")]
        // POST: Booking/Update/2
        [HttpPost]
        public ActionResult Update(int id, Booking booking)
        {
            try
            {
                Debug.WriteLine("The booking information is: ");
                Debug.WriteLine(booking.BookingID);
                Debug.WriteLine(booking.RoomID);
                Debug.WriteLine(booking.CheckInDate);
                Debug.WriteLine(booking.CheckOutDate);

                // serialize update bookingdata into JSON
                // Send the request to the API
                // POST: api/bookingdata/updatebooking/{id}
                // Header : Content-Type: application/json

                string url = "bookingdata/updatebooking/" + id;
                string jsonpayload = jss.Serialize(booking);
                Debug.WriteLine(jsonpayload);

                HttpContent content = new StringContent(jsonpayload);
                content.Headers.ContentType.MediaType = "application/json";

                HttpResponseMessage response = client.PostAsync(url, content).Result;
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return RedirectToAction("Error");
            }
        }

        //only authorised user can delete booking
        [Authorize(Roles = "Guest,Admin")]
        // GET: Booking/Delete/2
        public ActionResult Delete(int id)
        {
            // Get Particular Booking information

            // objective: communicate with booking data api to retrieve one booking
            // curl https://localhost:44302/api/bookingdata/findbooking/{id}

            string url = "bookingdata/findbooking/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            BookingDto selectBooking = response.Content.ReadAsAsync<BookingDto>().Result;

            return View(selectBooking);
        }
        //only authorised user can delete booking
        [Authorize(Roles = "Guest,Admin")]
        // POST: Booking/Remove/2
        [HttpPost]
        public ActionResult Remove(int id)
        {
            try
            {
                string url = "bookingdata/deletebooking/" + id;
                HttpContent content = new StringContent("");
                content.Headers.ContentType.MediaType = "application/json";
                HttpResponseMessage response = client.PostAsync(url, content).Result;
                Debug.WriteLine("Resp--------"+response);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("List");
                }
                else
                {
                    return RedirectToAction("Error");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return View();
            }
        }
    }
}