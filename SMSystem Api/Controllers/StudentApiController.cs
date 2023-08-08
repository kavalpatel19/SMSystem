 using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SMSystem_Api.Data;
using SMSystem_Api.Helpers;
using SMSystem_Api.Model.Students;
using SMSystem_Api.Repository;
using System.Data.Common;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SMSystem_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentApiController : ControllerBase
    {
        IStudentApiRepository StudentRepo;

        public StudentApiController(IStudentApiRepository repo) 
        {
            StudentRepo = repo;
        }

        // to Get all data searched/none searched
        [HttpGet]
        public async Task<IActionResult> GetAll(string? sid ,string? name , string? phone , int pageIndex)
        {
            SearchingPara para = new SearchingPara()
            {
                SId = sid,
                Name = name,
                Phone = phone,
                PageIndex = pageIndex
            };
            PaggedStudentModel students = await StudentRepo.GetAll(para);
            return Ok(students);
        }

        // GET api/<StudentApiController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            StudentModel student = await StudentRepo.Get(id);
            return Ok(student);
        }

        [HttpGet("Export")]
        public IActionResult GetAllStudents()
        {
            List<StudentModel> data= StudentRepo.GetAllStudents();
            return Ok(data);
        }

        // POST api/<StudentApiController>
        [HttpPost]
        public async Task<IActionResult> Post(StudentModel student)
        {
            StudentRepo.Add(student);
            return Ok();
        }

        // PUT api/<StudentApiController>/5
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] StudentModel student)
        {
            StudentRepo.Update(student);
            return Ok();
        }

        // DELETE api/<StudentApiController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            StudentRepo.Delete(id);
            return Ok();
        }
    }
}
