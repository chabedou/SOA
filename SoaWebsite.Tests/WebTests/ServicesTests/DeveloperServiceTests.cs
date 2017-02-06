using NUnit.Framework;
using SoaWebsite.Common.Models;
using SoaWebsite.Services.Services;
using System.Linq;
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
                var service = new DeveloperService(context);
                var developer = new Developer();
                developer.ID = 1;
                developer.FirstName = "Toto";
                developer.LastName = "Tata";
                service.AddDeveloper(developer);
                var secondDeveloper = new Developer();
                secondDeveloper.ID = 2;
                secondDeveloper.FirstName = "Bob";
                secondDeveloper.LastName = "Bobby";
                service.AddDeveloper(secondDeveloper);

                var skill = new Skill();
                skill.ID = 1;
                skill.Name = "Java";
                service.AddSkill(developer.ID, skill);
                skill = new Skill();
                skill.ID = 2;
                skill.Name = "C#";
                service.AddSkill(secondDeveloper.ID, skill);
            }
        }

        [Test]
        public void GivenADeveloper_WhenICallAddDeveloper_ThenItUpdatesTheDatabase()
        {
            var options = GetOptions("AddDeveloper");

            using (var context = new DeveloperContext(options))
            {
                var service = new DeveloperService(context);
                var developer = new Developer();
                developer.FirstName = "Toto";
                developer.LastName = "Tata";
                service.AddDeveloper(developer);
                developer = new Developer();
                developer.FirstName = "Bob";
                developer.LastName = "Bobby";
                service.AddDeveloper(developer);
            }

            using (var context = new DeveloperContext(options))
            {
                Assert.AreEqual(2, context.Developers.Count());
            }
        }

        [Test]
        public void GivenAValidDeveloperIdAndASkill_WhenICallAddSkill_ThenItUpdatesTheDatabaseAndReturnsTrue()
        {
            var options = GetOptions("AddSkill");
            using (var context = new DeveloperContext(options))
            {
                var service = new DeveloperService(context);
                var developer = new Developer();
                developer.FirstName = "Toto";
                developer.LastName = "Tata";
                service.AddDeveloper(developer);
                var skill = new Skill();
                skill.Name = "Python";
                Assert.AreEqual(true, service.AddSkill(1, skill));
                Assert.AreEqual("Python", context.Developers.Single().DeveloperSkills.Single().Skill.Name);
                Assert.AreEqual(1, context.Skills.Count());
            }
        }

        [Test]
        public void GivenANonValidDeveloperIdAndASkill_WhenICallAddSkill_ThenItReturnsFalse()
        {
            var options = GetOptions("AddSkillFalse");
            using (var context = new DeveloperContext(options))
            {
                var service = new DeveloperService(context);
                var developer = new Developer();
                developer.FirstName = "Toto";
                developer.LastName = "Tata";
                service.AddDeveloper(developer);
                var skill = new Skill();
                skill.Name = "Python";
                Assert.AreEqual(false, service.AddSkill(42, skill));
                developer = service.DeveloperWithSkillsById(1);
                Assert.AreEqual(0, developer.DeveloperSkills.Count());
                Assert.AreEqual(0, context.Skills.Count());
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
                var actual = service.DeveloperById(42);
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
                var actual = service.DeveloperWithSkillsById(42);
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
            var options = GetOptions("GivenADeveloperIdAndASkillId_WhenICallGetDeveloperSkill_ThenIGetTheDeveloperSkill");
            InitializeDatabaseWithDevelopersAndSkills(options);
            using (var context = new DeveloperContext(options))
            {
                var service = new DeveloperService(context);
                var developerskill = service.GetDeveloperSkill(1, 1);
                Assert.IsNotNull(developerskill);
                Assert.IsNotNull(developerskill.Developer);
                Assert.IsNotNull(developerskill.Skill);
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
                var developerskill = service.GetDeveloperSkill(42, 1);
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
                var developerskill = service.GetDeveloperSkill(1, 42);
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

        [Test]
        public void GivenAValidDeveloperIdAndAValidSkillId_WhenICallTryRemoveSkill_ThenItUpdatesTheDatabaseAndReturnsTrue()
        {
            var options = GetOptions("TryRemoveSkill");
            InitializeDatabaseWithDevelopersAndSkills(options);
            using (var context = new DeveloperContext(options))
            {
                var service = new DeveloperService(context);
                var developer = service.DeveloperWithSkillsById(1);
                var skill = service.SkillWithDevelopersByName("Java");
                Assert.AreEqual(1, developer.DeveloperSkills.Count());
                Assert.AreEqual(1, skill.DeveloperSkills.Count());
                Assert.AreEqual(true, service.TryRemoveSkill(developer.ID, skill.ID));
                Assert.AreEqual(0, developer.DeveloperSkills.Count());
                Assert.AreEqual(0, skill.DeveloperSkills.Count());
            }
        }

        [Test]
        public void GivenAInvalidDeveloperIdAndAValidSkillId_WhenICallTryRemoveSkill_ThenItReturnsFalse()
        {
            var options = GetOptions("TryRemoveSkillInvalidDev");
            InitializeDatabaseWithDevelopersAndSkills(options);
            using (var context = new DeveloperContext(options))
            {
                var service = new DeveloperService(context);
                var developer = service.DeveloperWithSkillsById(1);
                var skill = service.SkillWithDevelopersByName("Java");
                Assert.AreEqual(false, service.TryRemoveSkill(42, skill.ID));
                Assert.AreEqual(1, developer.DeveloperSkills.Count());
                Assert.AreEqual(1, skill.DeveloperSkills.Count());
            }
        }

        [Test]
        public void GivenAValidDeveloperIdAndAInvalidSkillId_WhenICallTryRemoveSkill_ThenItReturnsFalse()
        {
            var options = GetOptions("TryRemoveSkillInvalidSkill");
            InitializeDatabaseWithDevelopersAndSkills(options);
            using (var context = new DeveloperContext(options))
            {
                var service = new DeveloperService(context);
                var developer = service.DeveloperWithSkillsById(1);
                var skill = service.SkillWithDevelopersByName("Java");
                Assert.AreEqual(false, service.TryRemoveSkill(developer.ID, 42));
                Assert.AreEqual(1, developer.DeveloperSkills.Count());
                Assert.AreEqual(1, skill.DeveloperSkills.Count());
            }
        }
    }
}