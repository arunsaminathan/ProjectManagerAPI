using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using NUnit.Framework;
using ProjectManagerWebAPI.Controllers;
using ProjectManagerWebAPI.Models;
using static ProjectManagerWebAPI.Controllers.ProjectsController;
using static ProjectManagerWebAPI.Controllers.TasksController;

namespace ProjectManagerWebAPI
{
    [TestFixture]
    public class ProjectManagerTest
    {
        [TestCase]
        public void GetUserID()
        {
            var controller = new UsersController();
            var actionResult = controller.GetUser(1);
            var response = actionResult as OkNegotiatedContentResult<Models.User>;
            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Content.User_ID);
        }
        [TestCase]
        public void GetUser()
        {
            var controller = new UsersController();
            var actionResult = controller.GetUsers();
            var response = actionResult as OkNegotiatedContentResult<IEnumerable<Users>>;
            Assert.IsNotNull(response);
            var users = response.Content;
            Assert.AreEqual(7, users.Count());
        }
        [TestCase]
        public void PutShouldUpdateUser()
        {
            var controller = new UsersController();
            var user = new Models.User { User_ID = 1, First_Name = "Saminathan", Last_Name = "ARUN" ,Employee_ID=7,Status=1};
            var actionResult = controller.PutUser(user.User_ID, user);
            var response = actionResult as OkNegotiatedContentResult<User>;
            var actionResult1 = controller.GetUser(1);
            var response1 = actionResult1 as OkNegotiatedContentResult<Models.User>;
            var newuser = response1.Content;
            Assert.AreEqual(1, newuser.User_ID);
            Assert.AreEqual("Saminathan", newuser.First_Name);
            Assert.AreEqual("ARUN", newuser.Last_Name);
        }

        [TestCase]
        public void PostShouldAddUser()
        {
            var controller = new UsersController();
            var actionResult = controller.PostUser(new User
            {
                First_Name = "Aravinth",
                Last_Name = "Kumar",
                Employee_ID = 999,
                Status=1
            });
            var response = actionResult as CreatedAtRouteNegotiatedContentResult<User>;
            Assert.IsNotNull(response);
            Assert.AreEqual("DefaultApi", response.RouteName);
            Assert.AreEqual(response.Content.User_ID, response.RouteValues["id"]);
        }

        [TestCase]
        public void GetTaskID()
        {
            var controller = new TasksController();
            var actionResult = controller.GetTask(1);
            var response = actionResult as OkNegotiatedContentResult<Models.Task>;
            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Content.Task_ID);
        }
        [TestCase]
        public void GetTask()
        {
            var controller = new TasksController();
            var actionResult = controller.GetTasks();
            var response = actionResult as OkNegotiatedContentResult<IEnumerable<TaskDetails>>;
            Assert.IsNotNull(response);
            var users = response.Content;
            Assert.AreEqual(5, users.Count());
        }
        [TestCase]
        public void PutShouldUpdateTask()
        {
            var controller = new TasksController();
            DateTime datetime = new DateTime();
            var task = new Models.Task { Task_Name = "Test task", Start_Date =DateTime.Now, End_Date = datetime.AddDays(2),Project_ID=1, Parent_ID =1, ISTaskEnded ="N", Task_Priority =0, Status = 1 };
            var actionResult = controller.PutTask(task.Task_ID, task);
            var response = actionResult as OkNegotiatedContentResult<Task>;
            var actionResult1 = controller.GetTask(1);
            var response1 = actionResult1 as OkNegotiatedContentResult<Models.Task>;
            var newtask = response1.Content;
            Assert.AreEqual(3, newtask.User_ID);
            Assert.AreEqual("Test", newtask.Task_Name);
            Assert.AreEqual(10, newtask.Task_Priority);
        }

        [TestCase]
        public void PostShouldAddTask()
        {
            var controller = new TasksController();
            DateTime datetime = new DateTime();
            var actionResult = controller.PostTask(new Task
            {
                Task_Name = "Test task post",
                Start_Date = DateTime.Now,
                End_Date = datetime.AddDays(2),
                ISTaskEnded = "N",
                Task_Priority = 7,
                Project_ID = 1,
                Parent_ID = 1,
                User_ID=1,
                Status = 1
            });
            var response = actionResult as CreatedAtRouteNegotiatedContentResult<Task>;
            Assert.IsNotNull(response);
            Assert.AreEqual("DefaultApi", response.RouteName);
            Assert.AreEqual(response.Content.Task_ID, response.RouteValues["id"]);
        }

        public void GetProjectID()
        {
            var controller = new ProjectsController();
            var actionResult = controller.GetProject(1);
            var response = actionResult as OkNegotiatedContentResult<Models.Project>;
            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Content.User_ID);
        }
        [TestCase]
        public void GetProjet()
        {
            var controller = new ProjectsController();
            var actionResult = controller.GetProjects();
            var response = actionResult as OkNegotiatedContentResult<IEnumerable<Projects>>;
            Assert.IsNotNull(response);
            var users = response.Content;
            Assert.AreEqual(2, users.Count());
        }
        [TestCase]
        public void PutShouldUpdateProject()
        {
            var controller = new ProjectsController();
            DateTime datetime = new DateTime();
            var task = new Models.Project { Project_Name = "Project task test", Start_Date = DateTime.Now, End_Date = datetime.AddDays(2),User_ID=1, Project_ID = 1, Status = 1 };
            var actionResult = controller.PutProject(task.Project_ID, task);
            var response = actionResult as OkNegotiatedContentResult<Project>;
            var actionResult1 = controller.GetProject(1);
            var response1 = actionResult1 as OkNegotiatedContentResult<Models.Project>;
            var newProject = response1.Content;
            Assert.AreEqual(1, newProject.Project_ID);
            Assert.AreEqual("Project task test", newProject.Project_Name);
            Assert.AreEqual(1, newProject.User_ID);
        }

        [TestCase]
        public void PostShouldAddProject()
        {
            var controller = new ProjectsController();
            DateTime datetime = new DateTime();
            var actionResult = controller.PostProject(new Project
            {
                Project_Name = "Project task test add",
                Start_Date = DateTime.Now,
                End_Date = datetime.AddDays(2),
                User_ID = 1,
                Project_ID = 1,
                Status = 1
            });
            
            var response = actionResult as CreatedAtRouteNegotiatedContentResult<Project>;
            Assert.IsNotNull(response);
            Assert.AreEqual("DefaultApi", response.RouteName);
            Assert.AreEqual(response.Content.Project_ID, response.RouteValues["id"]);
        }
    }

}