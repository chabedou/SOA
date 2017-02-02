using System.Collections.Generic;

namespace SoaWebsite.Common.Contracts
{
    public interface IIDeveloperService
    {
        IEnumerable<IDeveloper> GetAllIDevelopers();

        IDeveloper DeveloperWithSkillsById(int? idDeveloper);

        IDeveloper DeveloperById(int? idDeveloper);

        ISkill SkillWithDevelopersByName(string skillName);

        void AddDeveloper(IDeveloper developer);

        IDeveloperSkill GetDeveloperISkill(int? idDeveloper, int? idSkill);

        void RemoveIDeveloper(IDeveloper developer);

        bool AddSkill(int? idDeveloper, ISkill skill);

        bool TryRemoveSkill(int? id, int? s);

        IEnumerable<IDeveloper> FindDevelopers(string[] skills, string sortOrder);

        bool SkillExists(int id);

        void Update(IDeveloper developer);

        bool DeveloperExists(int id);

        List<string> Skills();

    }
}