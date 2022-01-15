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
    public class StudentsController : Controller
    {
        private readonly UspevaemostContext db;
        private readonly ApplicationDbContext aC;

        public StudentsController(UspevaemostContext context, ApplicationDbContext context1)
        {
            db = context;
            aC = context1;
        }
        

        // GET: Students
        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> Index()
        {
            var uspevaemostContext = db.Students.Include(s => s.IdgroupNavigation);
            string role = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value;
            if (role == "user")
            {
                var user = User.Identity.Name;
                int userId = (from i in aC.Users where i.Email == user select i.Id).Single();
                return View(await db.Students.Where(vM => vM.ZachetkaId == userId).Include(s => s.IdgroupNavigation).ToListAsync());
            }
            return View(await uspevaemostContext.ToListAsync());
        }

        // GET: Students/Details/5
        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await db.Students
                .Include(s => s.IdgroupNavigation)
                .FirstOrDefaultAsync(m => m.ZachetkaId == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            ViewData["Idgroup"] = new SelectList(db.Academgroups, "GroupId", "GroupId");
            Student student = new Student();
            return View(student);
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ZachetkaId,Zachetka,Fio,DateStud,Idgroup")] Student student)
        {
            if (ModelState.IsValid)
            {

                var dbst = (from st in db.Students
                           where st.Zachetka == student.Zachetka
                           select st).Count();

                if(dbst != 0)
                {
                    ViewBag.Message = "Зачетка не уникальна!";
                    ViewData["Idgroup"] = new SelectList(db.Academgroups, "GroupId", "GroupId", student.Idgroup);
                    return View("Create");
                }
                else
                {
                    DateTime datemin = new DateTime(1995, 1, 1);
                    DateTime datemax = new DateTime(2004, 12, 31);
                    DateTime dbdateSt = student.DateStud;

                    if(datemin > dbdateSt || datemax < dbdateSt)
                    {
                        ViewBag.Message = "Вы вышли за границы диапазона!";
                        ViewData["Idgroup"] = new SelectList(db.Academgroups, "GroupId", "GroupId", student.Idgroup);
                        return View("Create");
                    }
                    else
                    {
                        var dbst1 = (from st in db.Students
                                     where st.Idgroup == student.Idgroup
                                     select st).Count();

                        if (dbst1 > 60)
                        {
                            ViewBag.Message = "Студентов в группе больше 60!";
                            ViewData["Idgroup"] = new SelectList(db.Academgroups, "GroupId", "GroupId", student.Idgroup);
                            return View("Create");
                        }
                        else
                        {
                            Academgroup acgr1 = (from acgr in db.Academgroups
                                                 where acgr.GroupId == student.Idgroup
                                                 select acgr).Single();

                            int godpostchet = (2000 + acgr1.Godpostup) - student.DateStud.Year;
                            if (godpostchet < 18)
                            {
                                ViewBag.Message = "Вам нет 18 лет";
                                ViewData["Idgroup"] = new SelectList(db.Academgroups, "GroupId", "GroupId", student.Idgroup);
                                return View("Create");
                            }
                            else
                            {
                                db.Add(student);
                                await db.SaveChangesAsync();
                                return RedirectToAction(nameof(Index));
                            }
                        }
                    }
                   
                }
                
            }
            ViewData["Idgroup"] = new SelectList(db.Academgroups, "GroupId", "GroupId", student.Idgroup);
            return View(student);
        }

        // GET: Students/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await db.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            ViewData["Idgroup"] = new SelectList(db.Academgroups, "GroupId", "GroupId", student.Idgroup);
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ZachetkaId,Zachetka,Fio,DateStud,Idgroup")] Student student)
        {
            if (id != student.ZachetkaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var dbst = (from st in db.Students
                                where st.Zachetka == student.Zachetka && st.ZachetkaId != student.ZachetkaId
                                select st).Count();

                    if (dbst != 0)
                    {
                        ViewBag.Message = "Зачетка не уникальна!";
                        ViewData["Idgroup"] = new SelectList(db.Academgroups, "GroupId", "GroupId", student.Idgroup);
                        return View("Edit");
                    }
                    else
                    {
                        DateTime datemin = new DateTime(1995, 1, 1);
                        DateTime datemax = new DateTime(2004, 12, 31);
                        DateTime dbdateSt = student.DateStud;

                        if (datemin > dbdateSt || datemax < dbdateSt)
                        {
                            ViewBag.Message = "Вы вышли за границы диапазона!";
                            ViewData["Idgroup"] = new SelectList(db.Academgroups, "GroupId", "GroupId", student.Idgroup);
                            return View("Edit");
                        }
                        else
                        {
                            var dbst1 = (from st in db.Students
                                         where st.Idgroup == student.Idgroup
                                         select st).Count();

                            if (dbst1 > 60)
                            {
                                ViewBag.Message = "Студентов в группе больше 60!";
                                ViewData["Idgroup"] = new SelectList(db.Academgroups, "GroupId", "GroupId", student.Idgroup);
                                return View("Edit");
                            }
                            else
                            {
                                Academgroup acgr1 = (from acgr in db.Academgroups
                                                     where acgr.GroupId == student.Idgroup
                                                     select acgr).Single();

                                int godpostchet = (2000 + acgr1.Godpostup) - student.DateStud.Year;
                                if (godpostchet < 18)
                                {
                                    ViewBag.Message = "Вам нет 18 лет";
                                    ViewData["Idgroup"] = new SelectList(db.Academgroups, "GroupId", "GroupId", student.Idgroup);
                                    return View("Edit");
                                }
                                else
                                {
                                    db.Update(student);
                                    await db.SaveChangesAsync();
                                }
                            }
                        }

                    }
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.ZachetkaId))
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
            ViewData["Idgroup"] = new SelectList(db.Academgroups, "GroupId", "GroupId", student.Idgroup);
            return View(student);
        }

        // GET: Students/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await db.Students
                .Include(s => s.IdgroupNavigation)
                .FirstOrDefaultAsync(m => m.ZachetkaId == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await db.Students.FindAsync(id);
            db.Students.Remove(student);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return db.Students.Any(e => e.ZachetkaId == id);
        }
    }
}
