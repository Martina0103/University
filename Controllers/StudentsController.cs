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
        public async Task<IActionResult> Index(string StudentStudentId, string SearchList)
        {
            IQueryable<Student> students = _context.Student.AsQueryable();
            IQueryable<string> StudentIdQuery = _context.Student.OrderBy(m => m.StudentId).Select(m => m.StudentId).Distinct(); 

            /* ZA PREBARUVANJE SO FULL NAME*/
            if (!string.IsNullOrEmpty(SearchList))
            {
                string[] full = SearchList.Split(' ');
                if(full.Length == 2)
                {
                    var ime = full[0];
                    var prezime = full[1];
                    students = students.Where(s => s.FirstName.Contains(ime) && s.LastName.Contains(prezime));  // Ako sodrzi ime I prezime
                }
                else
                {
                    students = students.Where(s => s.FirstName.Contains(SearchList) || s.LastName.Contains(SearchList)); // ako sodrzi ime ILI prezime
                }
                
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

        public async Task<IActionResult> SCourses(long? id)
        {
            IQueryable<Course> courses = _context.Course.Include(f => f.FirstTeacher).Include(s => s.SecondTeacher).AsQueryable();
            IQueryable<Enrollment> enrollments = _context.Enrollment.AsQueryable();

            enrollments = enrollments.Where(s => s.StudentId == id);
            IEnumerable<int> enrollmentsCoursesId = enrollments.Select(m => m.CourseId).Distinct();
            courses = courses.Where(m => enrollmentsCoursesId.Contains(m.Id));
            courses = courses.Include(m => m.Students).ThenInclude(m => m.Student);
            ViewData["StudentFullName"] = _context.Student.Where(t => t.Id == id).Select(t => t.FullName).FirstOrDefault();
            ViewData["StudentId"] = id;
            return View(courses);
        }
    }
}
