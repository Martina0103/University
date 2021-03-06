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
        public async Task<IActionResult> Index(string TeacherDegree, string TeacherAcademicRank, string SearchList )
        {
            IQueryable<Teacher> teachers  = _context.Teacher.AsQueryable();
            IQueryable<string> AcademicRankQuery = _context.Teacher.OrderBy(m => m.AcademicRank).Select(m => m.AcademicRank).Distinct(); 
            IQueryable<string> DegreeQuery = _context.Teacher.OrderBy(m => m.Degree).Select(m => m.Degree).Distinct(); 

            if (!string.IsNullOrEmpty(SearchList))
            {
                string[] full = SearchList.Split(' ');
                if(full.Length == 2)
                {
                    var ime = full[0];
                    var prezime = full[1];
                    teachers = teachers.Where(s => s.FirstName.Contains(ime) && s.LastName.Contains(prezime)); // ako sme vnele ime I prezime za prebaruvanje
                }
                else
                {
                    teachers = teachers.Where(s => s.FirstName.Contains(SearchList) || s.LastName.Contains(SearchList)); // ako sme vnele ime ILI prezime za prebaruvanje

                }
                
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
                Degrees = new SelectList(await DegreeQuery.ToListAsync()),
                AcademicRanks = new SelectList(await AcademicRankQuery.ToListAsync()),
                Teachers = await teachers.ToListAsync()

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
        public async Task<IActionResult> TCourses(long? id)
        {
            var courses = _context.Course.Where(m => m.FirstTeacherId == id || m.SecondTeacherId == id);
            courses = courses.Include(m => m.FirstTeacher).Include(m => m.SecondTeacher);
            ViewData["TeacherFullName"] = _context.Teacher.Where(t => t.Id == id).Select(t => t.FullName).FirstOrDefault();
            ViewData["TeacherId"] = id;
            ViewData["TeacherAcademicRank"] = _context.Teacher.Where(t => t.Id == id).Select(t => t.AcademicRank).FirstOrDefault();
            return View(courses);
        }
    }
}
