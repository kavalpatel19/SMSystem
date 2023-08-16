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
            var para = new SearchingPara()
            {
                SId = sid,
                Name = name,
                Phone = phone,
                PageIndex = pageIndex
            };
            var teachers = await teachRepo.GetAll(para).ConfigureAwait(false);
            return Ok(teachers);
        }

        // GET api/<TeacherApiController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var teacher = await teachRepo.Get(id).ConfigureAwait(false);
            return Ok(teacher);
        }

        [HttpGet("Export")]
        public IActionResult GetAllTeachers()
        {
            var data = teachRepo.GetAllTeachers();
            return Ok(data);
        }

        // POST api/<TeacherApiController>
        [HttpPost]
        public async Task<IActionResult> Post(TeacherModel teacher)
        {
            await teachRepo.Add(teacher).ConfigureAwait(false);
            return Ok();
        }

        // PUT api/<TeacherApiController>/5
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] TeacherModel teacher)
        {
            await teachRepo.Update(teacher).ConfigureAwait(false);
            return Ok();
        }

        // DELETE api/<TeacherApiController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await teachRepo.Delete(id).ConfigureAwait(false);
            return Ok();
        }
    }
}
