using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SoaWebsite.Common.Models;
using SoaWebsite.Common.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace SoaWebsite.Services.Services
{
    public class DeveloperService : ServiceDescriptor, IDeveloperService
    {
        private readonly DeveloperContext _context;

        public DeveloperService(DeveloperContext context) : base(typeof(DeveloperService),typeof(DeveloperService),new ServiceLifetime())
        {
            _context = context;
        }

        public IEnumerable<Developer> GetAllDevelopers()
        {
            return _context.Developers;
        }

        private Developer DeveloperWithSkillsById(int? idDeveloper)
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

        public Developer DeveloperById(int? idDeveloper)
        {
            if (idDeveloper != null)
            {
                var developer = _context.Developers
                    .SingleOrDefaultAsync(m => m.ID == idDeveloper).Result;
                return developer;
            }
            return null;
        }

        private Skill SkillWithDevelopersByName(string skillName)
        {
            Skill skill = _context.Skills
                            .Include(s => s.DeveloperSkills)
                            .ThenInclude(d => d.Developer)
                            .SingleOrDefaultAsync(m => m.Name == skillName).Result;
            return skill;
        }

        public void AddDeveloper(Developer Developer)
        {
            var developer=(Developer)Developer;
             developer.DeveloperSkills = new List<DeveloperSkill>();
            _context.Developers.Add(developer);
            _context.SaveChanges();
        }

        public DeveloperSkill GetDeveloperSkill(int? idDeveloper, int? idSkill)
        {
            Developer developer = (Developer) DeveloperWithSkillsById(idDeveloper);
            if (idSkill != null && developer != null)
            {
                DeveloperSkill developerSkill =  developer.DeveloperSkills
                                    .Where(d => d.SkillId == idSkill)
                                    .FirstOrDefault();
                return developerSkill;
            }
            return null;
        }
        public void RemoveDeveloper(Developer Developer)
        {
            var developer=(Developer)Developer;
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

        public bool AddSkill(int? idDeveloper, Skill skill)
        {
            Developer developer = DeveloperWithSkillsById(idDeveloper);
            
            Skill remoteSkill = SkillWithDevelopersByName(skill.Name);
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

        public IEnumerable<Developer> FindDevelopers(string[] skills, string sortOrder)
        {
            var filter = new Filter(skills);
            var developers = _context.Developers
                                     .Include(d => d.DeveloperSkills)
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

        public List<string> Skills()
        {
            return _context.Skills.Select(x => x.Name).ToList();
        }

        Developer IDeveloperService.DeveloperWithSkillsById(int? idDeveloper)
        {
            return DeveloperWithSkillsById(idDeveloper);
        }

        Skill IDeveloperService.SkillWithDevelopersByName(string skillName)
        {
            return SkillWithDevelopersByName(skillName);
        }

    }
}