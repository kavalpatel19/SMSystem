using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMSystem_Api.Helpers;
using SMSystem_Api.Model;
using SMSystem_Api.Model.Department;
using SMSystem_Api.Model.Exam;
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
        public async Task<IActionResult> GetAll(string? sid, string? name, string? clas, int pageIndex)
        {
            var baseResponse = new BaseResponseModel<PaggedSubjectModel>();
            try
            {
                var para = new SearchingPara()
                {
                    SId = sid,
                    Name = name,
                    Class = clas,
                    PageIndex = pageIndex
                };

                baseResponse = await SubRepo.GetAll(para).ConfigureAwait(false);

                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                baseResponse.Message = ex.Message;
                baseResponse.ResponseCode = 500;
                baseResponse.Result = new PaggedSubjectModel();
                return Ok(baseResponse);
            }
        }

        // GET api/<SubjectApiController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var baseResponse = new BaseResponseModel<SubjectModel>();
            try
            {
                baseResponse = await SubRepo.Get(id).ConfigureAwait(false);
                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                baseResponse.Message = ex.Message;
                baseResponse.ResponseCode = 500;
                baseResponse.Result = new SubjectModel();
                return Ok(baseResponse);
            }
        }

        [HttpGet("Export")]
        public IActionResult GetAllData()
        {
            var baseResponse = new BaseResponseModel<SubjectModel>();
            try
            {
                baseResponse = SubRepo.GetAllSubjects();
                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                baseResponse.Message = ex.Message;
                baseResponse.ResponseCode = 500;
                baseResponse.Result = new SubjectModel();
                return Ok(baseResponse);
            }
        }

        // POST api/<SubjectApiController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SubjectModel subject)
        {
            var baseResponse = new BaseResponseModel<SubjectModel>();
            try
            {
                baseResponse = await SubRepo.Add(subject).ConfigureAwait(false);
                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                baseResponse.Message = ex.Message;
                baseResponse.ResponseCode = 500;
                baseResponse.Result = new SubjectModel();
                return Ok(baseResponse);
            }
        }

        // PUT api/<SubjectApiController>/5
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] SubjectModel subject)
        {
            var baseResponse = new BaseResponseModel<SubjectModel>();
            try
            {
                baseResponse = await SubRepo.Update(subject).ConfigureAwait(false);
                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                baseResponse.Message = ex.Message;
                baseResponse.ResponseCode = 500;
                baseResponse.Result = new SubjectModel();
                return Ok(baseResponse);
            }
        }

        // DELETE api/<SubjectApiController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var baseResponse = new BaseResponseModel<SubjectModel>();
            try
            {
                baseResponse = await SubRepo.Delete(id).ConfigureAwait(false);

                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                baseResponse.Message = ex.Message;
                baseResponse.ResponseCode = 500;
                baseResponse.Result = new SubjectModel();
                return Ok(baseResponse);
            }
        }
    }
}
