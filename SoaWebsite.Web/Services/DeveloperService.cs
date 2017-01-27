using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SoaWebsite.Web.Models;

namespace SoaWebsite.Web.Services
{
    public class DeveloperService
    {
        private readonly DeveloperContext _context;

        public DeveloperService(DeveloperContext context)
        {
            _context = context;
        }
        public async Task<Developer> DeveloperWithSkillsById(int? idDeveloper)
        {
            if (idDeveloper != null)
            {
                var developer = await _context.Developers
                                    .Include(d => d.DeveloperSkills)
                                    .ThenInclude(d => d.Skill)
                                    .SingleOrDefaultAsync(m => m.ID == idDeveloper);
                return developer;
            }
            return null;
        }

        public async Task<Developer> DeveloperById(int? idDeveloper)
        {
            if (idDeveloper != null)
            {
                var developer = await _context.Developers
                    .SingleOrDefaultAsync(m => m.ID == idDeveloper);
                return developer;
            }
            return null;
        }

        public async Task<Skill> SkillWithDevelopersByName(string skillName)
        {
            Skill skill = await _context.Skills
                            .Include(s => s.DeveloperSkills)
                            .ThenInclude(d => d.Developer)
                            .SingleOrDefaultAsync(m => m.Name == skillName);
            return skill;
        }

        public void AddDeveloper(Developer developer)
        {
            developer.DeveloperSkills = new List<DeveloperSkill>();
            _context.Developers.Add(developer);
            _context.SaveChanges();
        }

        public async Task<DeveloperSkill> GetDeveloperSkill(int? idDeveloper, int? idSkill)
        {
            Developer developer = await DeveloperWithSkillsById(idDeveloper);
            if (idSkill != null && developer != null)
            {
                DeveloperSkill developerSkill = developer.DeveloperSkills
                                    .Where(d => d.Skill.ID == idSkill)
                                    .FirstOrDefault();
                return developerSkill;
            }
            return null;
        }
        public void RemoveDeveloper(Developer developer)
        {
            developer.DeveloperSkills = new List<DeveloperSkill>();
            _context.Developers.Remove(developer);
            _context.SaveChanges();
        }

        public async Task<bool> AddSkill(int? idDeveloper, Skill skill)
        {
            Developer developer = await DeveloperWithSkillsById(idDeveloper);
            var remoteSkill = await SkillWithDevelopersByName(skill.Name);
            if (developer != null)
            {
                if (remoteSkill == null)
                {
                    remoteSkill = new Skill() { Name = skill.Name };
                    remoteSkill.DeveloperSkills = new List<DeveloperSkill>();
                    _context.Skills.Add(remoteSkill);
                    _context.SaveChanges();
                }
                var developerSkill = new DeveloperSkill
                {
                    DeveloperId = developer.ID,
                    SkillId = remoteSkill.ID,
                    Skill = remoteSkill,
                    Developer = developer
                };
                developer.DeveloperSkills.Add(developerSkill);
                remoteSkill.DeveloperSkills.Add(developerSkill);
                _context.Update(developer);
                _context.Update(remoteSkill);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<bool> TryRemoveSkill(int? id, int? s)
        {
            Developer developer = await DeveloperWithSkillsById(id);
            if (developer != null)
            {
                var developerSkill = developer.DeveloperSkills
                                      .Where(d => d.Skill.ID == s)
                                      .FirstOrDefault();
                var skill = developerSkill.Skill;
                developer.DeveloperSkills.Remove(developerSkill);
                skill.DeveloperSkills.Remove(developerSkill);
                _context.Update(developer);
                _context.Update(skill);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public IEnumerable<Developer> FindDevelopers(string searchName, string searchSkill, string sortOrder)
        {
            var filter = new Filter(searchName, searchSkill);
            var developers = _context.Developers.Include(d => d.DeveloperSkills)
                                     .ThenInclude(x => x.Skill)
                                     .Where(filter.Condition())
                                     .Select(x => x);
            var sorter = new Sorter(sortOrder);
            var list = sorter.Sort(developers).ToList();
            return list;
        }

        public bool SkillExists(int id)
        {
            return _context.Skills.Any(e => e.ID == id);
        }

        public void Update(Developer developer)
        {
            _context.Update(developer);
             _context.SaveChanges();
        }
        public bool DeveloperExists(int id)
        {
            return _context.Developers.Any(e => e.ID == id);
        }

        public List<string> Skills(){
            return _context.Skills.Select(x=>x.Name).ToList();
        }
    }
}