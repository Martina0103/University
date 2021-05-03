using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace University.Models
{
    public class Enrollment
    {
        public Int64 Id { get; set; }

        public int CourseId { get; set; }

        public Course Course { get; set; }

        public Int64 StudentId { get; set; }

        public Student Student { get; set; }

        [StringLength(10)]
        public string Semester { get; set; }

        public int? Year { get; set; }

        public int? Grade { get; set; }

        [Display(Name = "Seminar URL")]
        [StringLength(255)]
        public string SeminarUrl { get; set; }

        [Display(Name = "Project URL")]
        [StringLength(255)]
        public string ProjectUrl { get; set; }

        [Display(Name = "Exam Points")]
        public int? ExamPoints { get; set; }

        [Display(Name = "Seminar Points")]
        public int? SeminarPoints { get; set; }

        [Display(Name = "Project Points")]
        public int? ProjectPoints { get; set; }

        [Display(Name = "Additional Points")]
        public int? AdditionalPoints { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Finish Date")]
        public DateTime? FinishDate { get; set; }
    }
}
