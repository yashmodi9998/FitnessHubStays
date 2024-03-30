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
    public class ActivityDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Retrieves a list of all activities.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: All activites in the database, including their details such as activity name,activity start time and end time,activity duration, activity price.
        /// </returns>

        // GET: api/ActivityData/ListActivities
        
        [HttpGet]
        [Route("api/ActivityData/ListActivities")]
       
        public IEnumerable<ActivityDto> ListActivities()
        {
            List<Activity> Activities = db.Activities.ToList();
            List<ActivityDto> ActivityDtos = new List<ActivityDto>();

            Activities.ForEach(activity => ActivityDtos.Add(new ActivityDto()
            {
                ActivityID = activity.ActivityID,
                ActivityName = activity.ActivityName,
                StartTime = activity.StartTime,
                EndTime = activity.EndTime,
                ActivityDuration = activity.ActivityDuration,
                ActivityPrice = activity.ActivityPrice,
                Status = activity.Status
            }));

            return ActivityDtos;
        }

        /// <summary>
        /// Adds a new activity to the system.
        /// </summary>
        /// <param name="activity">The activity object containing the information to add.</param>
        /// <returns>
        /// HEADER: 200 (OK) if the activity is added successfully.
        /// </returns>

        // POST: api/ActivityData/AddActivity
        [ResponseType(typeof(Activity))]
        [HttpPost]
        [Route("api/ActivityData/AddActivity")]
      
        public IHttpActionResult AddActivity(Activity activity)
        {
            Debug.WriteLine(activity);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Activities.Add(activity);
            db.SaveChanges();
            
            return Ok();
        }

        /// <summary>
        /// Retrieves information about a activity by its ID.
        /// </summary>
        /// <param name="activityId">The ID of the activity to retrieve.</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: The activity information including their details such as activity name,activity start time and end time,activity duration, activity price.
        /// </returns>

        // GET: api/ActivityData/FindActivity/2
        [ResponseType(typeof(Activity))]
        [HttpGet]
        [Route("api/ActivityData/FindActivity/{activityId}")]
        public IHttpActionResult FindActivity(int activityId)
        {
            Activity activity = db.Activities.Find(activityId);

            ActivityDto activityDto = new ActivityDto()
            {
                ActivityID = activity.ActivityID,
                ActivityName = activity.ActivityName,
                StartTime = activity.StartTime,
                EndTime = activity.EndTime,
                ActivityDuration = activity.ActivityDuration,
                ActivityPrice = activity.ActivityPrice,
                Status = activity.Status
            };

            if (activityDto == null)
            {
                return NotFound(); // HTTP Status Code 404
            }
            return Ok(activityDto); // return Activity Object
        }

        /// <summary>
        /// Updates the information of an existing activity in the system.
        /// </summary>
        /// <param name="id">The ID of the activity to update.</param>
        /// <param name="activity">The updated activity object with new information.</param>
        /// <returns>
        /// HEADER: 204 (No Content) if the activity is updated successfully.
        /// </returns>

        // POST: api/ActivityData/UpdateActivity/2
        [ResponseType(typeof(Activity))]
        [HttpPost]
        [Route("api/ActivityData/UpdateActivity/{id}")]
        public IHttpActionResult UpdateActivity(int id, Activity activity)
        {
            Debug.WriteLine("Check!! Is It Reached to the update method of activity or not!!");

            // curl -H "Content-Type:application/json" -d @activity.json https://localhost:44302/api/activityData/updateactivity/6

            if (!ModelState.IsValid)
            {
                Debug.WriteLine("State is not valid for the Activity Model");
                return BadRequest(ModelState);
            }

            if (id != activity.ActivityID)
            {
                Debug.WriteLine("ID is not match!!");
                Debug.WriteLine("Fetch ID: " + id);
                Debug.WriteLine("POST Parameter - Activity ID: " + activity.ActivityID);
                Debug.WriteLine("POST Parameter - Activity Name: " + activity.ActivityName);
                Debug.WriteLine("POST Parameter - Start time Of Activity: " + activity.StartTime);
                Debug.WriteLine("POST Parameter - End time Of Activity: " + activity.EndTime);
                Debug.WriteLine("POST Parameter - Duration Of Activity: " + activity.ActivityDuration);
                Debug.WriteLine("POST Parameter - Activity Price: " + activity.ActivityPrice);
            }

            db.Entry(activity).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (!ActivityExists(id))
                {
                    Debug.WriteLine("Activity not found");
                    return NotFound();
                }
                else
                {
                    Debug.WriteLine(e);
                    throw;
                }
            }
            Debug.WriteLine("Nothing is trigger in activity update function !!");
            return (StatusCode(HttpStatusCode.NoContent));
        }

        // Check Is Activity Already Exists and return boolean value either true or false
        private bool ActivityExists(int id)
        {
            return db.Activities.Count(e => e.ActivityID == id) > 0;
        }

        /// <summary>
        /// Delete an activity from the system by its ID.
        /// </summary>
        /// <param name="id">The ID of the activity to delete.</param>
        /// <returns>
        /// HEADER: 200 (OK) if the activity is deleted successfully.
        /// HEADER: 404 (Not Found) if the activity with the given ID is not found.
        /// </returns>

        // POST: api/ActivityData/DeleteActivity/2
        [ResponseType(typeof(Activity))]
        [HttpPost]
        [Route("api/ActivityData/DeleteActivity/{id}")]
        public IHttpActionResult DeleteActivity(int id)
        {
            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return NotFound();
            }
            db.Activities.Remove(activity);
            db.SaveChanges();

            return Ok();
        }
    }
}
