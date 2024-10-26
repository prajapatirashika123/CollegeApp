using Azure;
using CollegeApp.Models;
using CollegeApp.MyLogging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace CollegeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        public StudentController()
        {
        }

        [HttpGet]
        [Route("All", Name = "GetAllStudents")]
        public ActionResult<IEnumerable<StudentDTO>> GetStudents()
        {
            var students = CollegeRepository.Students.Select(s => new StudentDTO()
            {
                Id = s.Id,
                StudentName = s.StudentName,
                Address = s.Address,
                Email = s.Email,
            });
            //Ok - 200 - success
            return Ok(CollegeRepository.Students);
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<StudentDTO> GetStudentById(int id)
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
            var studentDTO = new StudentDTO
            {
                Id = student.Id,
                StudentName = student.StudentName,
                Email = student.Email,
                Address = student.Address,
            };
            //Ok - 200 - success
            return Ok(studentDTO);
        }

        [HttpGet("{name:alpha}", Name = "GetStudentByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<StudentDTO> GetStudentByName(string name)
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
            var studentDTO = new StudentDTO
            {
                Id = student.Id,
                StudentName = student.StudentName,
                Email = student.Email,
                Address = student.Address,
            };
            //Ok - 200 - success
            return Ok(studentDTO);
        }

        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<StudentDTO> CreateStudent([FromBody] StudentDTO model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            int newId = CollegeRepository.Students.LastOrDefault().Id + 1;
            Student student = new Student()
            {
                Id = newId,
                StudentName = model.StudentName,
                Address = model.Address,
                Email = model.Email,
            };
            CollegeRepository.Students.Add(student);
            model.Id = student.Id;
            //Status - 201
            ///api/Student/3
            //New student details
            return CreatedAtRoute("GetStudentById", new { id = model.Id }, model);
        }

        [HttpPut]
        [Route("Update")]
        //api/student/update
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult UpdateStudent([FromBody] StudentDTO model)
        {
            if (model == null || model.Id == 0)
            {
                BadRequest();
            }
            var existingStudent = CollegeRepository.Students.Where(x => x.Id == model.Id).FirstOrDefault();
            if (existingStudent == null)
            {
                return NotFound();
            }
            existingStudent.Address = model.Address;
            existingStudent.Email = model.Email;
            existingStudent.StudentName = model.StudentName;
            return NoContent();
        }

        [HttpPatch]
        [Route("{id:int}/UpdatePartial")]
        //api/student/1/updatePartial
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult UpdatePartialStudent(int id, [FromBody] JsonPatchDocument<StudentDTO> patchDocument)
        {
            if (patchDocument == null || id <= 0)
            {
                BadRequest();
            }
            var existingStudent = CollegeRepository.Students.Where(x => x.Id == id).FirstOrDefault();
            if (existingStudent == null)
            {
                return NotFound();
            }
            var studentDTO = new StudentDTO
            {
                Id = existingStudent.Id,
                Address = existingStudent.Address,
                Email = existingStudent.Email,
                StudentName = existingStudent.StudentName,
            };
            patchDocument.ApplyTo(studentDTO, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            existingStudent.Address = studentDTO.Address;
            existingStudent.Email = studentDTO.Email;
            existingStudent.StudentName = studentDTO.StudentName;
            //204- NoContent
            return NoContent();
        }

        [HttpDelete("{id}", Name = "DeleteStudentById")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public ActionResult<bool> DeleteStudentById(int id)
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