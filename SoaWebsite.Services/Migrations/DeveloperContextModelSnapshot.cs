using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using SoaWebsite.Common.Models;

namespace SoaWebsite.Services.Migrations
{
    [DbContext(typeof(DeveloperContext))]
    partial class DeveloperContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SoaWebsite.Common.Models.Developer", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.HasKey("ID");

                    b.ToTable("Developers");
                });

            modelBuilder.Entity("SoaWebsite.Common.Models.DeveloperSkill", b =>
                {
                    b.Property<int>("DeveloperId");

                    b.Property<int>("SkillId");

                    b.HasKey("DeveloperId", "SkillId");

                    b.HasIndex("DeveloperId");

                    b.HasIndex("SkillId");

                    b.ToTable("DeveloperSkill");
                });

            modelBuilder.Entity("SoaWebsite.Common.Models.Skill", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.HasKey("ID");

                    b.ToTable("Skills");
                });

            modelBuilder.Entity("SoaWebsite.Common.Models.DeveloperSkill", b =>
                {
                    b.HasOne("SoaWebsite.Common.Models.Developer", "Developer")
                        .WithMany("DeveloperSkills")
                        .HasForeignKey("DeveloperId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SoaWebsite.Common.Models.Skill", "Skill")
                        .WithMany("DeveloperSkills")
                        .HasForeignKey("SkillId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
