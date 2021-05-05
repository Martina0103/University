using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University.Models;

namespace University.ViewModels
{
    public class CourseSearchViewModel
    {
        public IEnumerable<Course> Course { get; set; }
        public SelectList Semesters { get; set; } //za semestar
        public int CourseSemester { get; set; } //int vrednost barame
        public SelectList Programmes { get; set; } //za programa
        public string CourseProgramme { get; set; } //string vrednost barame od Modelot
        public string SearchList { get; set; } //za naslovot (title)


    }
}
