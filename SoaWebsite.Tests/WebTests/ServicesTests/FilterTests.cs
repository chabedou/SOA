using NUnit.Framework;
using SoaWebsite.Common.Models;
using SoaWebsite.Services.Services;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using System.Collections.Generic;

namespace SoaWebsite.Tests.WebTests.ServicesTests
{
    [TestFixture]
    public class FilterTests
    {
        [Test]
        public void GivenASkillName_WhenICallCondition_ThenItReturnsAFunctionWhichChecksIfADeveloperHasThisSkill()
        {
            Developer developer = new Developer();
            developer.FirstName = "Toto";
            developer.LastName = "Tata";
            var list = new List<DeveloperSkill>();
            var skill = new Skill
            {
                Name = "Python"
            };
            var developerSkill = new DeveloperSkill
            {
                Skill = skill,
                Developer = developer
            };
            list.Add(developerSkill);
            developer.DeveloperSkills = list;

            var skills = new string[1];
            skills[0] = "Python";
            var filter = new Filter(skills);

            Assert.AreEqual(true, filter.Condition()(developer));
        }
    }
}