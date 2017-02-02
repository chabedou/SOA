using System.Collections.Generic;
using SoaWebsite.Common.Models;

namespace SoaWebsite.Common.Contracts
{
    public interface IDeveloperService
    {
        IEnumerable<Developer> GetAllDevelopers();

        Developer DeveloperWithSkillsById(int? idDeveloper);

        Developer DeveloperById(int? idDeveloper);

        Skill SkillWithDevelopersByName(string skillName);

        void AddDeveloper(Developer developer);

        DeveloperSkill GetDeveloperSkill(int? idDeveloper, int? idSkill);

        void RemoveDeveloper(Developer developer);

        bool AddSkill(int? idDeveloper, Skill skill);

        bool TryRemoveSkill(int? id, int? s);

        IEnumerable<Developer> FindDevelopers(string[] skills, string sortOrder);

        bool SkillExists(int id);

        void Update(Developer developer);

        bool DeveloperExists(int id);

        List<string> Skills();

    }
}