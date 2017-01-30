using NUnit.Framework;
using NSubstitute;
using SoaWebsite.Web.Controllers;
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
    public class DevelopersControllerTests
    {
        [Test]
        public void GivenADeveloper_WhenICallCreate_ThenTheDatabaseIsUpdated()
        {
            var options = new DbContextOptionsBuilder<DeveloperContext>()
                .UseInMemoryDatabase(databaseName: "Add_writes_to_database")
                .Options;

            using (var context = new DeveloperContext(options))
            {
                var controller = new DevelopersController(context);
                var developer = new Developer();
                developer.FirstName = "Toto";
                developer.LastName = "Tata";
                controller.Create(developer);
            }

            using (var context = new DeveloperContext(options))
            {
                Assert.AreEqual(1, context.Developers.Count());
                Assert.AreEqual("Toto", context.Developers.Single().FirstName);
                Assert.AreEqual("Tata", context.Developers.Single().LastName);
            }
        }

        [Test]
        public void GivenASkill_WhenICallAddSkill_ThenItUpdatesTheDatabase()
        {
            var options = new DbContextOptionsBuilder<DeveloperContext>()
                .UseInMemoryDatabase(databaseName: "AddSkill_writes_to_database")
                .Options;

            using (var context = new DeveloperContext(options))
            {
                var controller = new DevelopersController(context);
                var developer = new Developer();

                developer.FirstName = "Toto";
                developer.LastName = "Tata";
                controller.Create(developer);

                var skill = new Skill();
                skill.Name = "Python";
                controller.AddSkill(developer.ID, skill);
            }

            using (var context = new DeveloperContext(options))
            {
                Assert.AreEqual(1, context.Skills.Count());
                var developer = context.Developers.Include(d => d.DeveloperSkills).ThenInclude(x => x.Skill)
                                              .SingleOrDefaultAsync(m => m.FirstName == "Toto").Result;
                Assert.AreEqual("Python", developer.DeveloperSkills.Single().Skill.Name);
            }
        }

        [Test]
        public void GivenAnExistingDeveloper_WhenICallEdit_ItUpdatesTheDatabase()
        {
            var options = new DbContextOptionsBuilder<DeveloperContext>()
                .UseInMemoryDatabase(databaseName: "Edit")
                .Options;

            using (var context = new DeveloperContext(options))
            {
                var controller = new DevelopersController(context);
                var developer = new Developer();

                developer.FirstName = "Toto";
                developer.LastName = "Tata";
                controller.Create(developer);

                var skill = new Skill();
                skill.Name = "Python";
                controller.AddSkill(developer.ID, skill);
            }

            using (var context = new DeveloperContext(options))
            {
                var developer = context.Developers.Include(d => d.DeveloperSkills).ThenInclude(x => x.Skill)
                                              .SingleOrDefaultAsync(m => m.FirstName == "Toto").Result;
                var controller = new DevelopersController(context);
                developer.FirstName = "Bob";
                developer.LastName = "Bobby";
                controller.Edit(developer.ID, developer);
            }

            using (var context = new DeveloperContext(options))
            {
                var developer = context.Developers.Include(d => d.DeveloperSkills).ThenInclude(x => x.Skill)
                                              .SingleOrDefaultAsync(m => m.FirstName == "Bob").Result;
                Assert.AreEqual(1, context.Skills.Count());
                Assert.AreEqual(1, context.Developers.Count());
                Assert.AreEqual("Bobby", developer.LastName);
            }
        }

        [Test]
        public void GivenAnExistingSkill_WhenICallDeleteSkillConfirmed_ItIsRemovedFromTheDatabase()
        {
            var options = new DbContextOptionsBuilder<DeveloperContext>()
                .UseInMemoryDatabase(databaseName: "DeleteSkillConfirmed_updates_database")
                .Options;

            using (var context = new DeveloperContext(options))
            {
                var controller = new DevelopersController(context);
                var developer = new Developer();

                developer.FirstName = "Toto";
                developer.LastName = "Tata";
                controller.Create(developer);

                var python = new Skill();
                python.Name = "Python";
                var java = new Skill();
                java.Name = "Java";
                controller.AddSkill(developer.ID, python);
                controller.AddSkill(developer.ID, java);
            }

            using (var context = new DeveloperContext(options))
            {

                Assert.AreEqual(2, context.Skills.Count());
                var developer = context.Developers.Include(d => d.DeveloperSkills).ThenInclude(x => x.Skill)
                                              .SingleOrDefaultAsync(m => m.FirstName == "Toto").Result;
                var python = developer.DeveloperSkills.Where(m => m.Skill.Name == "Python").Single().Skill;
                Assert.AreEqual("Python", python.Name);
                var java = developer.DeveloperSkills.Where(m => m.Skill.Name == "Java").Single().Skill;
                Assert.AreEqual("Java", java.Name);

                var controller = new DevelopersController(context);
                controller.DeleteSkillConfirmed(developer.ID, python.ID);
                Assert.AreEqual(1, developer.DeveloperSkills.Count());
                Assert.AreEqual(0, python.DeveloperSkills.Count());
            }
        }
    }
}