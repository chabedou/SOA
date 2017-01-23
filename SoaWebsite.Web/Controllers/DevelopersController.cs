using System;
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
        private DeveloperContext _context;

        public DevelopersController(DeveloperContext context)
        {
            _context = context;
        }

        public IActionResult Index(string sortOrder,  string searchString)
        {
               ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
               ViewBag.LastNameSortParm = sortOrder == "last_name" ? "last_name_desc" : "last_name";
               var developers = from s in _context.Developers
                            select s;
                if (!String.IsNullOrEmpty(searchString))
                {
                        developers = developers.Where(s => s.LastName.Contains(searchString)
                               || s.FirstName.Contains(searchString));
                }
               switch (sortOrder)
               {
                   case "name_desc":
                        developers = developers.OrderByDescending(s => s.FirstName);
                        break;
                   case "last_name":
                        developers = developers.OrderBy(s => s.LastName);
                        break;
                    case "last_name_desc":
                        developers = developers.OrderByDescending(s => s.LastName);
                        break;
                  default:
                        developers = developers.OrderBy(s => s.FirstName);
                    break;
                }
            return View(developers.ToList());
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
        public async Task<IActionResult> AddSkill(int? id,[Bind("Name")] Skill skill)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {

                var developer = await _context.Developers.SingleOrDefaultAsync(m => m.ID == id);
                var skillSaved = await _context.Skills.SingleOrDefaultAsync(m => m.Name == skill.Name);
                if(skillSaved == null){
                     _context.Skills.Add(skill);
                     _context.SaveChanges();
                     skillSaved = await _context.Skills.SingleOrDefaultAsync(m => m.Name == skill.Name);
                }
                _context.Relationships.Add(new Relationship(){
                    DeveloperID=developer.ID,
                    SkillID=skillSaved.ID,
                });
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

            var developer = await _context.Developers.SingleOrDefaultAsync(m => m.ID == id);
            if (developer == null)
            {
                return NotFound();
            }
            developer.Relationships =  _context.Relationships.Where(x=>x.DeveloperID==id)
                                        .ToList();
            foreach(var x in developer.Relationships){
                x.Skill=await _context.Skills.SingleOrDefaultAsync(m => m.ID==x.SkillID);
                x.Developer=developer;
            }
            return View(developer);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var developer = await _context.Developers.SingleOrDefaultAsync(m => m.ID == id);
            developer.Relationships =  _context.Relationships.Where(x=>x.DeveloperID==id).ToList();
            foreach(var x in developer.Relationships){
                x.Skill=await _context.Skills.SingleOrDefaultAsync(m => m.ID==x.SkillID);
                x.Developer=developer;
            }
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
                    else
                    {
                        throw;
                    }
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

            var developer = await _context.Developers.SingleOrDefaultAsync(m => m.ID == id);
            if (developer == null)
            {
                return NotFound();
            }

            return View(developer);
        }

        // POST: Developers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var developer = await _context.Developers.SingleOrDefaultAsync(m => m.ID == id);
            _context.Developers.Remove(developer);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: Skills/Delete/5
        public async Task<IActionResult> DeleteSkill(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Skill = await _context.Relationships.SingleOrDefaultAsync(m => m.ID == id);
            if (Skill == null)
            {
                return NotFound();
            }

            return View(Skill);
        }

        // POST: Skills/Delete/5
        [HttpPost, ActionName("DeleteSkill")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSkillConfirmed(int id)
        {
            var skill = await _context.Relationships.SingleOrDefaultAsync(m => m.ID == id);
            _context.Relationships.Remove(skill);
            await _context.SaveChangesAsync();
            return RedirectToAction("Edit",new { id = skill.DeveloperID });
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
