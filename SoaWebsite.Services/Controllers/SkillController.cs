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
    [Route("api/[controller]")]
    public class SkillController : Controller
    {
        private readonly DeveloperService service;
        private readonly DeveloperContext _context;
        public SkillController(DeveloperContext context)
        {
            service = new DeveloperService(context);
            _context = context;
        }
        [HttpGet]
        public IEnumerable<Skill> GetAllSkills()
        {
            return _context.Skills;
        }

        [HttpGet("id/{id}")]
        public Skill SkillById(int id)
        {
            Skill skill = _context.Skills
                            .SingleOrDefault(m => m.ID == id);
            return skill;
        }

        [HttpGet("name/{skillname}")]
        public Skill SkillByName(string skillName)
        {
            Skill skill = _context.Skills
                            .SingleOrDefault(m => m.Name == skillName);
            return skill;
        }
    }
}