using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University.Models;

namespace University.ViewModels
{
    public class EnrollmentSearchView
    {
        public IEnumerable<Enrollment> Enrollments { get; set; } 
        public SelectList Indexes { get; set; } //prebaruvame po  Index
        public string EnrollmentIndex { get; set; } //int vrednost barame
        /*public SelectList Years { get; set; } //za programa
        public int EnrollmentYear { get; set; } //string vrednost barame od Modelot*/
        public string SearchList { get; set; } //prebaruvame po kursevi


    }
}
