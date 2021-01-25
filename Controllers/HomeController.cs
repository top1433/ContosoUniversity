using ContosoUniversity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;
using ContosoUniversity.Models.SchoolViewModels;

namespace ContosoUniversity.Controllers
{
    public class HomeController : Controller
    {
        private readonly SchoolContext _context;

        public HomeController(SchoolContext context)
        {
            _context = context;
        }
        
        public async Task<ActionResult> About()
        {
            IQueryable<EnrollmentDateGroup> data =
                from s in _context.Student
                group s by s.EnRollmentDate into dataGroup
                select new EnrollmentDateGroup
                {
                    EnrollmentDate = dataGroup.Key,
                    StudentCount = dataGroup.Count()
                };

            return View(await data.AsNoTracking().ToListAsync());
        }
    }
}
