using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University.Models;

namespace University.ViewModels
{
    public class StudentSearchViewModel
    {
        public IEnumerable<Student> Students { get; set; }
        public SelectList StudentIds { get; set; } //za indeks na studentot
        public string StudentStudentId { get; set; } //string vrednost barame
        public string SearchName { get; set; } //za imeto (FirstName)
        public string SearchLast { get; set; } // za prezimeto
       /* public string Full  
        {
            get { return String.Format("{0} {1}", SearchName, SearchLast); }
        }*/
    }
}
