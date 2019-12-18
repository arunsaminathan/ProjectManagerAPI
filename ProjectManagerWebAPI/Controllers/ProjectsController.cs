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
    public class ProjectsController : ApiController
    {
        private DBModels db = new DBModels();

        public partial class Projects
        {
            public int Project_ID { get; set; }
            public string Project_Name { get; set; }
            public Nullable<System.DateTime> Start_Date { get; set; }
            public Nullable<System.DateTime> End_Date { get; set; }
            public Nullable<int> Priority { get; set; }
            public int User_ID { get; set; }
            public int TaskCount { get; set; }
            public string First_Name { get; set; }
        }
        // GET: api/Projects
        public IQueryable<Projects> GetProjects()
        {
            return from s in db.Projects
                   join cs in db.Users on s.User_ID equals cs.User_ID
                   join t in db.Tasks on s.Project_ID equals t.Project_ID
                   //where s.Date >= DateTime.Parse("02/12/2014) && s.Date <= DateTime.Parse("03 / 12 / 2014")
                   select new Projects
                   {
                       //{
                       Project_ID = s.Project_ID,
                       Project_Name = s.Project_Name,
                       Start_Date = s.Start_Date,
                       End_Date = s.End_Date,
                       Priority = s.Priority,
                       User_ID = s.User_ID,
                       TaskCount= t.Task_ID,
                       First_Name = cs.First_Name
                   };
           // return db.Projects.OrderBy(e=>e.Project_ID);
        }

        // GET: api/Projects/5
        [ResponseType(typeof(Project))]
        public IHttpActionResult GetProject(int id)
        {
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return NotFound();
            }

            return Ok(project);
        }

        // PUT: api/Projects/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProject(int id, Project project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != project.Project_ID)
            {
                return BadRequest();
            }

            db.Entry(project).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
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

        // POST: api/Projects
        [ResponseType(typeof(Project))]
        public IHttpActionResult PostProject(Project project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Projects.Add(project);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = project.Project_ID }, project);
        }

        // DELETE: api/Projects/5
        [ResponseType(typeof(Project))]
        public IHttpActionResult DeleteProject(int id)
        {
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return NotFound();
            }

            db.Projects.Remove(project);
            db.SaveChanges();

            return Ok(project);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProjectExists(int id)
        {
            return db.Projects.Count(e => e.Project_ID == id) > 0;
        }
    }
}