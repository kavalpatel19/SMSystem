using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMSystem_Api.Helpers;
using SMSystem_Api.Migrations;
using SMSystem_Api.Model;
using SMSystem_Api.Model.Department;
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
            var baseResponse = new BaseResponseModel<PaggedExamModel>();
            try
            {
                var para = new SearchingPara()
                {
                    PageIndex = pageIndex
                };

                baseResponse = await ExamRepo.GetAll(para).ConfigureAwait(false);

                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                baseResponse.Message = ex.Message;
                baseResponse.ResponseCode = 500;
                baseResponse.Result = new PaggedExamModel();
                return Ok(baseResponse);
            }
        }

        // GET api/<ExamApiController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var baseResponse = new BaseResponseModel<ExamModel>();
            try
            {
                baseResponse = await ExamRepo.Get(id).ConfigureAwait(false);
                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                baseResponse.Message = ex.Message;
                baseResponse.ResponseCode = 500;
                baseResponse.Result = new ExamModel();
                return Ok(baseResponse);
            }
        }

        // POST api/<ExamApiController>/Export
        [HttpGet("Export")]
        public IActionResult GetAllExams()
        {
            var baseResponse = new BaseResponseModel<ExamModel>();
            try
            {
                baseResponse = ExamRepo.GetAllExams(); ;
                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                baseResponse.Message = ex.Message;
                baseResponse.ResponseCode = 500;
                baseResponse.Results = new List<ExamModel>();
                return Ok(baseResponse);
            }            
        }

        // POST api/<ExamApiController>/model
        [HttpPost]
        public async Task<IActionResult> Post(ExamModel exam)
        {
            var baseResponse = new BaseResponseModel<ExamModel>();
            try
            {
                baseResponse = await ExamRepo.Add(exam).ConfigureAwait(false);
                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                baseResponse.Message = ex.Message;
                baseResponse.ResponseCode = 500;
                baseResponse.Result = new ExamModel();
                return Ok(baseResponse);
            }
        }

        // PUT api/<ExamApiController>/model
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] ExamModel exam)
        {
            var baseResponse = new BaseResponseModel<ExamModel>();
            try
            {
                baseResponse = await ExamRepo.Update(exam).ConfigureAwait(false);
                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                baseResponse.Message = ex.Message;
                baseResponse.ResponseCode = 500;
                baseResponse.Result = new ExamModel();
                return Ok(baseResponse);
            }
        }

        // DELETE api/<ExamApiController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var baseResponse = new BaseResponseModel<ExamModel>();
            try
            {
                baseResponse = await ExamRepo.Delete(id).ConfigureAwait(false);
                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                baseResponse.Message = ex.Message;
                baseResponse.ResponseCode = 500;
                baseResponse.Result = new ExamModel();
                return Ok(baseResponse);
            }
        }
    }
}
