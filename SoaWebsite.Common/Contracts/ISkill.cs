using System.Collections.Generic;

namespace SoaWebsite.Common.Contracts
{
    public interface ISkill
    {
        int ID { get; set; }

        string Name { get; set; }
        List<IDeveloperSkill> GetDeveloperSkills();
    }
}