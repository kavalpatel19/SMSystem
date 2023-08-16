using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMSystem_Api.Helpers;
using SMSystem_Api.Model.Department;
using SMSystem_Api.Model.Subjects;
using SMSystem_Api.Repository.Interfaces;

namespace SMSystem_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectApiController : ControllerBase
    {
        private readonly ISubjectApiRepository SubRepo;

        public SubjectApiController(ISubjectApiRepository SubRepo)
        {
            this.SubRepo = SubRepo;
        }

        // GET: api/<SubjectApiController>
        [HttpGet]
        public async Task<IActionResult> Get(string? sid, string? name, string? clas, int pageIndex)
        {
            var para = new SearchingPara()
            {
                SId = sid,
                Name = name,
                Class = clas,
                PageIndex = pageIndex
            };
            var subject = await SubRepo.GetAll(para).ConfigureAwait(false);
            return Ok(subject);
        }

        // GET api/<SubjectApiController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var subject = await SubRepo.Get(id).ConfigureAwait(false);
            return Ok(subject);
        }

        [HttpGet("Export")]
        public IActionResult GetAllData()
        {
            var data = SubRepo.GetAllSubjects();
            return Ok(data);
        }

        // POST api/<SubjectApiController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SubjectModel subject)
        {
            await SubRepo.Add(subject).ConfigureAwait(false);
            return Ok();
        }

        // PUT api/<SubjectApiController>/5
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] SubjectModel subject)
        {
            await SubRepo.Update(subject).ConfigureAwait(false);
            return Ok();
        }

        // DELETE api/<SubjectApiController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await SubRepo.Delete(id).ConfigureAwait(false);
            return Ok();
        }
    }
}
