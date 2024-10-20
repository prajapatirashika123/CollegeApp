using CollegeApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace CollegeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        [HttpGet]
        [Route("All", Name = "GetAllStudents")]
        public ActionResult<IEnumerable<Student>> GetStudents()
        {
            //Ok - 200 - success
            return Ok(CollegeRepository.Students);
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Student> GetStudentById(int id)
        {
            //BadRequest - 400 - BadRequest - Client error
            if (id <= 0)
            {
                return BadRequest();
            }
            var student = CollegeRepository.Students.Where(x => x.Id == id).FirstOrDefault();
            //NotFound - 400 - NotFound - Client error
            if (student == null)
            {
                return NotFound($"The student with id {id} not found");
            }
            //Ok - 200 - success
            return Ok(student);
        }

        [HttpGet("{name:alpha}", Name = "GetStudentByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Student> GetStudentByName(string name)
        {
            //BadRequest - 400 - BadRequest - Client error
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest();
            }
            var student = CollegeRepository.Students.Where(x => x.StudentName == name).FirstOrDefault();
            //NotFound - 400 - NotFound - Client error
            if (student == null)
            {
                return NotFound($"The student with name {name} not found");
            }
            //Ok - 200 - success
            return Ok(student);
        }

        [HttpDelete("{id}", Name = "DeleteStudentById")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public ActionResult<bool> DeleteStudentBId(int id)
        {
            //BadRequest - 400 - BadRequest - Client error
            if (id <= 0)
            {
                return BadRequest();
            }
            var student = CollegeRepository.Students.Where(x => x.Id == id).First();
            //NotFound - 400 - NotFound - Client error
            if (student == null)
            {
                return NotFound($"The student with id {id} not found");
            }
            CollegeRepository.Students.Remove(student);
            //Ok - 200 - success
            return Ok(true);
        }
    }
}