using System;
using System.Collections.Generic;

namespace PSUCourse.API.Models
{
    public partial class Course
    {
        public int Id { get; set; }
        public string CourseName { get; set; }
        public int? ProfessorId { get; set; }
        public int? RoomNumber { get; set; }
        public bool? Sunday { get; set; }
        public bool? Monday { get; set; }
        public bool? Tuesday { get; set; }
        public bool? Wednesday { get; set; }
        public bool? Thursday { get; set; }
        public bool? Friday { get; set; }
        public bool? Saturday { get; set; }

        public virtual Professor Professor { get; set; }
    }
}
