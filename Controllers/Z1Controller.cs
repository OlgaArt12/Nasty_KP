using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uspevaemost.Models;
using Uspevaemost.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Uspevaemost.Controllers
{
    public class Z1Controller : Controller
    {
        private readonly UspevaemostContext db;
        private readonly ApplicationDbContext aC;
        public Z1Controller(UspevaemostContext context, ApplicationDbContext context1)
        {
            db = context;
            aC = context1;
        }

        // GET: Z1Controller
        [Authorize(Roles = "admin, user")]
        public ActionResult Index()
        {
            string role = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value;
            if (role == "user")
            {
                var user = User.Identity.Name;
                int userId = (from i in aC.Users where i.Email == user select i.Id).Single();
                var stud = (from s in db.Students where s.ZachetkaId == userId select s.Fio).Single();
                var rez = db.ZaprosTwos.Where(sF => sF.Fio == stud).ToList();
                return View(rez);
            }
            List<ZaprosTwo> z = db.ZaprosTwos.ToList();
            return View(z);
        }
    }
}
