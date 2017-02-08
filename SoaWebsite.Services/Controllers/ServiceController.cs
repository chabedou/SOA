using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoaWebsite.Common.Models;
using SoaWebsite.Services.Services;


namespace SoaWebsite.Services.Controllers
{
    [Route("api")]
    public class ServiceController : Controller
    {
        private readonly DeveloperService service;
        private readonly DeveloperContext _context;
        public ServiceController(DeveloperContext context)
        {
            service = new DeveloperService(context);
            _context = context;
        }
        [HttpGet]
        public IEnumerable<Developer> GetAllDevelopers()
        {
            return service.GetAllDevelopers();
        }
        
        [HttpGet("developer/id/{id}")]
        public Developer DeveloperById(int id)
        {
            var developer = _context.Developers
                                    .Include(d => d.DeveloperSkills)
                                    .ThenInclude(x => x.Skill)
                                    .SingleOrDefault(m => m.ID == id);
            return developer;
        }

        [HttpPost("developer/delete")]
        public IActionResult DeleteDeveloperById([FromBody]int id)
        {
            var developer = DeveloperById(id);
            if (developer == null)
            {
                return BadRequest();
            }
            _context.Developers.Remove(developer);
            _context.SaveChanges();

            return Ok();
        }

        [HttpPost("developer/create")]
        public IActionResult AddDeveloper([FromBody]Developer developer)
        {
            developer.DeveloperSkills = new List<DeveloperSkill>();
            _context.Developers.Add(developer);
            _context.SaveChanges();

            return Ok();
        }

        [HttpGet("skill")]
        public IEnumerable<Skill> GetAllSkills()
        {
            return service.GetAllSkills();
        }

        [HttpGet("skill/id/{id}")]
        public Skill SkillById(int id)
        {
            Skill skill = service.SkillById(id);
            return skill;
        }

        [HttpGet("skill/name/{skillname}")]
        public Skill GetSkillByName(string skillName)
        {
            Skill skill = _context.Skills
                            .SingleOrDefault(m => m.Name == skillName);
            return skill;
        }

        [HttpGet("skill/developer/{id}")]
        public IEnumerable<Skill> GetSkillsByDeveloperId(int id)
        {
            Developer dev = DeveloperById(id);
            var devskill = dev.DeveloperSkills;
            var skills = devskill.Select(d => d.Skill);
            return skills;
        }
    }
}