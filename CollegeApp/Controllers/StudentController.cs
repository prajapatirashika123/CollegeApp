using CollegeApp.Data;
using CollegeApp.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<ActionResult<StudentDTO>> CreateStudent([FromBody] StudentDTO model)
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
            await _dBContext.Students.AddAsync(student);
            await _dBContext.SaveChangesAsync();
            model.Id = student.Id;
            return CreatedAtRoute("GetStudentById", new { id = model.Id }, model);
        }

        [HttpDelete("{id}", Name = "DeleteStudentById")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<bool>> DeleteStudentById(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            var student = await _dBContext.Students.Where(x => x.Id == id).FirstAsync();
            if (student == null)
            {
                return NotFound($"The student with id {id} not found");
            }
            _dBContext.Students.Remove(student);
            _dBContext.SaveChangesAsync();
            return Ok(true);
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<StudentDTO>> GetStudentById(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            var student = await _dBContext.Students.Where(x => x.Id == id).FirstOrDefaultAsync();
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
            return Ok(studentDTO);
        }

        [HttpGet("{name:alpha}", Name = "GetStudentByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<StudentDTO>> GetStudentByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest();
            }
            var student = await _dBContext.Students.Where(x => x.StudentName == name).FirstOrDefaultAsync();
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
            return Ok(studentDTO);
        }

        [HttpGet]
        [Route("All", Name = "GetAllStudents")]
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetStudents()
        {
            var students = await _dBContext.Students.ToListAsync();
            return Ok(students);
        }

        [HttpPatch]
        [Route("{id:int}/UpdatePartial")]
        //api/student/1/updatePartial
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdatePartialStudent(int id, [FromBody] JsonPatchDocument<StudentDTO> patchDocument)
        {
            if (patchDocument == null || id <= 0)
            {
                BadRequest();
            }
            var existingStudent = await _dBContext.Students.Where(x => x.Id == id).FirstOrDefaultAsync();
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
            await _dBContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut]
        [Route("Update")]
        //api/student/update
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateStudent([FromBody] StudentDTO model)
        {
            if (model == null || model.Id == 0)
            {
                BadRequest();
            }
            var existingStudent = await _dBContext.Students.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
            if (existingStudent == null)
            {
                return NotFound();
            }
            existingStudent.Address = model.Address;
            existingStudent.DOB = model.DOB;
            existingStudent.Email = model.Email;
            existingStudent.StudentName = model.StudentName;
            await _dBContext.SaveChangesAsync();
            return NoContent();
        }
    }
}