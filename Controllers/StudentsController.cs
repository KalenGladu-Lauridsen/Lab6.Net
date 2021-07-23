using Lab6Cs.Data;
using Lab6Cs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab6Cs.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly StudentDbContext _context;

        public StudentsController(StudentDbContext context)
        {
            _context = context;
        }

        // GET /Students
        /// <summary>
        /// Get collection of Students.
        /// </summary>
        /// <returns>A collection of Students</returns>
        /// <response code="200">Returns a collection of Students</response>
        /// <response code="500">Internal error</response>      
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Student>>> Get()
        {
            return Ok(await _context.Students.ToListAsync());
        }

        // POST /Students
        /// <summary>
        /// Creates a Student.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Students
        ///     {
        ///        "FirstName": "fname",
        ///        "LastName": "lname",
        ///        "Program": "program"
        ///     }
        ///
        /// </remarks>
        /// <returns>A newly created Student</returns>
        /// <response code="201">Returns the newly created Student</response>
        /// <response code="400">If the Student is malformed</response>      
        /// <response code="500">Internal error</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Student>> CreateAsync([Bind("FirstName,LastName,Program")] StudentBase studentBase)
        {
            Student student = new Student
            {
                FirstName = studentBase.FirstName,
                LastName = studentBase.LastName,
                Program = studentBase.Program
            };

            _context.Add(student);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = student.ID }, student);
        }

        // GET /Students/{id}
        /// <summary>
        /// Get a Student.
        /// </summary>
        /// <param id="id"></param>
        /// <returns>A Car</returns>
        /// <response code="201">Returns a collection of Students</response>
        /// <response code="400">If the id is malformed</response>      
        /// <response code="404">If the Student is null</response>      
        /// <response code="500">Internal error</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Student>> GetById(Guid id)
        {
            Student student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }

        // PUT /Students/{id}
        /// <summary>
        /// Upserts a Student.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /Students
        ///     {
        ///        "FirstName": "fname",
        ///        "LastName": "lname",
        ///        "Program": "program"
        ///     }
        ///
        /// </remarks>
        /// <param id="id"></param>
        /// <returns>An upserted Student</returns>
        /// <response code="200">Returns the updated Student</response>
        /// <response code="201">Returns the newly created Student</response>
        /// <response code="400">If the Student or id is malformed</response>      
        /// <response code="500">Internal error</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Student>> Upsert(Guid id, [Bind("FirstName,LastName,Program")] StudentBase studentBase)
        {
            Student student = new Student
            {
                FirstName = studentBase.FirstName,
                LastName = studentBase.LastName,
                Program = studentBase.Program
            };

            if (!StudentExists(id))
            {
                student.ID = id;
                _context.Add(student);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetById), new { id = student.ID }, student);
            }

            Student dbStudent = await _context.Students.FindAsync(id);
            dbStudent.FirstName = student.FirstName;
            dbStudent.LastName = student.LastName;
            dbStudent.Program = student.Program;

            _context.Update(dbStudent);
            await _context.SaveChangesAsync();

            return Ok(dbStudent);
        }

        // DELETE /Students/{id}
        /// <summary>
        /// Deletes a Student.
        /// </summary>
        /// <param id="id"></param>
        /// <response code="202">Student is deleted</response>
        /// <response code="400">If the id is malformed</response>      
        /// <response code="500">Internal error</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var car = await _context.Students.FindAsync(id);
            _context.Students.Remove(car);
            await _context.SaveChangesAsync();
            return Accepted();
        }

        private bool StudentExists(Guid id)
        {
            return _context.Students.Any(e => e.ID == id);
        }
    }
}
