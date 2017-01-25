using NUnit.Framework;
using NSubstitute;
using SoaWebsite.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SoaWebsite.Web.Models;

namespace SoaWebsite.Web.Tests
{
    [TestFixture]
    internal class ControllerTester
    {
        [Test]
        public void DefaultValue()
        {

            Assert.AreEqual(1, 1);
        }
    }
}