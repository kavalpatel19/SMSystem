using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMSystem_Api.Helpers;
using SMSystem_Api.Model;
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
            var para = new SearchingPara()
            {
                PageIndex = pageIndex
            };
            var exams = await ExamRepo.GetAll(para).ConfigureAwait(false);
            return Ok(exams);
        }

        // GET api/<ExamApiController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var exam = await ExamRepo.Get(id).ConfigureAwait(false);
            return Ok(exam);
        }

        // POST api/<ExamApiController>/Export
        [HttpGet("Export")]
        public IActionResult GetAllFees()
        {
            try
            {
                var data = ExamRepo.GetAllExams();
                return Ok(data);
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        // POST api/<ExamApiController>/model
        [HttpPost]
        public async Task<IActionResult> Post(ExamModel exam)
        {
            await ExamRepo.Add(exam).ConfigureAwait(false);
            return Ok();
        }

        // PUT api/<ExamApiController>/model
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] ExamModel exam)
        {
            await ExamRepo.Update(exam).ConfigureAwait(false);
            return Ok();
        }

        // DELETE api/<ExamApiController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await ExamRepo.Delete(id).ConfigureAwait(false);
            return Ok();
        }
    }
}
