using FitnessHubStays.Models;
using FitnessHubStays.Models.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace FitnessHubStays.Controllers
{
    public class BookingActivityController : Controller
    {

        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static BookingActivityController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44302/api/");
        }

        // GET: BookingActivity
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Error()
        {
            return View();
        }
        //only authorised user can view booked activity
        [Authorize(Roles = "Guest,Admin")]
        public ActionResult List()
        {
            // objective: communicate with booking activity data api to retrieve a list of bookings activity
            // curl https://localhost:44302/api/BookingActivity/ListBookingActivity

            string url = "BookingActivity/ListBookingActivity";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<BookingActivityDto> bookingactivity = response.Content.ReadAsAsync<IEnumerable<BookingActivityDto>>().Result;

        

            return View(bookingactivity);
        }
        //only authorised user can view perticular booked activity
        [Authorize(Roles = "Guest,Admin")]
        // GET: BookingActivity/View/2
        public ActionResult View(int id)
        {
            // objective: communicate with booking activity data api to retrieve one booking activity
            // curl https://localhost:44302/api/BookingActivity/FindBookingActivity/{id}

            string url = "BookingActivity/FindBookedActivityFromBooking/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<BookingActivityDto> bookingactivity = response.Content.ReadAsAsync<IEnumerable<BookingActivityDto>>().Result;

            return View(bookingactivity);
        }
        //only authorised user can add new activity for booking
        [Authorize(Roles = "Guest,Admin")]
        // GET: BookingActivity/Add
        public ActionResult Add()
        {
            string url = "ActivityData/ListActivities";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<ActivityDto> activityDtos= response.Content.ReadAsAsync<IEnumerable<ActivityDto>>().Result;
            ViewBag.BookingId = Request.QueryString["bookingid"];
            return View(activityDtos);
        }

        //only authorised user can add new activity for booking
        [Authorize(Roles = "Guest,Admin")]
        // POST: BookingActivity/Create
        [HttpPost]
        public ActionResult Create(BookingActivity bookingactivity)
        {
            try
            {
                // Retrieve the activity price from the form data
                decimal activityPrice = Convert.ToDecimal(Request.Form["ActivityPrice"]);
                Debug.WriteLine("Activity price: " + activityPrice);

                // Retrieve the bookingId from the form data
                int bookingId = Convert.ToInt16(Request.Form["BookingId"]);
                Debug.WriteLine("Booking ID: " + bookingId);

                // Retrieve the existing booking data from the API
                string bookingDataUrl = "bookingdata/findbooking/" + bookingId;
                HttpResponseMessage bookingDataResponse = client.GetAsync(bookingDataUrl).Result;

                // Check if the response is successful
                if (!bookingDataResponse.IsSuccessStatusCode)
                {
                    // Handle the error if retrieving booking data fails
                    return RedirectToAction("Error");
                }

                // Deserialize the booking data from the response
                BookingDto selectBooking = bookingDataResponse.Content.ReadAsAsync<BookingDto>().Result;

                // Update the booking's total amount with the new activity price
                decimal newTotalAmount = selectBooking.TotalAmount + activityPrice;
                selectBooking.TotalAmount = newTotalAmount;

                // Send a request to update the booking with the new total amount
                string updateBookingUrl = $"bookingdata/updatebooking/{bookingId}";
                string jsonPayload = jss.Serialize(selectBooking);
                HttpContent updateBookingContent = new StringContent(jsonPayload);
                updateBookingContent.Headers.ContentType.MediaType = "application/json";
                HttpResponseMessage updateBookingResponse = client.PostAsync(updateBookingUrl, updateBookingContent).Result;

                // Check if the booking update request is successful
                if (!updateBookingResponse.IsSuccessStatusCode)
                {
                    // Handle the error if updating booking fails
                    return RedirectToAction("Error");
                }

                // After successfully updating the booking, proceed to add the booking activity

                // Send a request to add the booking activity
                string addBookingActivityUrl = "BookingActivity/AddBookingActivity";
                string bookingActivityPayload = jss.Serialize(bookingactivity);
                HttpContent bookingActivityContent = new StringContent(bookingActivityPayload);
                bookingActivityContent.Headers.ContentType.MediaType = "application/json";
                HttpResponseMessage response = client.PostAsync(addBookingActivityUrl, bookingActivityContent).Result;

                // Check if adding the booking activity is successful
                if (response.IsSuccessStatusCode)
                {
                    // Redirect to the booking list page if successful
                    return Redirect("/Booking/View/"+ bookingId);
                }
                else
                {
                    // Handle the error if adding booking activity fails
                    return RedirectToAction("Error");
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the process
                Debug.WriteLine(ex.Message);
                return RedirectToAction("Error");
            }
        }

        //only authorised user can chnage activity for booking
        [Authorize(Roles = "Guest,Admin")]
        // GET: BookingActivity/Edit/2
        public ActionResult Edit(int id)
        {
            // Get Particular Activity information

            // objective: communicate with booking activity data api to retrieve one booking activity
            // curl https://localhost:44302/api/BookingActivity/FindBookingActivity/{id}

            string url = "BookingActivity/FindBookingActivity/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            BookingActivityDto bookingActivityDto = response.Content.ReadAsAsync<BookingActivityDto>().Result;


            // Get Particular Activity information
            string actUrl = "ActivityData/ListActivities";
            HttpResponseMessage actResponse = client.GetAsync(actUrl).Result;
            IEnumerable<ActivityDto> activityDtos = actResponse.Content.ReadAsAsync<IEnumerable<ActivityDto>>().Result;

            var viewModel = new EditBookingActivityModel
            {
                Activities = activityDtos,
                BookingActivity = bookingActivityDto
            };
            
            return View(viewModel);
        }

        //only authorised user can change activity for booking
        [Authorize(Roles = "Guest,Admin")]
        // POST: BookingActivity/Update/2
        [HttpPost]
        public ActionResult Update(int id, BookingActivity bookingActivity)
        {
            try
            {
                Debug.WriteLine("The booking Activity information is: ");
                Debug.WriteLine(bookingActivity.BookingActivityID);
                Debug.WriteLine(bookingActivity.BookingID);

                // serialize update booking Activity information into JSON
                // Send the request to the API
                // POST: api/BookingActivity/UpdateBookingActivity/{id}
                // Header : Content-Type: application/json

                string url = "BookingActivity/UpdateBookingActivity/" + id;
                string jsonpayload = jss.Serialize(bookingActivity);
                Debug.WriteLine(jsonpayload);

                HttpContent content = new StringContent(jsonpayload);
                content.Headers.ContentType.MediaType = "application/json";

                HttpResponseMessage response = client.PostAsync(url, content).Result;
                return Redirect("/Booking/View/"+ bookingActivity.BookingID);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return RedirectToAction("Error");
            }

        }
        //only authorised user can delete activity from booking
        [Authorize(Roles = "Guest,Admin")]
        // GET: BookingActivity/Delete/2
        public ActionResult Delete(int id)
        {
            // get Particular BookingActivity information

            // curl https://localhost:44302/api/BookingData/FindBookingActivity/{id}

            string url = "BookingActivity/FindBookingActivity/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            BookingActivityDto bookingActivityDto= response.Content.ReadAsAsync<BookingActivityDto>().Result;

            return View(bookingActivityDto);
        }
        //only authorised user can delete activity from booking
        [Authorize(Roles = "Guest,Admin")]
        // POST: BookingActivity/Remove/2
        [HttpPost]
        public ActionResult Remove(int id)
        {
            Debug.WriteLine("Into remove");
            try
            {
                string url = "BookingActivity/DeleteBookingActivity/" + id;
                HttpContent content = new StringContent("");
                content.Headers.ContentType.MediaType = "application/json";
                HttpResponseMessage response = client.PostAsync(url, content).Result;
                Debug.WriteLine("Resp--------------------"+response);

                if (response.IsSuccessStatusCode)
                {

                    // Retrieve the activity price from the form data
                    decimal activityPrice = Convert.ToDecimal(Request.Form["ActivityPrice"]);
                    Debug.WriteLine("Activity price: " + activityPrice);


                    // Retrieve the bookingId from the form data
                    int bookingId = Convert.ToInt16(Request.Form["BookingId"]);
                    Debug.WriteLine("Booking ID: " + bookingId);

                    string bookingDataUrl = "bookingdata/findbooking/" + bookingId;
                    HttpResponseMessage bookingDataResponse = client.GetAsync(bookingDataUrl).Result;

                    // Check if the response is successful
                    if (!bookingDataResponse.IsSuccessStatusCode)
                    {
                        // Handle the error if retrieving booking data fails
                        return RedirectToAction("Error");
                    }

                    // Deserialize the booking data from the response
                    BookingDto selectBooking = bookingDataResponse.Content.ReadAsAsync<BookingDto>().Result;

                    // Update the booking's total amount with the new activity price
                    decimal newTotalAmount = selectBooking.TotalAmount - activityPrice;
                    selectBooking.TotalAmount = newTotalAmount;

                    // Send a request to update the booking with the new total amount
                    string updateBookingUrl = $"bookingdata/updatebooking/{bookingId}";
                    string jsonPayload = jss.Serialize(selectBooking);
                    HttpContent updateBookingContent = new StringContent(jsonPayload);
                    updateBookingContent.Headers.ContentType.MediaType = "application/json";
                    HttpResponseMessage updateBookingResponse = client.PostAsync(updateBookingUrl, updateBookingContent).Result;


                    return Redirect("/Booking/View/" + bookingId);
                }
                else
                {
                    Debug.WriteLine("Error----------");
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