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
    public class TasksController : ApiController
    {
        private DBModels db = new DBModels();
       
        // GET: api/Tasks
        [HttpGet]
        public IHttpActionResult GetTasks()
        {
            return Ok((from t in db.Tasks
                       join pt in db.ParentTasks on t.Parent_ID equals pt.Parent_ID into mma
                       from pt in mma.DefaultIfEmpty()
                       join p in db.Projects on (t == null ? 0 : t.Project_ID) equals p.Project_ID into pma
                       from p in pma.DefaultIfEmpty()
                       join u in db.Users on (t == null ? 0 : t.User_ID) equals u.User_ID into usr
                       from u in usr.DefaultIfEmpty()
                       where t.Status==1
                       select new TaskDetails
                       {
                           Project_ID = p.Project_ID,
                           Project_Name = p.Project_Name,
                           Start_Date = t.Start_Date,
                           End_Date = t.End_Date,
                           Task_Priority = t.Task_Priority,
                           User_ID = u.User_ID,
                           First_Name = u.First_Name,
                           Last_Name = u.Last_Name,
                           Employee_ID = u.Employee_ID,
                           Task_Name = t.Task_Name,
                           ISTaskEnded = t.ISTaskEnded,
                           Task_ID = t.Task_ID,
                           Parent_Task = pt.Parent_Task,
                           Parent_ID = pt.Parent_ID
                       }).AsEnumerable());
            //    return from p in db.ParentTasks
            //           join t in db.Tasks on p.Parent_ID equals t.Parent_ID into gj
            //           from subpet in gj.DefaultIfEmpty()
            //           join s in db.Projects on t.Project_ID equals s.Project_ID into fj
            //           from subpet1 in fj.DefaultIfEmpty()
            //           join cs in db.Users on s.User_ID equals cs.User_ID

            //               //where s.Date >= DateTime.Parse("02/12/2014) && s.Date <= DateTime.Parse("03 / 12 / 2014")
            //           select new TaskDetails
            //           {
            //               //{
            //               Project_ID = s.Project_ID,
            //               Project_Name = s.Project_Name,
            //               Start_date = t.Start_date,
            //               End_Date = t.End_Date,
            //               Task_Priority = t.Task_Priority,
            //               User_ID = s.User_ID,
            //               First_Name = cs.First_Name,
            //               Last_Name = cs.Last_Name,
            //               Employee_ID = cs.Employee_ID,
            //               Task_Name = t.Task_Name,
            //               Parent_Task = p.Parent_Task,
            //           };
        }
        // GET: api/Tasks/"SD"
   
        [Route("api/Tasks/Get/strSortBy")]
        [HttpGet]
        public IQueryable<Task> GetDetailedTasks(string strSortBy)
        {
            if (strSortBy == "SD")
            {
                return db.Tasks.OrderBy(a => a.Start_Date);
            }
            if (strSortBy == "ED")
            {
                return db.Tasks.OrderBy(a => a.End_Date);
            }
            if (strSortBy == "P")
            {
                return db.Tasks.OrderBy(a => a.Task_Priority);
            }
            else
            {
                return db.Tasks;
            }
        }
        // GET: api/Tasks/5
        [ResponseType(typeof(Task))]
        public IHttpActionResult GetTask(int id)
        {
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }

        // PUT: api/Tasks/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTask(int id, Task task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != task.Task_ID)
            {
                return BadRequest();
            }

            db.Entry(task).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(id))
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

        // POST: api/Tasks
        [ResponseType(typeof(Task))]
        public IHttpActionResult PostTask(Task task)
        {
            ParentTask parentTask = new ParentTask();
            parentTask.Parent_Task = task.Task_Name;
            task.Status = 1;
            task.ISTaskEnded = "N";
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (task.Task_ID == 0 && task.Parent_ID ==0)
            {
                db.ParentTasks.Add(parentTask);
            }
            else
            {
                db.Tasks.Add(task);
            }
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = task.Task_ID }, task);
        }

        // DELETE: api/Tasks/5
        [ResponseType(typeof(Task))]
        public IHttpActionResult DeleteTask(int id)
        {
            Task task = db.Tasks.Find(id);
            task.ISTaskEnded = "Y";
            //if (task == null)
            //{
                //return NotFound();
            //}

            //db.Tasks.Remove(task);
            //db.SaveChanges();

            //return Ok(task);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != task.Task_ID)
            {
                return BadRequest();
            }

            db.Entry(task).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(id))
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

        private bool TaskExists(int id)
        {
            return db.Tasks.Count(e => e.Task_ID == id) > 0;
        }

        public partial class TaskDetails
        {
            public int Project_ID { get; set; }
            public string Project_Name { get; set; }
            public Nullable<System.DateTime> Start_Date { get; set; }
            public Nullable<System.DateTime> End_Date { get; set; }
            public Nullable<int> Task_Priority { get; set; }
            public int User_ID { get; set; }
            public string First_Name { get; set; }
            public string Last_Name { get; set; }
            public string ISTaskEnded { get; set; }
            public Nullable<int> Employee_ID { get; set; }
            public Nullable<int> Parent_ID { get; set; }
            public Nullable<int> Task_ID { get; set; }
            public string Parent_Task { get; set; }
            public string Task_Name { get; set; }
        }
    }
}