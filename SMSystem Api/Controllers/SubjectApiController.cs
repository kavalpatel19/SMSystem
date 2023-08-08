using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMSystem_Api.Helpers;
using SMSystem_Api.Model.Department;
using SMSystem_Api.Model.Subjects;
using SMSystem_Api.Repository;

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
            SearchingPara para = new SearchingPara()
            {
                SId = sid,
                Name = name,
                Class = clas,
                PageIndex = pageIndex
            };
            PaggedSubjectModel subject = await SubRepo.GetAll(para);
            return Ok(subject);
        }

        // GET api/<SubjectApiController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            SubjectModel subject = await SubRepo.Get(id);
            return Ok(subject);
        }

        [HttpGet("Export")]
        public IActionResult GetAllData()
        {
            List<SubjectModel> data = SubRepo.GetAllSubjects();
            return Ok(data);
        }

        // POST api/<SubjectApiController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SubjectModel subject)
        {
            SubRepo.Add(subject);
            return Ok();
        }

        // PUT api/<SubjectApiController>/5
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] SubjectModel subject)
        {
            SubRepo.Update(subject);
            return Ok();
        }

        // DELETE api/<SubjectApiController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            SubRepo.Delete(id);
            return Ok();
        }
    }
}
