using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SoaWebsite.Common.Contracts;
using SoaWebsite.Services.Models;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace SoaWebsite.Web.Controllers
{

    public class DevelopersController : Controller
    {
        private readonly IDeveloperService service;
        private readonly ILogger logger;

        public DevelopersController(IDeveloperService service, ILogger<DevelopersController> logger)
        {
            this.service = service;
            this.logger = logger;
        }

        public IActionResult Index(string sortOrder, string[] selectedSkills)
        {
            sortOrder = sortOrder == null ? "FirstName.desc" : sortOrder;
            ViewBag.FirstNameSortParm = sortOrder == "FirstName" ? "FirstName.desc" : "FirstName";
            ViewBag.LastNameSortParm = sortOrder == "LastName" ? "LastName.desc" : "LastName";
            ViewBag.Skills = service.Skills();
            var list = service.FindDevelopers(selectedSkills, sortOrder).ToList();
            return View(list);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IDeveloper developer)
        {
            if (ModelState.IsValid)
            {
                service.AddDeveloper(developer);   
                return RedirectToAction("Index");
            }
            return View(developer);
        }

        public IActionResult AddSkill()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddSkill(int? idDeveloper, [Bind("Name")] Skill skill)
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

        public IActionResult Details(int? idDeveloper)
        {
            IDeveloper developer = service.DeveloperWithSkillsById(idDeveloper);
            if (developer == null)
            {
                return NotFound();
            }
            return View(developer);
        }

        public IActionResult Edit(int? idDeveloper)
        {
            IDeveloper developer = service.DeveloperWithSkillsById(idDeveloper);
            if (developer == null)
            {
                return NotFound();
            }
            return View(developer);
        }

        // POST: Developers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int idDeveloper, [Bind("ID,FirstName,LastName")] IDeveloper developer)
        {
            if (idDeveloper != developer.ID)
            {
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
                return RedirectToAction("Index");
            }
            return View(developer);
        }

        // GET: Developers/Delete/5
        public IActionResult Delete(int? idDeveloper)
        {
            var developer = service.DeveloperById(idDeveloper);
            if (developer == null)
            {
                return NotFound();
            }
            return View(developer);
        }

        // POST: Developers/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int idDeveloper)
        {
            IDeveloper developer = service.DeveloperWithSkillsById(idDeveloper);
            if (developer != null)
            {
                service.RemoveDeveloper(developer);
                return RedirectToAction("Index");
            }
            return NotFound();
        }

        // GET: Skills/Delete/5
        public IActionResult DeleteSkill(int? idDeveloper, int? idSkill)
        {
            var developerSkill = service.GetDeveloperSkill(idDeveloper, idSkill);
            if (developerSkill == null)
            {
                return NotFound();
            }
            return View(developerSkill);
        }

        // POST: Skills/Delete/5
        [HttpPost]
        [ActionName("DeleteSkill")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteSkillConfirmed(int idDeveloper, int idSkill)
        {
            var removed = service.TryRemoveSkill(idDeveloper, idSkill);
            if (removed)
            {
                return RedirectToAction("Edit", new { idDeveloper = idDeveloper });
            }
            return NotFound();
        }
    }
}