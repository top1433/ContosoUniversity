using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;
using ContosoUniversity.Models;

namespace ContosoUniversity.Controllers
{
    public class CoursesController : Controller
    {
        private readonly SchoolContext _context;

        public CoursesController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Courses
        public async Task<IActionResult> Index()
        {
            //var schoolContext = _context.Course.Include(c => c.Department);

            var courses = _context.Course.Include(c => c.Department);
            return View(await courses.ToListAsync());
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .Include(c => c.Department)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.CourseID == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
        //public IActionResult Create()
        //{
        //    ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "DepartmentID");
        //    return View();
        //}


        public IActionResult Create()
        {
            PopulateDepartmentDropDownList();
            return View();
        }




        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("CourseID,Title,Credits,DepartmentID")] Course course)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(course);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "DepartmentID", course.DepartmentID);
        //    return View(course);
        //}

        public async Task<IActionResult> Create([Bind("CourseID,Title,Credits,DepartmentID")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateDepartmentDropDownList(course.DepartmentID);
            return View(course);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            //ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "DepartmentID", course.DepartmentID);
            PopulateDepartmentDropDownList(course.DepartmentID);
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("CourseID,Title,Credits,DepartmentID")] Course course)
        //{
        //    if (id != course.CourseID)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(course);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!CourseExists(course.CourseID))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "DepartmentID", course.DepartmentID);
        //    return View(course);
        //}

        [HttpPost,ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var courseToUpdate = await _context.Course
                .FirstOrDefaultAsync(c=>c.CourseID == id);

            if(await TryUpdateModelAsync<Course>(courseToUpdate,"",c=>c.CourseID,c=>c.DepartmentID,c=>c.Title))
            {
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch(DbUpdateException /* ex */)
                {
                    ModelState.AddModelError("","unable to save changes. " + 
                        "try again, and if the problem persists,  " +
                        " see your system administrator .");
                }

                return RedirectToAction(nameof(Index));
            }

            PopulateDepartmentDropDownList(courseToUpdate.DepartmentID);
            return View(courseToUpdate);
        }

        //GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .Include(c => c.Department)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.CourseID == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Course.FindAsync(id);
            _context.Course.Remove(course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            return _context.Course.Any(e => e.CourseID == id);
        }





        private void PopulateDepartmentDropDownList(object selectedDepartment = null)
        {
            var departmentQuery = from d in _context.Departments
                                  orderby d.Name
                                  select d;
            ViewBag.DepartmentID = new SelectList(departmentQuery.AsNoTracking(),"DepartmentID","Name","SelectedDepartment");
        }
    }
}
