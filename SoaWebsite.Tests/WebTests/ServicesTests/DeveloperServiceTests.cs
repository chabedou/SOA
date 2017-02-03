using NUnit.Framework;
using NSubstitute;
using SoaWebsite.Web.Controllers;
using SoaWebsite.Common.Models;
using SoaWebsite.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SoaWebsite.Tests
{
    [TestFixture]
    public class DeveloperServiceTests
    {
        public DbContextOptions<DeveloperContext> GetOptions(string databaseName)
        {
            var options = new DbContextOptionsBuilder<DeveloperContext>()
                .UseInMemoryDatabase(databaseName: databaseName)
                .Options;
            return options;
        }

        [Test]
        public void GivenADeveloper_WhenICallAddDeveloper_ThenItUpdatesTheDatabase()
        {
            var options = GetOptions("AddDeveloper");

            using (var context = new DeveloperContext(options))
            {
                var service=new DeveloperService(context);
                var controller = new DevelopersController(service);
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
                Assert.AreEqual(2, context.Developers.Count());
            }
        }

        [Test]
        public void GivenADeveloperID_WhenICallDeveloperById_ThenItReturnsTheDeveloper()
        {
            var options = GetOptions("DeveloperById");

            using (var context = new DeveloperContext(options))
            {
                var service=new DeveloperService(context);
                var controller = new DevelopersController(service);
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
        public void GivenANonExistantDeveloperID_WhenICallDeveloperById_ThenItReturnsNull()
        {
            var options = GetOptions("DeveloperById");

            using (var context = new DeveloperContext(options))
            {
                var service=new DeveloperService(context);
                var controller = new DevelopersController(service);
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
                var service = new DeveloperService(context);
                var actual = service.DeveloperById(65);
                Assert.AreEqual(null, actual);
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
                var service=new DeveloperService(context);
                var controller = new DevelopersController(service);
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

        [Test]
        public void GivenASkillName_WhenICallSkillWithDevelopersByName_ThenItReturnsTheSkill()
        {
            var options = new DbContextOptionsBuilder<DeveloperContext>()
                .UseInMemoryDatabase(databaseName: "SkillWithDevelopersByName")
                .Options;

            var python = new Skill();
            python.Name = "Python";

            using (var context = new DeveloperContext(options))
            {
                var service=new DeveloperService(context);
                var controller = new DevelopersController(service);
                var developer = new Developer();
                developer.FirstName = "Toto";
                developer.LastName = "Tata";
                controller.Create(developer);

                
                controller.AddSkill(developer.ID, python);
                var skill = new Skill();
                skill.Name = "Java";
                controller.AddSkill(developer.ID, skill);
            }

            using (var context = new DeveloperContext(options))
            {
                var service = new DeveloperService(context);
                var skill = service.SkillWithDevelopersByName("Python");
                Assert.AreEqual(python.Name, skill.Name);
                Assert.AreEqual("Toto", skill.DeveloperSkills.Single().Developer.FirstName);
            }
        }

        [Test]
        public void GivenANonExistentSkillName_WhenICallSkillWithDevelopersByName_ThenItReturnsNull()
        {
            var options = new DbContextOptionsBuilder<DeveloperContext>()
                .UseInMemoryDatabase(databaseName: "SkillWithDevelopersByNameNull")
                .Options;

            var python = new Skill();
            python.Name = "Python";

            using (var context = new DeveloperContext(options))
            {
                var service=new DeveloperService(context);
                var controller = new DevelopersController(service);
                var developer = new Developer();
                developer.FirstName = "Toto";
                developer.LastName = "Tata";
                controller.Create(developer);

                
                controller.AddSkill(developer.ID, python);
                var skill = new Skill();
                skill.Name = "Java";
                controller.AddSkill(developer.ID, skill);
            }

            using (var context = new DeveloperContext(options))
            {
                var service = new DeveloperService(context);
                var skill = service.SkillWithDevelopersByName("C#");
                Assert.AreEqual(null, skill);
            }
        }

        [Test]
        public void GivenADeveloperIdAndASkillId_WhenICallGetDeveloperSkill_ThenIGetTheDeveloperSkill()
        {
            var options = new DbContextOptionsBuilder<DeveloperContext>()
                .UseInMemoryDatabase(databaseName: "DeveloperSkill")
                .Options;

            var python = new Skill();
            python.Name = "Python";

            using (var context = new DeveloperContext(options))
            {
                var service=new DeveloperService(context);
                var controller = new DevelopersController(service);
                var developer = new Developer();
                developer.FirstName = "Toto";
                developer.LastName = "Tata";
                controller.Create(developer);
                
                controller.AddSkill(developer.ID, python);
                var skill = new Skill();
                skill.Name = "Java";
                controller.AddSkill(developer.ID, skill);
            }

            using (var context = new DeveloperContext(options))
            {
                var service = new DeveloperService(context);
                var developerskill = service.GetDeveloperSkill(1, 1);
                Assert.AreEqual(1, context.Developers.Count());
            }
        }
    }
}