using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SoaWebsite.Web.Models;
using SoaWebsite.Web.Services;

namespace SoaWebsite.Web.Controllers
{
    public class DevelopersController : Controller
    {
        private readonly DeveloperService service;

        public DevelopersController(DeveloperContext context)
        {
            service = new DeveloperService(context);
        }
        public IActionResult Index(string sortOrder, string searchName, string searchSkill)
        {
            sortOrder = sortOrder == null ? "FirstName.desc" : sortOrder;
            ViewBag.FirstNameSortParm = sortOrder == "FirstName" ? "FirstName.desc" : "FirstName";
            ViewBag.LastNameSortParm = sortOrder == "LastName" ? "LastName.desc" : "LastName";
            var list = service.FindDevelopers(searchName, searchSkill, sortOrder);
            return View(list);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Developer developer)
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
        public async Task<IActionResult> AddSkill(int? id, [Bind("Name")] Skill skill)
        {
            if (id == null)
            {
                return NotFound();
            }
            var added = await service.AddSkill(id, skill);
            if (ModelState.IsValid && added)
            {
                return RedirectToAction("Index");
            }
            return View(skill);
        }

        public async Task<IActionResult> Details(int? id)
        {
            Developer developer = await service.DeveloperWithSkillsById(id);
            if (developer == null)
            {
                return NotFound();
            }
            return View(developer);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            Developer developer = await service.DeveloperWithSkillsById(id);
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
        public IActionResult Edit(int id, [Bind("ID,FirstName,LastName")] Developer developer)
        {
            if (id != developer.ID)
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
        public async Task<IActionResult> Delete(int? id)
        {
            var developer = await service.DeveloperById(id);
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Developer developer = await service.DeveloperById(id);
            if (developer != null)
            {
                return RedirectToAction("Index");
            }
            return NotFound();
        }

        // GET: Skills/Delete/5
        public async Task<IActionResult> DeleteSkill(int? id, int? s)
        {
            var developerSkill = await service.GetDeveloperSkill(id, s);
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
        public async Task<IActionResult> DeleteSkillConfirmed(int id, int s)
        {
            var removed = await service.TryRemoveSkill(id, s);
            if (removed)
            {
                return RedirectToAction("Edit", new { id = id });
            }
            return NotFound();
        }
    }
}