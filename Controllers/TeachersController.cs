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
    public class TeachersController : Controller
    {
        private readonly UniversityContext _context;

        public TeachersController(UniversityContext context)
        {
            _context = context;
        }

        // GET: Teachers
        public async Task<IActionResult> Index(string TeacherDegree, string TeacherAcademicRank, string SearchName, string SearchLast)
        {
            // return View(await _context.Teacher.ToListAsync());

            // Dodadeno za da gi pokazuva predmetite vo Teachers Controller-ot ***
            /*var universityContext = _context.Teacher.Include(n => n.FirstCourses).ThenInclude(n => n.FirstTeacher)
                .Include(m => m.SecondCourses).ThenInclude(m => m.SecondTeacher); //dodadeno e */
            //******
            IQueryable<Teacher> teachers  = _context.Teacher.AsQueryable();
            IQueryable<string> AcademicRankQuery = (IQueryable<string>)_context.Teacher.OrderBy(m => m.AcademicRank).Select(m => m.AcademicRank).Distinct(); // dodadov (IQueryable<string>) inaku mi javuva greshka
            IQueryable<string> DegreeQuery = (IQueryable<string>)_context.Teacher.OrderBy(m => m.Degree).Select(m => m.Degree).Distinct(); // dodadov (IQueryable<string>) inaku mi javuva greshka

            if (!string.IsNullOrEmpty(SearchName))
            {
                teachers = teachers.Where(s => s.FirstName.Contains(SearchName)); // ako go sodr\i soodvetniot naslov
            }
            if (!string.IsNullOrEmpty(SearchLast))
            {
                teachers = teachers.Where(s => s.LastName.Contains(SearchLast)); // ako go sodr\i soodvetniot naslov
            }
            if (!string.IsNullOrEmpty(TeacherDegree))
            {
                teachers = teachers.Where(s => s.Degree == TeacherDegree);
            }
            if (!string.IsNullOrEmpty(TeacherAcademicRank))
            {
                teachers = teachers.Where(s => s.AcademicRank == TeacherAcademicRank);
            }

            teachers = teachers.Include(n => n.FirstCourses).ThenInclude(n => n.FirstTeacher)
                .Include(m => m.SecondCourses).ThenInclude(m => m.SecondTeacher);

            var TeacherVM = new TeacherSearchViewModel
            {
                Degrees = new SelectList(DegreeQuery.AsEnumerable()),
                AcademicRanks = new SelectList(AcademicRankQuery.AsEnumerable()),
                Teachers = teachers.AsEnumerable()

            };

            return View(TeacherVM);
            
        }

        // GET: Teachers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Dodadeno za da gi pokazuva predmetite vo Teachers Controller-ot ***
            var teacher = await _context.Teacher
               .Include(n => n.FirstCourses).ThenInclude(n => n.FirstTeacher) 
               .Include(n => n.SecondCourses).ThenInclude(n => n.SecondTeacher)
               .FirstOrDefaultAsync(m => m.Id == id);
            //*******

            /*var teacher = await _context.Teacher
                .FirstOrDefaultAsync(m => m.Id == id);*/

            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // GET: Teachers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Teachers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Degree,AcademicRank,OfficeNumber,HireDate")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                _context.Add(teacher);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(teacher);
        }

        // GET: Teachers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teacher.FindAsync(id);
            if (teacher == null)
            {
                return NotFound();
            }
            return View(teacher);
        }

        // POST: Teachers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Degree,AcademicRank,OfficeNumber,HireDate")] Teacher teacher)
        {
            if (id != teacher.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teacher);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeacherExists(teacher.Id))
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
            return View(teacher);
        }

        // GET: Teachers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Dodadeno za da gi pokazuva predmetite vo Teachers Controller-ot ***
            var teacher = await _context.Teacher
               .Include(n => n.FirstCourses).ThenInclude(n => n.FirstTeacher) 
               .Include(n => n.SecondCourses).ThenInclude(n => n.SecondTeacher)
               .FirstOrDefaultAsync(m => m.Id == id);
            //*********

            /*var teacher = await _context.Teacher
                .FirstOrDefaultAsync(m => m.Id == id);*/

            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teacher = await _context.Teacher.FindAsync(id);
            _context.Teacher.Remove(teacher);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeacherExists(int id)
        {
            return _context.Teacher.Any(e => e.Id == id);
        }
    }
}
