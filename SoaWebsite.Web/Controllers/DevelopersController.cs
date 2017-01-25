﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SoaWebsite.Web.Models;

namespace SoaWebsite.Web.Controllers
{
    public class DevelopersController : Controller
    {
        private readonly DeveloperContext _context;

        public DevelopersController(DeveloperContext context)
        {
            _context = context;
        }

        public IActionResult Index(string sortOrder, string searchName, string searchSkill)
        {
            sortOrder=sortOrder==null?"FirstName.desc":sortOrder;
            ViewBag.FirstNameSortParm = sortOrder == "FirstName" ? "FirstName.desc" : "FirstName";
            ViewBag.LastNameSortParm = sortOrder == "LastName" ? "LastName.desc" : "LastName";
            var developerFilter = new DeveloperFilter(searchName,searchSkill);
            var developers = _context.Developers.Include(d => d.DeveloperSkills)
                                     .ThenInclude(x => x.Skill)
                                     .Where(developerFilter.Filter())
                                     .Select(x => x);
            var developorOrder = new DeveloperSorter(sortOrder);
            var list=developorOrder.Sort(developers).ToList();
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
                developer.DeveloperSkills = new List<DeveloperSkill>();
                _context.Developers.Add(developer);
                _context.SaveChanges();
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
            if (ModelState.IsValid)
            {
                var developer = await _context.Developers.Include(d => d.DeveloperSkills)
                                              .SingleOrDefaultAsync(m => m.ID == id);
                var skillSaved = await _context.Skills.Include(s => s.DeveloperSkills)
                                               .SingleOrDefaultAsync(m => m.Name == skill.Name);
                if (skillSaved == null)
                {
                    skill.DeveloperSkills = new List<DeveloperSkill>();
                    skillSaved = skill;
                    _context.Skills.Add(skill);
                    _context.SaveChanges();
                }
                var developerSkill = new DeveloperSkill
                                     {
                                         DeveloperId = developer.ID,
                                         SkillId = skillSaved.ID,
                                         Skill = skillSaved,
                                         Developer = developer
                                     };
                developer.DeveloperSkills.Add(developerSkill);
                skillSaved.DeveloperSkills.Add(developerSkill);
                _context.Update(developer);
                _context.Update(skillSaved);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(skill);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var developer = await _context.Developers
                                          .Include(d => d.DeveloperSkills)
                                          .ThenInclude(d => d.Skill)
                                          .SingleOrDefaultAsync(m => m.ID == id);
            if (developer == null)
            {
                return NotFound();
            }
            return View(developer);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var developer = await _context.Developers
                                          .Include(d => d.DeveloperSkills)
                                          .ThenInclude(d => d.Skill)
                                          .SingleOrDefaultAsync(m => m.ID == id);
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
        public async Task<IActionResult> Edit(int id, [Bind("ID,FirstName,LastName")] Developer developer)
        {
            if (id != developer.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(developer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeveloperExists(developer.ID))
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
            if (id == null)
            {
                return NotFound();
            }

            var developer = await _context.Developers
                                          .SingleOrDefaultAsync(m => m.ID == id);
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
            var developer = await _context.Developers.SingleOrDefaultAsync(m => m.ID == id);
            _context.Developers.Remove(developer);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: Skills/Delete/5
        public async Task<IActionResult> DeleteSkill(int? id, int? s)
        {

            if (id == null || s == null)
            {
                return NotFound();
            }

            var developer = await _context.Developers.Include(d => d.DeveloperSkills)
                                          .ThenInclude(d => d.Skill)
                                          .SingleOrDefaultAsync(m => m.ID == id);
            if (developer == null)
            {
                return NotFound();
            }
            var developerSkill = developer.DeveloperSkills
                                          .Where(d => d.Skill.ID == s)
                                          .FirstOrDefault();
            return View(developerSkill);
        }

        // POST: Skills/Delete/5
        [HttpPost]
        [ActionName("DeleteSkill")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSkillConfirmed(int id, int s)
        {
            var developer = await _context.Developers
                                          .Include(d => d.DeveloperSkills)
                                          .ThenInclude(d => d.Skill)
                                          .SingleOrDefaultAsync(m => m.ID == id);
            var skill = await _context.Skills
                                      .Include(d => d.DeveloperSkills)
                                      .SingleOrDefaultAsync(m => m.ID == s);
            var developerSkill = developer.DeveloperSkills
                                          .Where(d => d.Skill.ID == s)
                                          .FirstOrDefault();
            developer.DeveloperSkills.Remove(developerSkill);
            skill.DeveloperSkills.Remove(developerSkill);
            _context.Update(developer);
            _context.Update(skill);
            await _context.SaveChangesAsync();
            return RedirectToAction("Edit", new { id = developer.ID });
        }

        private bool SkillExists(int id)
        {
            return _context.Skills.Any(e => e.ID == id);
        }

        private bool DeveloperExists(int id)
        {
            return _context.Developers.Any(e => e.ID == id);
        }
    }
}