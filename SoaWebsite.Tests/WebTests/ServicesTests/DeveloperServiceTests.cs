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

        private void InitializeDatabaseWithDevelopersAndSkills(DbContextOptions<DeveloperContext> options)
        {
            using (var context = new DeveloperContext(options))
            {
                var service=new DeveloperService(context);
                var controller = new DevelopersController(service);
                var developer = new Developer();
                developer.FirstName = "Toto";
                developer.LastName = "Tata";
                controller.Create(developer);
                var secondDeveloper = new Developer();
                secondDeveloper.FirstName = "Bob";
                secondDeveloper.LastName = "Bobby";
                controller.Create(secondDeveloper);
                
                var skill = new Skill();
                skill.Name = "Java";
                controller.AddSkill(developer.ID, skill);
                skill = new Skill();
                skill.Name = "C#";
                controller.AddSkill(secondDeveloper.ID, skill);
            }
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
            InitializeDatabaseWithDevelopersAndSkills(options);
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
        public void GivenANonValidDeveloperID_WhenICallDeveloperById_ThenItReturnsNull()
        {
            var options = GetOptions("DeveloperByIdNull");
            InitializeDatabaseWithDevelopersAndSkills(options);
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
            var options = GetOptions("develeperWithSkillsByID");
            InitializeDatabaseWithDevelopersAndSkills(options);
            using (var context = new DeveloperContext(options))
            {
                var id = context.Developers.Single(m => m.FirstName == "Toto").ID;
                var service = new DeveloperService(context);
                var actual = service.DeveloperWithSkillsById(id);
                Assert.AreEqual("Toto", actual.FirstName);
                Assert.AreEqual("Java", actual.DeveloperSkills.Single().Skill.Name);
            }
        }

        [Test]
        public void GivenANonValidDeveloperID_WhenICallDeveloperWithSkillsById_ThenItReturnsNull()
        {
            var options = GetOptions("develeperWithSkillsByIDNull");
            InitializeDatabaseWithDevelopersAndSkills(options);
            using (var context = new DeveloperContext(options))
            {
                var service = new DeveloperService(context);
                var actual = service.DeveloperWithSkillsById(65);
                Assert.AreEqual(null, actual);
            }
        }

        [Test]
        public void GivenASkillName_WhenICallSkillWithDevelopersByName_ThenItReturnsTheSkill()
        {
            var options = GetOptions("SkillWithDevelopersByName");
            InitializeDatabaseWithDevelopersAndSkills(options);

            using (var context = new DeveloperContext(options))
            {
                var service = new DeveloperService(context);
                var skill = service.SkillWithDevelopersByName("C#");
                Assert.AreEqual("C#", skill.Name);
                Assert.AreEqual("Bob", skill.DeveloperSkills.Single().Developer.FirstName);
            }
        }

        [Test]
        public void GivenANonValidSkillName_WhenICallSkillWithDevelopersByName_ThenItReturnsNull()
        {
            var options = GetOptions("SkillWithDevelopersByNameNull");
            InitializeDatabaseWithDevelopersAndSkills(options);
            using (var context = new DeveloperContext(options))
            {
                var service = new DeveloperService(context);
                var skill = service.SkillWithDevelopersByName("Python");
                Assert.AreEqual(null, skill);
            }
        }

        [Test]
        public void GivenADeveloperIdAndASkillId_WhenICallGetDeveloperSkill_ThenIGetTheDeveloperSkill()
        {
            var options = GetOptions("DeveloperSkill");
            InitializeDatabaseWithDevelopersAndSkills(options);
            using (var context = new DeveloperContext(options))
            {
                var service = new DeveloperService(context);
                var developerskill = service.GetDeveloperSkill(1, 1);
                Assert.AreEqual("Toto", developerskill.Developer.FirstName);
                Assert.AreEqual("Java", developerskill.Skill.Name);
            }
        }

        [Test]
        public void GivenANonValidDeveloperIdAndASkillId_WhenICallGetDeveloperSkill_ThenItReturnsNull()
        {
            var options = GetOptions("DeveloperSkillInvalidDev");
            InitializeDatabaseWithDevelopersAndSkills(options);
            using (var context = new DeveloperContext(options))
            {
                var service = new DeveloperService(context);
                var developerskill = service.GetDeveloperSkill(3, 1);
                Assert.AreEqual(null, developerskill);
            }
        }

        [Test]
        public void GivenADeveloperIdAndANonValidSkillId_WhenICallGetDeveloperSkill_ThenItReturnsNull()
        {
            var options = GetOptions("DeveloperSkillInvalidSkill");
            InitializeDatabaseWithDevelopersAndSkills(options);

            using (var context = new DeveloperContext(options))
            {
                var service = new DeveloperService(context);
                var developerskill = service.GetDeveloperSkill(1, 3);
                Assert.AreEqual(null, developerskill);
            }
        }

        [Test]
        public void GivenAValidDeveloper_WhenICallRemoveDeveloper_ThenItUpdatesTheDatabase()
        {
            var options = GetOptions("RemoveDeveloper");
            InitializeDatabaseWithDevelopersAndSkills(options);

            using (var context = new DeveloperContext(options))
            {
                var service = new DeveloperService(context);
                var developer = service.DeveloperWithSkillsById(1);
                service.RemoveDeveloper(developer);
                Assert.AreEqual(1, context.Developers.Count());
                Assert.AreEqual("Bob", context.Developers.Single().FirstName);
            }
        }
    }
}