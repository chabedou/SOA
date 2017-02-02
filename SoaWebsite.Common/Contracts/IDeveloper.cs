using System.Collections.Generic;

namespace SoaWebsite.Common.Contracts
{
    public interface IDeveloper
    {
        int ID { get; set; }

        string FirstName { get; set; }

        string LastName { get; set; }

        List<IDeveloperSkill> GetDeveloperSkills();

    }
}