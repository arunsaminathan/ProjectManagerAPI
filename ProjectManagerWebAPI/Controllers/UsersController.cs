using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ProjectManagerWebAPI.Models;

namespace ProjectManagerWebAPI.Controllers
{
    public class UsersController : ApiController
    {
        private DBModels db = new DBModels();

        // GET: api/Users

        public IHttpActionResult GetUsers()
        {
            //return db.Users;
            return Ok((from s in db.Users
                       where s.Status == 1
                       select new Users
                       {
                           User_ID = s.User_ID,
                           First_Name = s.First_Name,
                           Last_Name = s.Last_Name,
                           Employee_ID = s.Employee_ID
                       }).AsEnumerable());
            
        }

        // GET: api/Users/5
        [ResponseType(typeof(User))]
        public IHttpActionResult GetUser(int id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT: api/Users/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUser(int id, User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.User_ID)
            {
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Users
        [ResponseType(typeof(User))]
        public IHttpActionResult PostUser(User user)
        {
            user.Status = 1;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Users.Add(user);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (UserExists(user.User_ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = user.User_ID }, user);
        }

        // DELETE: api/Users/5
        [ResponseType(typeof(User))]
        public IHttpActionResult DeleteUser(int id)
        {
            User user = db.Users.Find(id);
            user.Status = 0;

            //if (user == null)
            //{
            //    return NotFound();
            //}

            //db.Users.Remove(user);
            //db.SaveChanges();

            //return Ok(user);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.User_ID)
            {
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return db.Users.Count(e => e.User_ID == id) > 0;
        }
    }
    public partial class Users
    {
        public int User_ID { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public Nullable<int> Employee_ID { get; set; }
    }
}