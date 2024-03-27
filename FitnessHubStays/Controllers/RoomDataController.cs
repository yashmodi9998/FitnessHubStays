using FitnessHubStays.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace FitnessHubStays.Controllers
{
    public class RoomDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Retrieves a list of all rooms.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: All rooms in the database, including their details such as room number, type, price, status.
        /// </returns>

        // GET: api/RoomData/ListRooms
        [HttpGet]
        [Route("api/RoomData/ListRooms")]
        public IEnumerable<RoomDto> ListRooms()
        {
            List<Room> Rooms = db.Rooms.ToList();
            List<RoomDto> RoomDtos = new List<RoomDto>();

            Rooms.ForEach(room => RoomDtos.Add(new RoomDto()
            {
                RoomID = room.RoomID,
                RoomNumber = room.RoomNumber,
                RoomType = room.RoomType,
                RoomPrice = room.RoomPrice,
                RoomStatus = room.RoomStatus
            }));

            return RoomDtos;
        }

        /// <summary>
        /// Adds a new room to the system.
        /// </summary>
        /// <param name="room">The room object containing the information to add.</param>
        /// <returns>
        /// HEADER: 200 (OK) if the room is added successfully.
        /// </returns>

        // POST: api/RoomData/AddRoom
        [ResponseType(typeof(Room))]
        [HttpPost]
        [Route("api/RoomData/AddRoom")]
        public IHttpActionResult AddRoom(Room room)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Rooms.Add(room);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Retrieves information about a room by its ID.
        /// </summary>
        /// <param name="roomId">The ID of the room to retrieve.</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: The room information, including its details such as room number, type, price, status.
        /// </returns>

        // GET: api/RoomData/FindRoom/2
        [ResponseType(typeof(Room))]
        [HttpGet]
        [Route("api/RoomData/FindRoom/{roomId}")]
        public IHttpActionResult FindRoom(int roomId)
        {
            Room room = db.Rooms.Find(roomId);

            RoomDto roomDto = new RoomDto()
            {
                RoomID = room.RoomID,
                RoomNumber = room.RoomNumber,
                RoomType = room.RoomType,
                RoomPrice = room.RoomPrice,
                RoomStatus = room.RoomStatus
            };

            if (roomDto == null)
            {
                return NotFound(); // HTTP Status Code 404
            }
            return Ok(roomDto); // return Room Object
        }

        /// <summary>
        /// Updates the information of an existing room in the system.
        /// </summary>
        /// <param name="id">The ID of the room to update.</param>
        /// <param name="room">The updated room object with new information.</param>
        /// <returns>
        /// HEADER: 204 (No Content) if the room is updated successfully.
        /// </returns>

        // POST: api/RoomData/UpdateRoom/2
        [ResponseType(typeof(Room))]
        [HttpPost]
        [Route("api/RoomData/UpdateRoom/{id}")]
        public IHttpActionResult UpdateRoom(int id, Room room)
        {
            Debug.WriteLine("Check!! Is It Reached to the update method or not!!");

            // curl -H "Content-Type:application/json" -d @room.json https://localhost:44302/api/roomData/updateroom/6

            if (!ModelState.IsValid)
            {
                Debug.WriteLine("State is not valid for the Room Model");
                return BadRequest(ModelState);
            }

            if (id != room.RoomID)
            {
                Debug.WriteLine("ID is not match!!");
                Debug.WriteLine("Fetch ID: " + id);
                Debug.WriteLine("POST Parameter - Room ID: " + room.RoomID);
                Debug.WriteLine("POST Parameter - Room Number: " + room.RoomNumber);
                Debug.WriteLine("POST Parameter - Type Of Room: " + room.RoomType);
                Debug.WriteLine("POST Parameter - Price of Room: " + room.RoomPrice);
                Debug.WriteLine("POST Parameter - Status of Room: " + room.RoomStatus);
            }

            db.Entry(room).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (!RoomExists(id))
                {
                    Debug.WriteLine("Room not found");
                    return NotFound();
                }
                else
                {
                    Debug.WriteLine(e);
                    throw;
                }
            }
            Debug.WriteLine("Nothing is trigger in room update function !!");
            return (StatusCode(HttpStatusCode.NoContent));
        }

        // Check Is Room Already Exists and return boolean value either true or false
        private bool RoomExists(int id)
        {
            return db.Rooms.Count(e => e.RoomID == id) > 0;
        }
    }
}
