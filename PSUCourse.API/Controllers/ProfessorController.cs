using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

using PSUCourse.API.Models;
using PSUCourse.API.Dtos;

namespace PSUCourse.API.Controllers
{
    /// <summary>
    /// Controller used by front-end to access the Web API methods for the professor API
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProfessorController : ControllerBase
    {
        private readonly PendulumStateContext _context = new PendulumStateContext();
        private readonly IMapper _mapper;

        public ProfessorController(IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Method used to get the list of professors
        /// GET: http://domain/api/professor
        /// </summary>
        /// <returns>Returns all the professors in the database</returns>
        [HttpGet]
        public async Task<IActionResult> GetProfessors()
        {
            var professors = await _context.Professor.ToListAsync();
            var professorsList = _mapper.Map<IEnumerable<ProfessorDto>>(professors);

            return Ok(professorsList);
        }
    }
}