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
    public class EnRollmentsController : Controller
    {
        private readonly SchoolContext _context;

        public EnRollmentsController(SchoolContext context)
        {
            _context = context;
        }

        // GET: EnRollments
        public async Task<IActionResult> Index()
        {
            var schoolContext = _context.EnRollment.Include(e => e.Course).Include(e => e.Student);
            return View(await schoolContext.ToListAsync());
        }

        // GET: EnRollments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enRollment = await _context.EnRollment
                .Include(e => e.Course)
                .Include(e => e.Student)
                .FirstOrDefaultAsync(m => m.EnrollmentID == id);
            if (enRollment == null)
            {
                return NotFound();
            }

            return View(enRollment);
        }

        // GET: EnRollments/Create
        public IActionResult Create()
        {
            ViewData["CourseID"] = new SelectList(_context.Course, "CourseID", "CourseID");
            ViewData["StudentID"] = new SelectList(_context.Student, "ID", "ID");
            return View();
        }

        // POST: EnRollments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EnrollmentID,StudentID,CourseID,Grade")] EnRollment enRollment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(enRollment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseID"] = new SelectList(_context.Course, "CourseID", "CourseID", enRollment.CourseID);
            ViewData["StudentID"] = new SelectList(_context.Student, "ID", "ID", enRollment.StudentID);
            return View(enRollment);
        }

        // GET: EnRollments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enRollment = await _context.EnRollment.FindAsync(id);
            if (enRollment == null)
            {
                return NotFound();
            }
            ViewData["CourseID"] = new SelectList(_context.Course, "CourseID", "CourseID", enRollment.CourseID);
            ViewData["StudentID"] = new SelectList(_context.Student, "ID", "ID", enRollment.StudentID);
            return View(enRollment);
        }

        // POST: EnRollments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EnrollmentID,StudentID,CourseID,Grade")] EnRollment enRollment)
        {
            if (id != enRollment.EnrollmentID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(enRollment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnRollmentExists(enRollment.EnrollmentID))
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
            ViewData["CourseID"] = new SelectList(_context.Course, "CourseID", "CourseID", enRollment.CourseID);
            ViewData["StudentID"] = new SelectList(_context.Student, "ID", "ID", enRollment.StudentID);
            return View(enRollment);
        }

        // GET: EnRollments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enRollment = await _context.EnRollment
                .Include(e => e.Course)
                .Include(e => e.Student)
                .FirstOrDefaultAsync(m => m.EnrollmentID == id);
            if (enRollment == null)
            {
                return NotFound();
            }

            return View(enRollment);
        }

        // POST: EnRollments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var enRollment = await _context.EnRollment.FindAsync(id);
            _context.EnRollment.Remove(enRollment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EnRollmentExists(int id)
        {
            return _context.EnRollment.Any(e => e.EnrollmentID == id);
        }
    }
}
