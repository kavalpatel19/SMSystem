using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMSystem_Api.Helpers;
using SMSystem_Api.Model.Exam;
using SMSystem_Api.Model.Fees;
using SMSystem_Api.Repository.Interfaces;

namespace SMSystem_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamApiController : ControllerBase
    {
        private readonly IExamApiRepository ExamRepo;

        public ExamApiController(IExamApiRepository ExamRepo)
        {
            this.ExamRepo = ExamRepo;
        }

        // to Get all data searched/none searched
        [HttpGet]
        public async Task<IActionResult> GetAll(int pageIndex)
        {
            SearchingPara para = new SearchingPara()
            {
                PageIndex = pageIndex
            };
            PaggedExamModel exams = await ExamRepo.GetAll(para);
            return Ok(exams);
        }

        // GET api/<ExamApiController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            ExamModel exam = await ExamRepo.Get(id);
            return Ok(exam);
        }

        // POST api/<ExamApiController>/Export
        [HttpGet("Export")]
        public IActionResult GetAllFees()
        {
            List<ExamModel> data = ExamRepo.GetAllExams();
            return Ok(data);
        }

        // POST api/<ExamApiController>/model
        [HttpPost]
        public async Task<IActionResult> Post(ExamModel exam)
        {
            ExamRepo.Add(exam);
            return Ok();
        }

        // PUT api/<ExamApiController>/model
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] ExamModel exam)
        {
            ExamRepo.Update(exam);
            return Ok();
        }

        // DELETE api/<ExamApiController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            ExamRepo.Delete(id);
            return Ok();
        }
    }
}
