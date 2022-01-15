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
    public class AcademgroupsController : Controller
    {
        private readonly UspevaemostContext _context;
        private readonly ApplicationDbContext aC;

        public AcademgroupsController(UspevaemostContext context, ApplicationDbContext context1)
        {
            _context = context;
            aC = context1;
        }

        // GET: Academgroups
        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> Index()
        {
            string role = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value;
            if (role == "user")
            {
                var user = User.Identity.Name;
                int userId = (from i in aC.Users where i.Email == user select i.Id).Single();
                var stud = (from s in _context.Students where s.ZachetkaId == userId select s.IdgroupNavigation.GroupId).Single();
                return View(await _context.Academgroups.Where(vM => vM.GroupId == stud).ToListAsync());
            }
            return View(await _context.Academgroups.ToListAsync());
        }

        // GET: Academgroups/Details/5
        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dbcountSt = (from cST in _context.Students
                          where cST.Idgroup == id
                          select cST).Count();

            ViewBag.Message = "Студентов в группе: " + dbcountSt;

            var academgroup = await _context.Academgroups
                .FirstOrDefaultAsync(m => m.GroupId == id);
            if (academgroup == null)
            {
                return NotFound();
            }

            return View(academgroup);
        }

        // GET: Academgroups/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            Academgroup academgroup = new Academgroup();
            return View(academgroup);
        }

        // POST: Academgroups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GroupId,Shifr,Godpostup,Number")] Academgroup academgroup)
        {
            var dbacgr = (from acgr in _context.Academgroups
                         where acgr.Shifr == academgroup.Shifr && acgr.Godpostup == academgroup.Godpostup && acgr.Number == academgroup.Number
                         select acgr).Count();
            if(dbacgr != 0)
            {
                ViewBag.Message = "Эта группа уже существует! ";
                return View("Create");
            }
            if (ModelState.IsValid)
            {
                _context.Add(academgroup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(academgroup);
        }

        // GET: Academgroups/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var academgroup = await _context.Academgroups.FindAsync(id);
            if (academgroup == null)
            {
                return NotFound();
            }
            return View(academgroup);
        }

        // POST: Academgroups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GroupId,Shifr,Godpostup,Number")] Academgroup academgroup)
        {
            if (id != academgroup.GroupId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var dbacgr = (from acgr in _context.Academgroups
                                  where acgr.Shifr == academgroup.Shifr && acgr.Godpostup == academgroup.Godpostup && acgr.Number == academgroup.Number && acgr.GroupId != academgroup.GroupId
                                  select acgr).Count();
                    if(dbacgr != 0)
                    {
                        ViewBag.Message = "Эта группа уже существует! ";
                        return View("Edit");
                    }
                    _context.Update(academgroup);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AcademgroupExists(academgroup.GroupId))
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
            return View(academgroup);
        }

        // GET: Academgroups/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var academgroup = await _context.Academgroups
                .FirstOrDefaultAsync(m => m.GroupId == id);
            if (academgroup == null)
            {
                return NotFound();
            }

            return View(academgroup);
        }

        // POST: Academgroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var academgroup = await _context.Academgroups.FindAsync(id);
            _context.Academgroups.Remove(academgroup);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AcademgroupExists(int id)
        {
            return _context.Academgroups.Any(e => e.GroupId == id);
        }
    }
}
