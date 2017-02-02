using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SoaWebsite.Common.Contracts;
using SoaWebsite.Web.ViewModels;

namespace SoaWebsite.Web.Controllers
{

    public class MainController : Controller
    {
        private readonly IDeveloperService service;
        private readonly MainViewModel main = new MainViewModel()
        {
            OneOfSkills = new List<string>(),
            AllSkills = new List<string>()
        };
        public MainController(IDeveloperService service)
        {
            this.service = service;
        }

        public IActionResult Index()
        {
            ViewBag.Skills = service.Skills();
            var list = service.FindDevelopers(new string[] { }, "").ToList();
            main.Developers = list;
            return View(main);
        }

        public IActionResult Details(int idDeveloper)
        {
            main.SelectedForDetails = service.DeveloperWithSkillsById(idDeveloper);
            main.Developers = service.FindDevelopers(new string[] { }, "").ToList();
            return View("Index", main);
        }

        public IActionResult Edit(int? idDeveloper)
        {
            IDeveloper developer = service.DeveloperWithSkillsById(idDeveloper);
            main.Developers = service.FindDevelopers(new string[] { }, "").ToList();
            main.SelectedForEdit = developer;
            return View("Index", main);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int idDeveloper,[Bind("ID,FirstName,LastName")] IDeveloper developer)
        {   
            if(idDeveloper!=developer.ID){
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    service.Update(developer);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!service.DeveloperExists(developer.ID))
                    {
                        return NotFound();
                    }
                    throw;
                }
                main.Developers = service.FindDevelopers(new string[] { }, "").ToList();
                return View("Index", main);
            }
            main.Developers = service.FindDevelopers(new string[] { }, "").ToList();
            main.SelectedForEdit = developer;
            return View("Index", main);
        }
        public IActionResult DeleteSkill(int idDeveloper, int idSkill)
        {
            var removed = service.TryRemoveSkill(idDeveloper, idSkill);
            if (removed)
            {
                return RedirectToAction("Index");
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddSkill(int? idDeveloper, [Bind("ID,Name")] ISkill skill)
        {
            if (idDeveloper == null)
            {
                return NotFound();
            }
            var added = service.AddSkill(idDeveloper, skill);
            if (ModelState.IsValid && added)
            {
                return RedirectToAction("Index");
            }
            return View(skill);
        }
    }
}