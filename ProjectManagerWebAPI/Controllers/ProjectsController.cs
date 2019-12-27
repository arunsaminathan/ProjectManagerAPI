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

        // GET: api/Projects
      
        public IHttpActionResult GetProjects()
        {
            return Ok((from s in db.Projects
                       join t in db.Tasks on s.Project_ID equals t.Project_ID into pma
                       from t in pma.DefaultIfEmpty()
                       join u in db.Users on (s == null ? 0 : s.User_ID) equals u.User_ID into usr
                       from u in usr.DefaultIfEmpty()
                       where s.Status == 1
                       select new Projects
                       {
                           //{
                           Project_ID = s.Project_ID,
                           Project_Name = s.Project_Name,
                           Start_Date = s.Start_Date,
                           End_Date = s.End_Date,
                           Priority = s.Priority,
                           User_ID = s.User_ID,
                           First_Name = u.First_Name
                       }).AsEnumerable());

            //return db.Projects;
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
            project.Status = 1;
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
            project.Status = 0;
            //if (project == null)
            //{
            //    return NotFound();
            //}

            //db.Projects.Remove(project);
            //db.SaveChanges();

            //return Ok(project);
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

    }
}