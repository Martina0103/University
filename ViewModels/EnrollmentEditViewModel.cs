using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University.Models;

namespace University.ViewModels
{
    public class EnrollmentEditViewModel
    {
        public Course Course { get; set; }
        public IEnumerable<int> SelectedStudent { get; set; }
        //public IEnumerable<SelectListItems> StudentList { get; set; }
    }
}
