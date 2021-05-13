using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using University.Data;
using University.Models;
using University.ViewModels;

namespace University.Controllers
{
    public class StudentsController : Controller
    {
        private readonly UniversityContext _context;

        public StudentsController(UniversityContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index(string StudentStudentId, string SearchName, string SearchLast)
        {
            IQueryable<Student> students = _context.Student.AsQueryable();
            IQueryable<string> StudentIdQuery = (IQueryable<string>)_context.Student.OrderBy(m => m.StudentId).Select(m => m.StudentId).Distinct(); // dodadov (IQueryable<string>) inaku mi javuva greshka
            

            if (!string.IsNullOrEmpty(SearchName))
            {
                students = students.Where(s => s.FirstName.Contains(SearchName)); // ako go sodrzi soodvetnoto ime
            }
            if (!string.IsNullOrEmpty(SearchLast))
            {
                students = students.Where(s => s.LastName.Contains(SearchLast)); // ako go sodrzi soodvetnoto ime
            }
            if (!string.IsNullOrEmpty(StudentStudentId))
            {
                students = students.Where(s => s.StudentId == StudentStudentId);
            }


            students = students.Include(c => c.Courses).ThenInclude(c => c.Course); //gi dodavame i Kursevite kaj studentite

            var StudentVM = new StudentSearchViewModel
            {
                StudentIds = new SelectList(await StudentIdQuery.ToListAsync()),
                Students =await students.ToListAsync()

            };
            return View(StudentVM);
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(long? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .Include(n => n.Courses).ThenInclude(n => n.Course) //dodadeno
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StudentId,FirstName,LastName,EnrollmentDate,AcquiredCredits,CurrentSemester,EducationLevel")] Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,StudentId,FirstName,LastName,EnrollmentDate,AcquiredCredits,CurrentSemester,EducationLevel")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
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
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var student = await _context.Student.FindAsync(id);
            _context.Student.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(long id)
        {
            return _context.Student.Any(e => e.Id == id);
        }
    }
}
