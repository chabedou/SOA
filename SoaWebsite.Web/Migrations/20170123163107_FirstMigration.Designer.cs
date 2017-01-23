using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using SoaWebsite.Web.Models;

namespace SoaWebsite.Web.Migrations
{
    [DbContext(typeof(DeveloperContext))]
    [Migration("20170123163107_FirstMigration")]
    partial class FirstMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SoaWebsite.Web.Models.Developer", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.HasKey("ID");

                    b.ToTable("Developers");
                });

            modelBuilder.Entity("SoaWebsite.Web.Models.DeveloperSkill", b =>
                {
                    b.Property<int>("DeveloperId");

                    b.Property<int>("SkillId");

                    b.HasKey("DeveloperId", "SkillId");

                    b.HasIndex("DeveloperId");

                    b.HasIndex("SkillId");

                    b.ToTable("DeveloperSkill");
                });

            modelBuilder.Entity("SoaWebsite.Web.Models.Skill", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.ToTable("Skills");
                });

            modelBuilder.Entity("SoaWebsite.Web.Models.DeveloperSkill", b =>
                {
                    b.HasOne("SoaWebsite.Web.Models.Developer", "Developer")
                        .WithMany("DeveloperSkills")
                        .HasForeignKey("DeveloperId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SoaWebsite.Web.Models.Skill", "Skill")
                        .WithMany("DeveloperSkills")
                        .HasForeignKey("SkillId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
