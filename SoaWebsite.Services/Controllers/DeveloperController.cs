using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SoaWebsite.Web.Models;
using SoaWebsite.Services.Services;
using Newtonsoft.Json;

namespace SoaWebsite.Services.Controllers
{
    [Route("api")]
    public class DeveloperController : Controller
    {
        private readonly DeveloperService service;
        private readonly DeveloperContext _context;
        public DeveloperController(DeveloperContext context)
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

        public IEnumerable<Skill> GetAllSkills()
        {
            return _context.Skills;
        }

        [HttpGet("skill/id/{id}")]
        public Skill SkillById(int id)
        {
            Skill skill = _context.Skills
                            .SingleOrDefault(m => m.ID == id);
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