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
    public class StudPlansController : Controller
    {
        private readonly UspevaemostContext db;
        private readonly ApplicationDbContext aC;

        public StudPlansController(UspevaemostContext context, ApplicationDbContext context1)
        {
            db = context;
            aC = context1;
        }

        // GET: StudPlans
        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> Index()
        {
            string role = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value;
            if (role == "user")
            {
                var user = User.Identity.Name;
                int userId = (from i in aC.Users where i.Email == user select i.Id).Single();
                var stud = (from s in db.Students where s.ZachetkaId == userId select s.IdgroupNavigation.GroupId).Single();
                return View(await db.StudPlans.Where(vM => vM.IdGroup == stud).Include(s => s.IdGroupNavigation).Include(s => s.NameSubjectNavigation).ToListAsync());
            }
            var uspevaemostContext = db.StudPlans.Include(s => s.IdGroupNavigation).Include(s => s.NameSubjectNavigation);
            return View(await uspevaemostContext.ToListAsync());
        }

        // GET: StudPlans/Details/5
        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studPlan = await db.StudPlans
                .Include(s => s.IdGroupNavigation)
                .Include(s => s.NameSubjectNavigation)
                .FirstOrDefaultAsync(m => m.PlanId == id);
            if (studPlan == null)
            {
                return NotFound();
            }

            return View(studPlan);
        }

        // GET: StudPlans/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            StudPlan studplan = new StudPlan();
            ViewData["IdGroup"] = new SelectList(db.Academgroups, "GroupId", "GroupId");
            ViewData["NameSubject"] = new SelectList(db.Subjects, "IdnameSubject", "NameSubject");
            return View();
        }

        // POST: StudPlans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PlanId,Clock,Attestation,NameSubject,IdGroup")] StudPlan studPlan)
        {
            if (ModelState.IsValid)
            {
                db.Add(studPlan);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdGroup"] = new SelectList(db.Academgroups, "GroupId", "GroupId", studPlan.IdGroup);
            ViewData["NameSubject"] = new SelectList(db.Subjects, "IdnameSubject", "NameSubject", studPlan.NameSubject);
            return View(studPlan);
        }

        // GET: StudPlans/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studPlan = await db.StudPlans.FindAsync(id);
            if (studPlan == null)
            {
                return NotFound();
            }
            ViewData["IdGroup"] = new SelectList(db.Academgroups, "GroupId", "GroupId", studPlan.IdGroup);
            ViewData["NameSubject"] = new SelectList(db.Subjects, "IdnameSubject", "NameSubject", studPlan.NameSubject);
            return View(studPlan);
        }

        // POST: StudPlans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PlanId,Clock,Attestation,NameSubject,IdGroup")] StudPlan studPlan)
        {
            if (id != studPlan.PlanId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(studPlan);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudPlanExists(studPlan.PlanId))
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
            ViewData["IdGroup"] = new SelectList(db.Academgroups, "GroupId", "GroupId", studPlan.IdGroup);
            ViewData["NameSubject"] = new SelectList(db.Subjects, "IdnameSubject", "NameSubject", studPlan.NameSubject);
            return View(studPlan);
        }

        // GET: StudPlans/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studPlan = await db.StudPlans
                .Include(s => s.IdGroupNavigation)
                .Include(s => s.NameSubjectNavigation)
                .FirstOrDefaultAsync(m => m.PlanId == id);
            if (studPlan == null)
            {
                return NotFound();
            }

            return View(studPlan);
        }

        // POST: StudPlans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var studPlan = await db.StudPlans.FindAsync(id);
            db.StudPlans.Remove(studPlan);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudPlanExists(int id)
        {
            return db.StudPlans.Any(e => e.PlanId == id);
        }
    }
}
