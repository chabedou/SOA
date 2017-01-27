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
        public void CreateAddsADeveloperInTheDatabase()
        {
            var options = new DbContextOptionsBuilder<DeveloperContext>()
                .UseInMemoryDatabase(databaseName: "Add_writes_to_database")
                .Options;

            // Run the test against one instance of the context
            using (var context = new DeveloperContext(options))
            {
                var controller = new DevelopersController(context);
                var developer = new Developer();
                developer.FirstName = "Toto";
                developer.LastName = "Tata";
                controller.Create(developer);
            }

            // Use a separate instance of the context to verify correct data was saved to database
            using (var context = new DeveloperContext(options))
            {
                Assert.AreEqual(1, context.Developers.Count());
                Assert.AreEqual("Toto", context.Developers.Single().FirstName);
                Assert.AreEqual("Tata", context.Developers.Single().LastName);
                //Assert.AreEqual(1, context.Developers.Single().ID);
            }
        }

        [Test]
        public async Task AddSkillPythonToADeveloperWorksFine()
        {
            var options = new DbContextOptionsBuilder<DeveloperContext>()
                .UseInMemoryDatabase(databaseName: "AddSkill_writes_to_database")
                .Options;

            // Run the test against one instance of the context
            using (var context = new DeveloperContext(options))
            {
                var controller = new DevelopersController(context);
                var developer = new Developer();
                
                developer.FirstName = "Toto";
                developer.LastName = "Tata";
                controller.Create(developer);

                var skill = new Skill();
                skill.Name = "Python";
                await controller.AddSkill((int?) developer.ID, skill);
            }

            // Use a separate instance of the context to verify correct data was saved to database
            using (var context = new DeveloperContext(options))
            {
                Assert.AreEqual(1, context.Skills.Count());
                var developer = await context.Developers.Include(d => d.DeveloperSkills).ThenInclude(x => x.Skill)
                                              .SingleOrDefaultAsync(m => m.FirstName == "Toto");
                Assert.AreEqual("Python", developer.DeveloperSkills.Single().Skill.Name);
            }
        }
    }
}