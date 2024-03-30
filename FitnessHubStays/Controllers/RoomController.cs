using FitnessHubStays.Models;
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
    public class RoomController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static RoomController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44302/api/");
        }

        // GET: Room/List
        public ActionResult List()
        {
            // objective: communicate with our room data api to retrieve a list of rooms
            // curl https://localhost:44302/api/roomdata/listrooms

            string url = "roomdata/listrooms";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<RoomDto> rooms = response.Content.ReadAsAsync<IEnumerable<RoomDto>>().Result;

            return View(rooms);
        }

        // GET: Room/Add
        public ActionResult Add()
        {
            return View();
        }

        // POST: Room/Create
        [HttpPost]
        public ActionResult Create(Room room) {
            // Debug.WriteLine("the room json load is : ");

            // objective: add a new room into our system using the API
            // curl -H "Content-Type:application/json" -d @room.json https://localhost:44302/api/roomData/addroom 

            string url = "roomdata/addroom";
            string jsonpayload = jss.Serialize(room);

            // Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        public ActionResult Error()
        {
            return View();
        }

        // Get: Room/View/2
        public ActionResult View(int id)
        {
            // objective: communicate with our room data api to retrieve one room
            // curl https://localhost:44302/api/roomdata/findroom/{id}

            string url = "roomdata/findroom/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            // Debug.WriteLine("The response code is ");
            // Debug.WriteLine(response.StatusCode);

            RoomDto selectRoom = response.Content.ReadAsAsync<RoomDto>().Result;
            // Debug.WriteLine("room data retrived : ");
            // Debug.WriteLine(selectRoom.RoomNumber);

            return View(selectRoom);
        }

        // GET: Room/Edit/2
        public ActionResult Edit(int id)
        {
            // Get Particular Room information

            // objective: communicate with our room data api to retrieve one room
            // curl https://localhost:44302/api/roomdata/findroom/{id}

            string url = "roomdata/findroom/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            RoomDto selectRoom = response.Content.ReadAsAsync<RoomDto>().Result;

            return View(selectRoom);
        }

        // POST: Room/Update/2
        [HttpPost]
        public ActionResult Update(int id, Room room)
        {
            try
            {
                Debug.WriteLine("The room information is: ");
                Debug.WriteLine(room.RoomNumber);
                Debug.WriteLine(room.RoomType);
                Debug.WriteLine(room.RoomPrice);
                Debug.WriteLine(room.RoomStatus);

                // serialize update roomdata into JSON
                // Send the request to the API
                // POST: api/RoomData/UpdateRoom/{id}
                // Header : Content-Type: application/json

                string url = "roomdata/updateroom/" + id;
                string jsonpayload = jss.Serialize(room);
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

        // GET: Room/Delete/2
        public ActionResult Delete(int id)
        {
            // Get Particular Room information

            // objective: communicate with our room data api to retrieve one room
            // curl https://localhost:44302/api/roomdata/findroom/{id}

            string url = "roomdata/findroom/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            RoomDto selectRoom = response.Content.ReadAsAsync<RoomDto>().Result;

            return View(selectRoom);
        }

        // POST: Room/Remove/2
        [HttpPost]
        public ActionResult Remove(int id)
        {
            try
            {
                string url = "roomdata/deleteroom/" + id;
                HttpContent content = new StringContent("");
                content.Headers.ContentType.MediaType = "application/json";
                HttpResponseMessage response = client.PostAsync(url, content).Result;
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