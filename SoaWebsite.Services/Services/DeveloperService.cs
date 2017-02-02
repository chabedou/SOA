using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SoaWebsite.Services.Models;
using SoaWebsite.Services.Services;
using SoaWebsite.Common.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace SoaWebsite.Services.Services
{
    public class DeveloperService : ServiceDescriptor , IDeveloperService
    {
        private readonly DeveloperContext _context;

        public DeveloperService(DeveloperContext context) : base(typeof(DeveloperService),typeof(DeveloperService),new ServiceLifetime())
        {
            _context = context;
        }

        public IEnumerable<IDeveloper> GetAllDevelopers()
        {
            return _context.Developers;
        }

        public IDeveloper DeveloperWithSkillsById(int? idDeveloper)
        {
            if (idDeveloper != null)
            {
                var developer = _context.Developers
                                    .Include(d => d.DeveloperSkills)
                                    .ThenInclude(d => d.Skill)
                                    .SingleOrDefaultAsync(m => m.ID == idDeveloper).Result;
                return developer;
            }
            return null;
        }

        public IDeveloper DeveloperById(int? idDeveloper)
        {
            if (idDeveloper != null)
            {
                var developer = _context.Developers
                    .SingleOrDefaultAsync(m => m.ID == idDeveloper).Result;
                return developer;
            }
            return null;
        }

        public ISkill SkillWithDevelopersByName(string skillName)
        {
            Skill skill = _context.Skills
                            .Include(s => s.DeveloperSkills)
                            .ThenInclude(d => d.Developer)
                            .SingleOrDefaultAsync(m => m.Name == skillName).Result;
            return skill;
        }

        public void AddDeveloper(IDeveloper ideveloper)
        {
            var developer=(Developer)ideveloper;
             developer.DeveloperSkills = new List<DeveloperSkill>();
            _context.Developers.Add(developer);
            _context.SaveChanges();
        }

        public IDeveloperSkill GetDeveloperSkill(int? idDeveloper, int? idSkill)
        {
            IDeveloper developer = (Developer) DeveloperWithSkillsById(idDeveloper);
            if (idSkill != null && developer != null)
            {
                IDeveloperSkill developerSkill =  developer.GetDeveloperSkills()
                                    .Where(d => d.SkillId == idSkill)
                                    .FirstOrDefault();
                return developerSkill;
            }
            return null;
        }
        public void RemoveDeveloper(IDeveloper ideveloper)
        {
            var developer=(Developer)ideveloper;
            foreach (var developerSkill in developer.DeveloperSkills)
            {
                var skill = developerSkill.Skill;
                skill.DeveloperSkills.Remove(developerSkill);
                _context.Update(skill);
            }
            developer.DeveloperSkills = new List<DeveloperSkill>();
            _context.Update(developer);
            _context.SaveChanges();
            _context.Developers.Remove(developer);
            _context.SaveChanges();
        }

        public bool AddSkill(int? idDeveloper, ISkill skill)
        {
            Developer developer = (Developer)DeveloperWithSkillsById(idDeveloper);
            Skill remoteSkill = (Skill)SkillWithDevelopersByName(skill.Name);
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
                    Skill = (Skill)remoteSkill,
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

        public bool TryRemoveSkill(int? id, int? s)
        {
            Developer developer = (Developer)DeveloperWithSkillsById(id);
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

        public IEnumerable<IDeveloper> FindDevelopers(string[] skills, string sortOrder)
        {
            var filter = new Filter(skills);
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

        public void Update(IDeveloper developer)
        {
            _context.Update(developer);
            _context.SaveChanges();
        }
        
        public bool DeveloperExists(int id)
        {
            return _context.Developers.Any(e => e.ID == id);
        }

        public List<string> Skills()
        {
            return _context.Skills.Select(x => x.Name).ToList();
        }

    }
}