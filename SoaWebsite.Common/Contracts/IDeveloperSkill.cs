namespace SoaWebsite.Common.Contracts
{
    public interface IDeveloperSkill
    {
        int DeveloperId { get; set; }
        IDeveloper IDeveloper();

        int SkillId { get; set; }
        ISkill ISkill();


    }
}