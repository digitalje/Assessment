using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSUCourse.API.Dtos
{
    /// <summary>
    /// Model used to move professor data between layers
    /// </summary>
    public class ProfessorDto
    {
        /// <summary>
        /// Unique identifier for the professor
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Professor name
        /// </summary>
        public string ProfessorName { get; set; }
        /// <summary>
        /// Professor email address
        /// </summary>
        public string Email { get; set; }
    }
}
