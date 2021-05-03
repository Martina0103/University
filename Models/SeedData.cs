
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University.Data;

namespace University.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new UniversityContext(
            serviceProvider.GetRequiredService<DbContextOptions<UniversityContext>>()))
            {
                //Movie zamena so Course (so toa shto ima dva nadvoreshni klucevi za Teacher)
                // Actor zamena so Student
                //  Director zamena so Teacher (so toa shto ima dva nadvoreshni klucevi vo Course)
                // ActorMovie zamena so Enrollment
                // Look for any course.
                if (context.Course.Any() || context.Teacher.Any() || context.Student.Any())
                {
                    return; // DB has been seeded
                }

                // DODAVAME VO TABELATA Teacher
                context.Teacher.AddRange(
                    new Teacher { /*Id = 1, */FirstName = "Daniel", LastName = "Denkovski", Degree = "PhD", AcademicRank = "Asst. Professor", OfficeNumber = "+ 389 2 3099 177", HireDate = DateTime.Parse("2017-1-1") },
                    new Teacher { /*Id = 2, */FirstName = "Pero", LastName = "Latkovski", Degree = "PhD", AcademicRank = "Professor", OfficeNumber = "+389 2 3099 113", HireDate = DateTime.Parse("2004-1-1") },
                    new Teacher { /*Id = 3, */FirstName = "Danijela", LastName = "Efnusheva", Degree = "PhD", AcademicRank = "Asst. Professor", OfficeNumber = "+ 389 2 3099 177", HireDate = DateTime.Parse("2009-2-1") },
                    new Teacher { /*Id = 3, */FirstName = "Ana", LastName = "Colakoska", Degree = "M.Sc.", AcademicRank = "Assistant", OfficeNumber = "+ 389 2 3099 177", HireDate = DateTime.Parse("2017-9-1") }
                );
                context.SaveChanges();

                //dodavame vo tabelata Student
                context.Student.AddRange(
                    new Student { /*Id = 1, */StudentId = "193/2017", FirstName = "Martina", LastName = "Markovska", EnrollmentDate = DateTime.Parse("2017-9-15"), AcquiredCredits = 195, CurrentSemester = 8, EducationLevel = "student" },
                    new Student { /*Id = 2, */StudentId = "184/2017", FirstName = "Elena", LastName = "Indovska", EnrollmentDate = DateTime.Parse("2017-9-15"), CurrentSemester = 8, EducationLevel = "student" },
                    new Student { /*Id = 3, */StudentId = "171/2017", FirstName = "Darko", LastName = "Angelovski", EnrollmentDate = DateTime.Parse("2017-9-15"), CurrentSemester = 8, EducationLevel = "student" }
                );
                context.SaveChanges();

                // dodavame elementi vo Course
                context.Course.AddRange(
                    new Course {/*Id = 1, */Title = "Razvoj na serverski Web", Credits = 6, Semester = 6, Programme = "KTI/TKII", EducationLevel = "Undergraduate studies", FirstTeacherId = context.Teacher.Single(d => d.FirstName == "Pero" && d.LastName == "Latkovski").Id, SecondTeacherId = context.Teacher.Single(d => d.FirstName == "Daniel" && d.LastName == "Denkovski").Id },
                    new Course {/*Id = 2, */Title = "Mrezni standardi i uredi", Credits = 6, Semester = 7, Programme = "KTI", EducationLevel = "Undergraduate studies", FirstTeacherId = context.Teacher.Single(d => d.FirstName == "Danijela" && d.LastName == "Efnusheva").Id, SecondTeacherId = context.Teacher.Single(d => d.FirstName == "Ana" && d.LastName == "Colakoska").Id },
                    new Course {/*Id = 1, */Title = "Mobilni servisi os Android programiranje", Credits = 6, Semester = 8, Programme = "KTI/TKII", EducationLevel = "Undergraduate studies", FirstTeacherId = context.Teacher.Single(d => d.FirstName == "Pero" && d.LastName == "Latkovski").Id, SecondTeacherId = context.Teacher.Single(d => d.FirstName == "Daniel" && d.LastName == "Denkovski").Id }

                );
                context.SaveChanges();
                context.Enrollments.AddRange
                (
                    new Enrollment { CourseId = 1, StudentId = 1, Semester = "8", Year = 2021, SeminarUrl = "url//", ProjectUrl = "url//", /*ExamPoints =, */ /*SeminarPoints = , */ /*ProjectPoints = */ /*AdditionalPoints = */ /*FinishDate = */ },
                    new Enrollment { CourseId = 2, StudentId = 1, Semester = "7", Year = 2020, SeminarUrl = "url//", ProjectUrl = "url//", ExamPoints = 100, SeminarPoints = 100,  /*ProjectPoints = */ /*AdditionalPoints = */ FinishDate = DateTime.Parse("2021-01-01") },
                    new Enrollment { CourseId = 1, StudentId = 2, Semester = "8", Year = 2021, SeminarUrl = "url//", ProjectUrl = "url//", /*ExamPoints =, */ /*SeminarPoints = , */ /*ProjectPoints = */ /*AdditionalPoints = */ /*FinishDate = */ },
                    new Enrollment { CourseId = 3, StudentId = 3, Semester = "8", Year = 2021, SeminarUrl = "url//", ProjectUrl = "url//", /*ExamPoints =, */ /*SeminarPoints = , */ /*ProjectPoints = */ /*AdditionalPoints = */ /*FinishDate = */ }
                );
                context.SaveChanges();
            }
        }
    }
}
