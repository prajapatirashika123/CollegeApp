using CollegeApp.Data;
using CollegeApp.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CollegeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly CollegeDBContext _dBContext;

        public StudentController(CollegeDBContext dBContext)
        {
            _dBContext = dBContext;
        }

        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<StudentDTO> CreateStudent([FromBody] StudentDTO model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            Student student = new Student()
            {
                Address = model.Address,
                DOB = model.DOB,
                Email = model.Email,
                StudentName = model.StudentName,
            };
            _dBContext.Students.Add(student);
            _dBContext.SaveChanges();
            model.Id = student.Id;
            //Status - 201
            ///api/Student/3
            //New student details
            return CreatedAtRoute("GetStudentById", new { id = model.Id }, model);
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
            var student = _dBContext.Students.Where(x => x.Id == id).First();
            //NotFound - 400 - NotFound - Client error
            if (student == null)
            {
                return NotFound($"The student with id {id} not found");
            }
            _dBContext.Students.Remove(student);
            _dBContext.SaveChanges();
            //Ok - 200 - success
            return Ok(true);
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<StudentDTO> GetStudentById(int id)
        {
            //BadRequest - 400 - BadRequest - Client error
            if (id <= 0)
            {
                return BadRequest();
            }
            var student = _dBContext.Students.Where(x => x.Id == id).FirstOrDefault();
            //NotFound - 400 - NotFound - Client error
            if (student == null)
            {
                return NotFound($"The student with id {id} not found");
            }
            var studentDTO = new StudentDTO
            {
                Address = student.Address,
                DOB = student.DOB,
                Email = student.Email,
                Id = student.Id,
                StudentName = student.StudentName,
            };
            //Ok - 200 - success
            return Ok(studentDTO);
        }

        [HttpGet("{name:alpha}", Name = "GetStudentByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<StudentDTO> GetStudentByName(string name)
        {
            //BadRequest - 400 - BadRequest - Client error
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest();
            }
            var student = _dBContext.Students.Where(x => x.StudentName == name).FirstOrDefault();
            //NotFound - 400 - NotFound - Client error
            if (student == null)
            {
                return NotFound($"The student with name {name} not found");
            }
            var studentDTO = new StudentDTO
            {
                Address = student.Address,
                DOB = student.DOB,
                Email = student.Email,
                Id = student.Id,
                StudentName = student.StudentName,
            };
            //Ok - 200 - success
            return Ok(studentDTO);
        }

        [HttpGet]
        [Route("All", Name = "GetAllStudents")]
        public ActionResult<IEnumerable<StudentDTO>> GetStudents()
        {
            var students = _dBContext.Students.Select(s => new StudentDTO()
            {
                Address = s.Address,
                DOB = s.DOB,
                Email = s.Email,
                Id = s.Id,
                StudentName = s.StudentName,
            });
            //Ok - 200 - success
            return Ok(students);
        }

        [HttpPatch]
        [Route("{id:int}/UpdatePartial")]
        //api/student/1/updatePartial
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult UpdatePartialStudent(int id, [FromBody] JsonPatchDocument<StudentDTO> patchDocument)
        {
            if (patchDocument == null || id <= 0)
            {
                BadRequest();
            }
            var existingStudent = _dBContext.Students.Where(x => x.Id == id).FirstOrDefault();
            if (existingStudent == null)
            {
                return NotFound();
            }
            var studentDTO = new StudentDTO
            {
                Address = existingStudent.Address,
                DOB = existingStudent.DOB,
                Email = existingStudent.Email,
                Id = existingStudent.Id,
                StudentName = existingStudent.StudentName,
            };
            patchDocument.ApplyTo(studentDTO, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            existingStudent.Address = studentDTO.Address;
            existingStudent.DOB = studentDTO.DOB;
            existingStudent.Email = studentDTO.Email;
            existingStudent.StudentName = studentDTO.StudentName;
            _dBContext.SaveChanges();
            //204- NoContent
            return NoContent();
        }

        [HttpPut]
        [Route("Update")]
        //api/student/update
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult UpdateStudent([FromBody] StudentDTO model)
        {
            if (model == null || model.Id == 0)
            {
                BadRequest();
            }
            var existingStudent = _dBContext.Students.Where(x => x.Id == model.Id).FirstOrDefault();
            if (existingStudent == null)
            {
                return NotFound();
            }
            existingStudent.Address = model.Address;
            existingStudent.DOB = model.DOB;
            existingStudent.Email = model.Email;
            existingStudent.StudentName = model.StudentName;
            _dBContext.SaveChanges();
            return NoContent();
        }
    }
}