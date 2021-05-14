using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University.Models;

namespace University.ViewModels
{
    public class TeacherSearchViewModel
    {
        public IEnumerable<Teacher> Teachers { get; set; }
        public SelectList Degrees { get; set; } //za stepen na obrazovanie
        public string TeacherDegree { get; set; } //string vrednost barame
        public SelectList AcademicRanks { get; set; } //za akademski rank
        public string TeacherAcademicRank { get; set; } //string vrednost barame od Modelot
        public string SearchList { get; set; } // za prebaruvanje FULL NAME
        
    }
}

