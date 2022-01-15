using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Uspevaemost.Data;
using Uspevaemost.Models;

namespace Uspevaemost.Controllers
{
    public class SubjectsController : Controller
    {
        private readonly UspevaemostContext db;

        public SubjectsController(UspevaemostContext context)
        {
            db = context;
        }

        // GET: Subjects
        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> Index()
        {
            return View(await db.Subjects.ToListAsync());
        }

        // GET: Subjects/Details/5
        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await db.Subjects
                .FirstOrDefaultAsync(m => m.IdnameSubject == id);
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        // GET: Subjects/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            Subject subject = new Subject();
            return View(subject);
        }

        // POST: Subjects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdnameSubject,NameSubject,Fioprepod")] Subject subject)
        {
            if (ModelState.IsValid)
            {
                var dbsub = (from sub in db.Subjects
                            where sub.NameSubject == subject.NameSubject
                            select sub).Count();
                if(dbsub != 0)
                {
                    ViewBag.Message = "Этот предмет уже существует!";
                    return View("Create");
                }

                db.Add(subject);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(subject);
        }

        // GET: Subjects/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await db.Subjects.FindAsync(id);
            if (subject == null)
            {
                return NotFound();
            }
            return View(subject);
        }

        // POST: Subjects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdnameSubject,NameSubject,Fioprepod")] Subject subject)
        {
            if (id != subject.IdnameSubject)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var dbsub = (from sub in db.Subjects
                                 where sub.NameSubject == subject.NameSubject && sub.IdnameSubject != subject.IdnameSubject
                                 select sub).Count();
                    if (dbsub != 0)
                    {
                        ViewBag.Message = "Этот предмет уже существует!";
                        return View("Edit");
                    }
                    db.Update(subject);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubjectExists(subject.IdnameSubject))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(subject);
        }

        // GET: Subjects/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await db.Subjects
                .FirstOrDefaultAsync(m => m.IdnameSubject == id);
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        // POST: Subjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subject = await db.Subjects.FindAsync(id);
            db.Subjects.Remove(subject);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubjectExists(int id)
        {
            return db.Subjects.Any(e => e.IdnameSubject == id);
        }
    }
}
