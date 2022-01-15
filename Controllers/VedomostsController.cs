using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Uspevaemost.Data;
using Uspevaemost.Models;

namespace Uspevaemost.Controllers
{
    public class VedomostsController : Controller
    {
        private readonly UspevaemostContext db;
        private readonly ApplicationDbContext aC;

        public VedomostsController(UspevaemostContext context, ApplicationDbContext context1)
        {
            db = context;
            aC = context1;
        }

        // GET: Vedomosts
        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> Index()
        {
            string role = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value;
            if (role == "user")
            {
                var user = User.Identity.Name;
                int userId = (from i in aC.Users where i.Email == user select i.Id).Single();
                var stud = (from s in db.Students where s.ZachetkaId == userId select s.ZachetkaId).Single();
                return View(await db.Vedomosts.Where(vM => vM.IdZach == stud).Include(v => v.IdZachNavigation).Include(v => v.NameSubject).ToListAsync());
            }
            var uspevaemostContext = db.Vedomosts.Include(v => v.IdZachNavigation).Include(v => v.NameSubject);
            return View(await uspevaemostContext.ToListAsync());
        }

        // GET: Vedomosts/Details/5
        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vedomost = await db.Vedomosts
                .Include(v => v.IdZachNavigation)
                .Include(v => v.NameSubject)
                .FirstOrDefaultAsync(m => m.Idvedomost == id);
            if (vedomost == null)
            {
                return NotFound();
            }

            return View(vedomost);
        }

        // GET: Vedomosts/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            Vedomost ved = new Vedomost();
            ViewData["IdZach"] = new SelectList(db.Students, "ZachetkaId", "Fio");
            ViewData["NameSubjectId"] = new SelectList(db.Subjects, "IdnameSubject", "NameSubject");
            return View(ved);
        }

        // POST: Vedomosts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Idvedomost,Mark,NameSubjectId,IdZach,Marknumb")] Vedomost ved)
        {
            if (ModelState.IsValid)
            {
                db.Add(ved);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdZach"] = new SelectList(db.Students, "ZachetkaId", "Fio", ved.IdZach);
            ViewData["NameSubjectId"] = new SelectList(db.Subjects, "IdnameSubject", "NameSubject", ved.NameSubjectId);
            return View(ved);
        }

        // GET: Vedomosts/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vedomost = await db.Vedomosts.FindAsync(id);
            if (vedomost == null)
            {
                return NotFound();
            }
            ViewData["IdZach"] = new SelectList(db.Students, "ZachetkaId", "Fio", vedomost.IdZach);
            ViewData["NameSubjectId"] = new SelectList(db.Subjects, "IdnameSubject", "NameSubject", vedomost.NameSubjectId);
            return View(vedomost);
        }

        // POST: Vedomosts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Idvedomost,Mark,NameSubjectId,IdZach,Marknumb")] Vedomost vedomost)
        {
            if (id != vedomost.Idvedomost)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(vedomost);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VedomostExists(vedomost.Idvedomost))
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
            ViewData["IdZach"] = new SelectList(db.Students, "ZachetkaId", "Fio", vedomost.IdZach);
            ViewData["NameSubjectId"] = new SelectList(db.Subjects, "IdnameSubject", "NameSubject", vedomost.NameSubjectId);
            return View(vedomost);
        }

        // GET: Vedomosts/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vedomost = await db.Vedomosts
                .Include(v => v.IdZachNavigation)
                .Include(v => v.NameSubject)
                .FirstOrDefaultAsync(m => m.Idvedomost == id);
            if (vedomost == null)
            {
                return NotFound();
            }

            return View(vedomost);
        }

        // POST: Vedomosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vedomost = await db.Vedomosts.FindAsync(id);
            db.Vedomosts.Remove(vedomost);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VedomostExists(int id)
        {
            return db.Vedomosts.Any(e => e.Idvedomost == id);
        }
    }
}
