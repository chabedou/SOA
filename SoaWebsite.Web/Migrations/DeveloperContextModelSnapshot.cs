using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using SoaWebsite.Web.Models;

namespace SoaWebsite.Web.Migrations
{
    [DbContext(typeof(DeveloperContext))]
    partial class DeveloperContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

            modelBuilder.Entity("SoaWebsite.Web.Models.Skill", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DeveloperID");

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.HasIndex("DeveloperID");

                    b.ToTable("Skills");
                });

            modelBuilder.Entity("SoaWebsite.Web.Models.Skill", b =>
                {
                    b.HasOne("SoaWebsite.Web.Models.Developer", "Developer")
                        .WithMany("Skills")
                        .HasForeignKey("DeveloperID")
                        .HasConstraintName("ForeignKey_Skill_Developer")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
