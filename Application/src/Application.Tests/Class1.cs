using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Controllers;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace Application.Tests
{
    [TestFixture]
    public class Class1
    {
        [Test]
        public void SimpleTest()
        {
            var homeController = new HomeController();
            var view = homeController.Index();
            Assert.IsInstanceOf<IActionResult>(view);
        }
    }
}
