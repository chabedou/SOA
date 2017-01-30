using NUnit.Framework;
using NSubstitute;
using SoaWebsite.Web.Controllers;
using SoaWebsite.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SoaWebsite.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace SoaWebsite.Tests
{
    [TestFixture]
    public class DeveloperServiceTests
    {
        [Test]
        public void GivenADeveloperID_WhenICallDeveloperById_ThenItReturnsTheDeveloper()
        {
            var options = new DbContextOptionsBuilder<DeveloperContext>()
                .UseInMemoryDatabase(databaseName: "develeperByID")
                .Options;

            using (var context = new DeveloperContext(options))
            {
                var controller = new DevelopersController(context);
                var developer = new Developer();
                developer.FirstName = "Toto";
                developer.LastName = "Tata";
                controller.Create(developer);
                developer = new Developer();
                developer.FirstName = "Bob";
                developer.LastName = "Bobby";
                controller.Create(developer);
            }

            using (var context = new DeveloperContext(options))
            {
                var id = context.Developers.Single(m => m.FirstName == "Toto").ID;
                var service = new DeveloperService(context);
                var actual = service.DeveloperById(id);
                Assert.AreEqual("Toto", actual.FirstName);
                id = context.Developers.Single(m => m.FirstName == "Bob").ID;
                actual = service.DeveloperById(id);
                Assert.AreEqual("Bobby", actual.LastName);
            }
        }

        [Test]
        public void GivenADeveloperID_WhenICallDeveloperWithSkillsById_ThenItReturnsTheDeveloperWithHisSkills()
        {
            var options = new DbContextOptionsBuilder<DeveloperContext>()
                .UseInMemoryDatabase(databaseName: "develeperWithSkillsByID")
                .Options;

            using (var context = new DeveloperContext(options))
            {
                var controller = new DevelopersController(context);
                var developer = new Developer();
                developer.FirstName = "Toto";
                developer.LastName = "Tata";
                controller.Create(developer);
                developer = new Developer();
                developer.FirstName = "Bob";
                developer.LastName = "Bobby";
                controller.Create(developer);
                var skill = new Skill();
                skill.Name = "Python";
                controller.AddSkill(developer.ID, skill);
            }

            using (var context = new DeveloperContext(options))
            {
                var id = context.Developers.Single(m => m.FirstName == "Toto").ID;
                var service = new DeveloperService(context);
                var actual = service.DeveloperWithSkillsById(id);
                Assert.AreEqual("Toto", actual.FirstName);
                id = context.Developers.Single(m => m.FirstName == "Bob").ID;
                actual = service.DeveloperWithSkillsById(id);
                Assert.AreEqual("Bobby", actual.LastName);
                Assert.AreEqual("Python", actual.DeveloperSkills.Single().Skill.Name);
            }
        }
    }
}