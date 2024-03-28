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
    public class ActivityController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static ActivityController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44302/api/");
        }

        // GET: Activity/List
        public ActionResult List()
        {
            // objective: communicate with our activity data api to retrieve a list of activities
            // curl https://localhost:44302/api/activitydata/listactivities

            string url = "activitydata/listactivities";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<ActivityDto> activities = response.Content.ReadAsAsync<IEnumerable<ActivityDto>>().Result;

            return View(activities);
        }

        // GET: Activity/Add
        public ActionResult Add()
        {
            return View();
        }

        // POST: Activity/Create
        [HttpPost]
        public ActionResult Create(Activity activity)
        {
            // Debug.WriteLine("the activity json load is : ");

            // objective: add a new activity into our system using the API
            // curl -H "Content-Type:application/json" -d @activity.json https://localhost:44302/api/activitydata/addactivity 

            string url = "activitydata/addactivity";
            string jsonpayload = jss.Serialize(activity);

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

        // Get: Activity/View/2
        public ActionResult View(int id)
        {
            // objective: communicate with our activity data api to retrieve one activity
            // curl https://localhost:44302/api/activitydata/findactivity/{id}

            string url = "activitydata/findactivity/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            // Debug.WriteLine("The response code is ");
            // Debug.WriteLine(response.StatusCode);

            ActivityDto selectActivity = response.Content.ReadAsAsync<ActivityDto>().Result;
            // Debug.WriteLine("activity data retrived : ");
            // Debug.WriteLine(selectActivity.ActivityName);

            return View(selectActivity);
        }

        // GET: Activity/Edit/2
        public ActionResult Edit(int id)
        {
            // Get Particular Activity information

            // objective: communicate with our activity data api to retrieve one activity
            // curl https://localhost:44302/api/activitydata/findactivity/{id}

            string url = "activitydata/findactivity/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            ActivityDto selectActivity = response.Content.ReadAsAsync<ActivityDto>().Result;

            return View(selectActivity);
        }

        // POST: Activity/Update/2
        [HttpPost]
        public ActionResult Update(int id, Activity activity)
        {
            try
            {
                Debug.WriteLine("The activity information is: ");
                Debug.WriteLine(activity.ActivityName);
                Debug.WriteLine(activity.ActivityDay);
                Debug.WriteLine(activity.ActivityDuration);
                Debug.WriteLine(activity.ActivityPrice);
                Debug.WriteLine(activity.Status);

                // serialize update activitydata into JSON
                // Send the request to the API
                // POST: api/activitydata/updateactivity/{id}
                // Header : Content-Type: application/json

                string url = "activitydata/updateactivity/" + id;
                string jsonpayload = jss.Serialize(activity);
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
    }
}