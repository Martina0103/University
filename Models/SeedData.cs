using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using University.Data;
using University.Models;

namespace University.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new UniversityContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<UniversityContext>>()))
            {
                if (context.Student.Any() || context.Teacher.Any() || context.Course.Any())
                {
                    return; // DB has been seeded
                }

                context.Teacher.AddRange(
                    new Teacher { /*Id=1, */ FirstName = "Pero", LastName = "Latkoski", Degree = "PhD", AcademicRank = "Professor" },
                    new Teacher { /*Id=2, */ FirstName = "Daniel", LastName = "Denkovski", Degree = "PhD", AcademicRank = "Asst. Prof" },
                    new Teacher { /*Id=3, */ FirstName = "Danijela", LastName = "Efnusheva", Degree = "PhD", AcademicRank = "Professor" },
                    new Teacher { /*Id=4, */ FirstName = "Ana", LastName = "Colakoska", Degree = "MSc", AcademicRank = "Assistant" },
                    new Teacher { /*Id=5, */ FirstName = "Marija", LastName = "Kalendar", Degree = "PhD", AcademicRank = "Professor" },
                    new Teacher { /*Id=6, */ FirstName = "Emilija", LastName = "Kizevska", Degree = "MSc", AcademicRank = "Assistant" }

                );

                context.SaveChanges();

                context.Student.AddRange(
                    new Student { /*Id=3, */ StudentId = "193/2017", FirstName = "Martina", LastName = "Markovska", EnrollmentDate = DateTime.Parse("2017-09-15"), AcquiredCredits = 195, CurrentSemester = 8, EducationLevel = "Student" },
                    new Student { /*Id=2, */ StudentId = "184/2017", FirstName = "Elena", LastName = "Indovska", EnrollmentDate = DateTime.Parse("2017-09-15"), AcquiredCredits = 162, CurrentSemester = 8, EducationLevel = "Student" },
                    new Student { /*Id=1, */ StudentId = "171/2017", FirstName = "Darko", LastName = "Angelovski", EnrollmentDate = DateTime.Parse("2017-09-15"), AcquiredCredits = 50, CurrentSemester = 4, EducationLevel = "Student" }
                );

                context.SaveChanges();

                context.Course.AddRange(
                    new Course
                    {
                        /*Id=3, */
                        Title = "Razvoj na serverski WEB aplikacii",
                        Credits = 6,
                        Semester = 6,
                        Programme = "KTI/ TKII",
                        EducationLevel = "Undergraduate",
                        FirstTeacherId = context.Teacher.Single(t => t.FirstName == "Pero" && t.LastName == "Latkoski").Id,
                        SecondTeacherId = context.Teacher.Single(t => t.FirstName == "Daniel" && t.LastName == "Denkovski").Id
                    },

                    new Course
                    {
                        /*Id=2, */
                        Title = "Mrezni standardi i uredi",
                        Credits = 6,
                        Semester = 7,
                        Programme = "KTI",
                        EducationLevel = "Undergraduate",
                        FirstTeacherId = context.Teacher.Single(t => t.FirstName == "Danijela" && t.LastName == "Efnusheva").Id,
                        SecondTeacherId = context.Teacher.Single(t => t.FirstName == "Ana" && t.LastName == "Colakoska").Id
                    },
                    new Course
                    {
                        /*Id=1, */
                        Title = "Distribuirani sistemi",
                        Credits = 6,
                        Semester = 8,
                        Programme = "KTI",
                        EducationLevel = "Undergraduate",
                        FirstTeacherId = context.Teacher.Single(t => t.FirstName == "Marija" && t.LastName == "Kalendar").Id,
                        SecondTeacherId = context.Teacher.Single(t => t.FirstName == "Emilija" && t.LastName == "Kizevska").Id
                    }

                );

                context.SaveChanges();

                context.Enrollment.AddRange(
                    new Enrollment { CourseId = 2, StudentId = 3, Semester = "7" }, //MSU i Martina
                    new Enrollment { CourseId = 1, StudentId = 3, Semester = "6" }, //Ds i Martina
                    new Enrollment { CourseId = 3, StudentId = 3, Semester = "8" }, //RSWEB i Martina
                    new Enrollment { CourseId = 1, StudentId = 2, Semester = "6" }, //Ds i Elena
                    new Enrollment { CourseId = 1, StudentId = 1, Semester = "8" }, //Ds i Darko
                    new Enrollment { CourseId = 3, StudentId = 2, Semester = "8" } //RSWEB i Elena
                    
                );

                context.SaveChanges();

            }
        }
    }
}
