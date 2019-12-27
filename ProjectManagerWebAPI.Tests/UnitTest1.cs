using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Results;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using ProjectManagerWebAPI.Controllers;
using ProjectManagerWebAPI.Models;

namespace ProjectManagerWebAPI.Tests
{
    [TestFixture]
    public class UnitTest1
    {
        [TestCase]
        public void GetUser()
        {
            UsersController us = new UsersController();

            var getResult = us.GetUsers();
            IHttpActionResult actionResult = us.GetUser(10);
            var notFoundRes = actionResult as NotFoundResult;
            Assert.IsNotNull(notFoundRes);
            //Assert.Equals("OK",actionResult);


            //var idResult = us.GetUser(1);
            //var oddRes = getResult as OkNegotiatedContentResult<object>; // oddRes is null
            //Assert.AreEqual(HttpStatusCode.Success, oddRes.StatusCode); // throws for null ref
            //Assert.AreEqual(us._data, oddRes.Content); // thro
        }
    }
}
