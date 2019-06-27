using System;
using System.Collections.Generic;

namespace PSUCourse.API.Models
{
    public partial class Professor
    {
        public Professor()
        {
            Course = new HashSet<Course>();
        }

        public int Id { get; set; }
        public string ProfessorName { get; set; }
        public string Email { get; set; }

        public virtual ICollection<Course> Course { get; set; }
    }
}
