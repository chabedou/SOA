using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoaWebsite.Common.Contracts;
using SoaWebsite.Common.Models;


namespace SoaWebsite.Web.Controllers
{
    public class SkillsController : Controller
    {
        private ISkillService service;

        public SkillsController(ISkillService service)
        {
            this.service = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Skill skill)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            return View(skill);
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            return View();
        }

        // POST: Skills/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("ID,FirstName,LastName")] Skill skill)
        {
            if (id != skill.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SkillExists(skill.ID))
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
            return View(skill);
        }

        // GET: Skills/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            return View();
        }

        // POST: Skills/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            return RedirectToAction("Index");
        }

        private bool SkillExists(int id)
        {
            return true;
        }
    }
}
