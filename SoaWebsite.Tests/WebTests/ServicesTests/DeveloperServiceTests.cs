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
        public async Task GivenADeveloperID_WhenICallDeveloperById_ThenItReturnsTheDeveloper()
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
                var actual = await service.DeveloperById(id);
                Assert.AreEqual("Toto", actual.FirstName);
                id = context.Developers.Single(m => m.FirstName == "Bob").ID;
                actual = await service.DeveloperById(id);
                Assert.AreEqual("Bobby", actual.LastName);
            }
        }

        [Test]
        public async Task GivenADeveloperID_WhenICallDeveloperWithSkillsById_ThenItReturnsTheDeveloperWithHisSkills()
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
                await controller.AddSkill(developer.ID, skill);
            }

            using (var context = new DeveloperContext(options))
            {
                var id = context.Developers.Single(m => m.FirstName == "Toto").ID;
                var service = new DeveloperService(context);
                var actual = await service.DeveloperWithSkillsById(id);
                Assert.AreEqual("Toto", actual.FirstName);
                id = context.Developers.Single(m => m.FirstName == "Bob").ID;
                actual = await service.DeveloperWithSkillsById(id);
                Assert.AreEqual("Bobby", actual.LastName);
                Assert.AreEqual("Python", actual.DeveloperSkills.Single().Skill.Name);
            }
        }

        [Test]
        public async Task GivenASkillName_WhenICallSkillWithDevelopersByName_ThenItReturnsTheSkill()
        {
            var options = new DbContextOptionsBuilder<DeveloperContext>()
                .UseInMemoryDatabase(databaseName: "SkillWithDevelopersByName")
                .Options;

            var python = new Skill();
            python.Name = "Python";

            using (var context = new DeveloperContext(options))
            {
                var controller = new DevelopersController(context);
                var developer = new Developer();
                developer.FirstName = "Toto";
                developer.LastName = "Tata";
                controller.Create(developer);

                
                await controller.AddSkill(developer.ID, python);
                var skill = new Skill();
                skill.Name = "Java";
                await controller.AddSkill(developer.ID, skill);
            }

            using (var context = new DeveloperContext(options))
            {
                var service = new DeveloperService(context);
                var skill = await service.SkillWithDevelopersByName("Python");
                Assert.AreEqual(python.Name, skill.Name);
                Assert.AreEqual("Toto", skill.DeveloperSkills.Single().Developer.FirstName);
            }
        }

        [Test]
        public async Task GivenANonExistentSkillName_WhenICallSkillWithDevelopersByName_ThenItReturnsNull()
        {
            var options = new DbContextOptionsBuilder<DeveloperContext>()
                .UseInMemoryDatabase(databaseName: "SkillWithDevelopersByNameNull")
                .Options;

            var python = new Skill();
            python.Name = "Python";

            using (var context = new DeveloperContext(options))
            {
                var controller = new DevelopersController(context);
                var developer = new Developer();
                developer.FirstName = "Toto";
                developer.LastName = "Tata";
                controller.Create(developer);

                
                await controller.AddSkill(developer.ID, python);
                var skill = new Skill();
                skill.Name = "Java";
                await controller.AddSkill(developer.ID, skill);
            }

            using (var context = new DeveloperContext(options))
            {
                var service = new DeveloperService(context);
                var skill = await service.SkillWithDevelopersByName("C#");
                Assert.AreEqual(null, skill);
            }
        }

        [Test]
        public void GivenADeveloper_WhenICallAddDeveloper_ThenItUpdatesTheDatabase()
        {
            var options = new DbContextOptionsBuilder<DeveloperContext>()
                .UseInMemoryDatabase(databaseName: "AddDeveloper")
                .Options;

            using (var context = new DeveloperContext(options))
            {
                var service = new DeveloperService(context);
                var developer = new Developer();
                developer.FirstName = "Toto";
                developer.LastName = "Tata";
                service.AddDeveloper(developer);
            }

            using (var context = new DeveloperContext(options))
            {
                Assert.AreEqual(1, context.Developers.Count());
            }
        }

        [Test]
        public async Task GivenADeveloperIdAndASkillId_WhenICallGetDeveloperSkill_ThenIGetTheDeveloperSkill()
        {
            var options = new DbContextOptionsBuilder<DeveloperContext>()
                .UseInMemoryDatabase(databaseName: "DeveloperSkill")
                .Options;

            var python = new Skill();
            python.Name = "Python";

            using (var context = new DeveloperContext(options))
            {
                var controller = new DevelopersController(context);
                var developer = new Developer();
                developer.FirstName = "Toto";
                developer.LastName = "Tata";
                controller.Create(developer);
                
                await controller.AddSkill(developer.ID, python);
                var skill = new Skill();
                skill.Name = "Java";
                await controller.AddSkill(developer.ID, skill);
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