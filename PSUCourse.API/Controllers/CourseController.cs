using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

using PSUCourse.API.Dtos;
using PSUCourse.API.Models;

namespace PSUCourse.API.Controllers
{
    /// <summary>
    /// Controller used by front-end to access the Web API methods for the course API
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly PendulumStateContext _context = new PendulumStateContext();
        private readonly IMapper _mapper;

        public CourseController(IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Method used to scrub the string and remove trailing spaces
        /// </summary>
        /// <param name="course"></param>
        private void CleanInput(CourseProfessorDto course)
        {
            course.CourseName = course.CourseName.Trim();
            course.ProfessorEmail = course.ProfessorEmail == null ? null : course.ProfessorEmail.Trim();
            course.ProfessorName = course.ProfessorName == null ? null : course.ProfessorName.Trim();
        }

        /// <summary>
        /// Method used to update 
        /// </summary>
        /// <param name="course">The courseProfessor object that the user is adding/updating</param>
        /// <param name="contextCourse">The EF model for the course that will be saved to the database.</param>
        private void AddUpdateProfessor(CourseProfessorDto course, Course contextCourse)
        {
            // If we don't have a professor, don't worry about doing anything with the professor.
            if (course.ProfessorName != null && !course.ProfessorName.Equals(string.Empty))
            {
                // Do we already have this professor?
                var professorToUse = _context.Professor.Where(p => p.ProfessorName.ToUpper() == course.ProfessorName.ToUpper()).FirstOrDefault();
                if (professorToUse != null)
                {
                    // If the professor already eixsts, use the id when creating the course
                    contextCourse.ProfessorId = professorToUse.Id;

                    // If the professor's email changed, update it as well
                    if ((string.IsNullOrEmpty(professorToUse.Email) != string.IsNullOrEmpty(course.ProfessorEmail))
                        || !professorToUse.Email.ToUpper().Equals(course.ProfessorEmail.ToUpper()))
                    {
                        // Prepare the context to be updated
                        professorToUse.Email = course.ProfessorEmail;
                        _context.Update(professorToUse);
                        // Save the course changes to the database
                        _context.SaveChanges();
                    }
                }
                else
                {
                    // If we don't have the professor, we will need to add it to the database.
                    var professorToAdd = _mapper.Map<Professor>(course);
                    _context.Professor.Add(professorToAdd);
                    _context.SaveChanges();
                    contextCourse.ProfessorId = professorToAdd.Id;
                }
            }
        }

        /// <summary>
        /// Returns the list of courses. There is no search filter on the request.
        /// GET: http://domain/api/course
        /// </summary>
        /// <returns>Returns a list of courses</returns>
        [HttpGet]
        public async Task<IActionResult> GetCourses()
        {
            // Get all the courses in the database and return the associated professors
            var courses = await _context.Course.Include(p => p.Professor).ToListAsync();

            // Map the EF entity model to the Dto model.
            var coursesToReturn = _mapper.Map<IEnumerable<CourseProfessorDto>>(courses);

            return Ok(coursesToReturn);
        }

        /// <summary>
        /// Returns the course for a given course id. If the course id is invalid, a NotFound error will be returned.
        /// GET: http://domain/api/course/1
        /// </summary>
        /// <param name="id">The course id to return</param>
        /// <returns>Returns the course for the given id if found and the appropriate status code based on the result of the query.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourse(int id)
        {
            // Get the course that matches the course id provided. Include the professor object in the query results
            var course = await _context.Course.Where(c => c.Id == id).Include(p => p.Professor).FirstOrDefaultAsync();
            
            // If we found a course, map to the Dto model and return it
            if (course != null)
            {
                // Map the EF entity model to the Dto model.
                var courseToReturn = _mapper.Map<CourseProfessorDto>(course);
                return Ok(courseToReturn);
            }
            else
            {
                // If no matching course was found, return a not found status code.
                return NotFound();
            }
        }

        /// <summary>
        /// Method used to add a course to the database.
        /// POST: http://domain/api/course
        /// </summary>
        /// <param name="course">Course object that should be added</param>
        /// <returns>Returns the appropriate status based on the results of the add</returns>
        [HttpPost]
        public async Task<IActionResult> AddCourse(CourseProfessorDto course)
        {
            try
            {
                // Clean the input strings
                CleanInput(course);

                // Check if the course already exists in the database
                if (_context.Course.Where(c => c.CourseName.Trim().ToUpper() == course.CourseName.Trim().ToUpper()).Any())
                {
                    // Return an error if we already found a course with the same name
                    return Conflict("Course already exists");
                }

                // Prep the course object
                var courseToAdd = _mapper.Map<Course>(course);

                AddUpdateProfessor(course, courseToAdd);

                // Add the new course
                _context.Add(courseToAdd);

                // Save the changes to the database
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch
            {
                return BadRequest("Failed to add the course");
            }
        }

        /// <summary>
        /// Method used to update the course
        /// PUT: http://domain/api/course
        /// </summary>
        /// <param name="course">Object that contains the updates to the course</param>
        /// <returns>Returns the appropriate status based on the results of the update</returns>
        [HttpPut]
        public async Task<IActionResult> UpdateCourse(CourseProfessorDto course)
        {
            try
            {
                // Clean the input strings
                CleanInput(course);

                // See if we can find the course in the database
                var origCourse = _context.Course.Find(course.ID);

                // If not found return an error
                if (origCourse == null)
                {
                    return NotFound();
                }

                // Prep the course object
                var courseToUpdate = _mapper.Map<Course>(course);

                AddUpdateProfessor(course, courseToUpdate);

                // Prepare the context to be updated
                _context.Entry(origCourse).CurrentValues.SetValues(courseToUpdate);

                // Save the course changes to the database
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch 
            {
                return BadRequest("Failed to update the course");
            }
        }

        /// <summary>
        /// Method used to delete a course
        /// DELETE: http://domain/api/course/1
        /// </summary>
        /// <param name="id">The course id to be removed</param>
        /// <returns>Returns the appropriate status based on the results of the delete</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            try
            {
                // Validate if the course exists
                if (!_context.Course.Any(x => x.Id == id))
                {
                    // If not found, return the appropriate status
                    return NotFound();
                }

                // Remove the course
                _context.Remove(_context.Course.Where(x => x.Id == id).FirstOrDefault());

                // Save the changes
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch
            {
                return BadRequest("Failed to delete the course");
            }
        }
    }
}