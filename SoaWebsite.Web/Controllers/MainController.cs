using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoaWebsite.Common.Contracts;
using SoaWebsite.Common.Models;

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

        public IActionResult Create()
        {
            ViewData["Message"] = "";
            Developer developer = new Developer();
            main.Developers = service.FindDevelopers(new string[] { }, "").ToList();
            main.SelectedForCreate = developer;
            return View("Index", main);
        }
        public IActionResult Index(string[] selectedSkills)
        {
            ViewBag.Skills = service.Skills();
            ViewData["Message"] = "";
            var list = service.FindDevelopers(selectedSkills, "").ToList();
            main.Developers = list;
            return View(main);
        }

        public IActionResult Details(int idDeveloper)
        {
            ViewData["Message"] = "";
            ViewBag.Skills = service.Skills();
            main.SelectedForDetails = service.DeveloperWithSkillsById(idDeveloper);
            main.Developers = service.FindDevelopers(new string[] { }, "").ToList();
            return View("Index", main);
        }

        public IActionResult Edit(int? idDeveloper)
        {
            ViewData["Message"] = "";
            ViewBag.Skills = service.Skills();
            Developer developer = service.DeveloperWithSkillsById(idDeveloper);
            main.Developers = service.FindDevelopers(new string[] { }, "").ToList();
            main.SelectedForEdit = developer;
            return View("Index", main);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int idDeveloper, [Bind("ID,FirstName,LastName")] Developer developer)
        {
            ViewBag.Skills = service.Skills();
            main.Developers = service.FindDevelopers(new string[] { }, "").ToList();
            if (idDeveloper != developer.ID)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    service.Update(developer);
                    ViewData["Message"] = "Success : Developer was modified";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!service.DeveloperExists(developer.ID))
                    {
                        ViewData["Message"] = "Fail : Developer was already in the database";
                    }
                    else
                    {
                        ViewData["Message"] = "Fail : Developer can not be added";
                    }
                    throw;
                }
            }
            else
            {
                ViewData["Message"] = "Fail : the name of developer is not valid";
                main.SelectedForEdit = developer;
            }

            return View("Index", main);
        }
        public IActionResult DeleteSkill(int idDeveloper, int idSkill)
        {
            var removed = service.TryRemoveSkill(idDeveloper, idSkill);
            if (removed)
            {
                ViewData["Message"] = "Success : the developer was removed";
            }
            else
            {
                ViewData["Message"] = "Fail : the developer can not be removed";
            }
            ViewBag.Skills = service.Skills();
            main.Developers = service.FindDevelopers(new string[] { }, "").ToList();
            return View("Index", main);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddSkill(int? idDeveloper, [Bind("ID,Name")] Skill skill)
        {
            ViewBag.Skills = service.Skills();
            if (ModelState.IsValid)
            {
                var added = service.AddSkill(idDeveloper, skill);
                if (added)
                {
                    ViewData["Message"] = "Success : Skill added to the user";
                }
                else
                {
                    ViewData["Message"] = "Fail : skill exists";
                }
            }
            else
            {
                ViewData["Message"] = "Fail : skill is not valid";
            }
            main.Developers = service.FindDevelopers(new string[] { }, "").ToList();
            return View("Index", main);
        }

        // GET: Developers/Delete/5
        public IActionResult Delete(int? idDeveloper)
        {
            Developer developer = service.DeveloperWithSkillsById(idDeveloper);
            if (developer != null)
            {
                service.RemoveDeveloper(developer);
            }
            return RedirectToAction("index"); ;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Developer developer)
        {
            ViewBag.Skills = service.Skills();
            if (ModelState.IsValid)
            {
                ViewData["Message"] = "Success : developer added";
                service.AddDeveloper(developer);
            }
            else
            {
                ViewData["Message"] = "Fail : Invalid developer name, see below ";
                main.SelectedForCreate = developer;
            }
            main.Developers = service.FindDevelopers(new string[] { }, "").ToList();
            return View("Index", main);
        }
    }
}