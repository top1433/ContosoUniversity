using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.Models
{
    public enum Grade
    {
        A,B,C,D,E,F
    }
    public class EnRollment
    {
        public int EnrollmentID { get; set; }
        public int StudentID { get; set; }
        public int CourseID { get; set; }


        [DisplayFormat(NullDisplayText ="No Grade")]
        public Grade? Grade { get; set; }
        public Student Student { get; set; }
        public Course Course { get; set; }
    }
}
