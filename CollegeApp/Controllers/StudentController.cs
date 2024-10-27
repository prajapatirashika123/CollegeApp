using AutoMapper;
using CollegeApp.Data;
using CollegeApp.Data.Repository;
using CollegeApp.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CollegeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class StudentController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IStudentRepository _studentRepository;

        public StudentController(IMapper mapper, IStudentRepository studentRepository)
        {
            _mapper = mapper;
            _studentRepository = studentRepository;
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
            Student student = _mapper.Map<Student>(model);
            var studentAfterCreation = await _studentRepository.Create(student);
            model.Id = studentAfterCreation.Id;
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
            var student = await _studentRepository.GetById(student => student.Id == id);
            if (student == null)
            {
                return NotFound($"The student with id {id} not found");
            }
            await _studentRepository.Delete(student);
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
            var student = await _studentRepository.GetById(student => student.Id == id);
            if (student == null)
            {
                return NotFound($"The student with id {id} not found");
            }
            var studentDTO = _mapper.Map<StudentDTO>(student);
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
            var student = await _studentRepository.GetByName(student => student.StudentName.ToLower().Contains(name));
            if (student == null)
            {
                return NotFound($"The student with name {name} not found");
            }
            var studentDTO = _mapper.Map<StudentDTO>(student);
            return Ok(studentDTO);
        }

        [HttpGet]
        [Route("All", Name = "GetAllStudents")]
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetStudents()
        {
            var students = await _studentRepository.GetAll();
            var studentDTOData = _mapper.Map<List<StudentDTO>>(students);
            return Ok(studentDTOData);
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
            var existingStudent = await _studentRepository.GetById(student => student.Id == id, true);
            if (existingStudent == null)
            {
                return NotFound();
            }
            var studentDTO = _mapper.Map<StudentDTO>(existingStudent);
            patchDocument.ApplyTo(studentDTO, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            existingStudent = _mapper.Map<Student>(studentDTO);
            await _studentRepository.Update(existingStudent);
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
            var existingStudent = await _studentRepository.GetById(student => student.Id == model.Id, true);
            if (existingStudent == null)
            {
                return NotFound();
            }
            var newRecord = _mapper.Map<Student>(model);
            await _studentRepository.Update(newRecord);
            return NoContent();
        }
    }
}