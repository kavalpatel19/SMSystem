using Microsoft.AspNetCore.Mvc;
using SMSystem_Api.Helpers;
using SMSystem_Api.Model.Students;
using SMSystem_Api.Model.Teachers;
using SMSystem_Api.Repository.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SMSystem_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherApiController : ControllerBase
    {
        private readonly ITeacherApiRepository teachRepo;

        public TeacherApiController(ITeacherApiRepository TeachRepo)
        {
            teachRepo = TeachRepo;
        }

        // GET: api/<TeacherApiController>
        [HttpGet]
        public async Task<IActionResult> GetAll(string? sid, string? name, string? phone, int pageIndex)
        {
            SearchingPara para = new SearchingPara()
            {
                SId = sid,
                Name = name,
                Phone = phone,
                PageIndex = pageIndex
            };
            PaggedTeacherModel teachers = await teachRepo.GetAll(para);
            return Ok(teachers);
        }

        // GET api/<TeacherApiController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            TeacherModel teacher = await teachRepo.Get(id);
            return Ok(teacher);
        }

        [HttpGet("Export")]
        public IActionResult GetAllTeachers()
        {
            List<TeacherModel> data = teachRepo.GetAllTeachers();
            return Ok(data);
        }

        // POST api/<TeacherApiController>
        [HttpPost]
        public async Task<IActionResult> Post(TeacherModel teacher)
        {
            teachRepo.Add(teacher);
            return Ok();
        }

        // PUT api/<TeacherApiController>/5
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] TeacherModel teacher)
        {
            teachRepo.Update(teacher);
            return Ok();
        }

        // DELETE api/<TeacherApiController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            teachRepo.Delete(id);
            return Ok();
        }
    }
}
