namespace PSUCourse.API.Dtos
{
    /// <summary>
    /// Model used to move course data between layers.
    /// </summary>
    public class CourseProfessorDto
    {
        /// <summary>
        /// Unique identified for the course
        /// </summary>
        public int ID { get; set;}
        /// <summary>
        /// Course name (duplicates not allowed)
        /// </summary>
        public string CourseName { get; set;}
        /// <summary>
        /// Professor id for the professor (optional)
        /// </summary>
        public int? ProfessorID { get; set; }
        /// <summary>
        /// Professor name
        /// </summary>
        public string ProfessorName { get; set;}
        /// <summary>
        /// Professor email
        /// </summary>
        public string ProfessorEmail { get; set; }
        /// <summary>
        /// Room number (optional)
        /// </summary>
        public int? RoomNumber { get; set;}
        /// <summary>
        /// Days of the week. Each day is represented by a boolean flag. If set to true, the course runs on
        /// the given day.
        /// </summary>
        public bool Sunday { get; set;}
        public bool Monday { get; set;}
        public bool Tuesday { get; set;}
        public bool Wednesday { get; set;}
        public bool Thursday { get; set;}
        public bool Friday { get; set;}
        public bool Saturday { get; set;}
    }
}