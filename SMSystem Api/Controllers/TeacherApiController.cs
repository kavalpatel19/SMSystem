using Microsoft.AspNetCore.Mvc;
using SMSystem_Api.Helpers;
using SMSystem_Api.Model.Exam;
using SMSystem_Api.Model;
using SMSystem_Api.Model.Students;
using SMSystem_Api.Model.Teachers;
using SMSystem_Api.Repository.Interfaces;
using DocumentFormat.OpenXml.Office2010.Excel;

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
            var baseResponse = new BaseResponseModel<PaggedTeacherModel>();
            try
            {
                var para = new SearchingPara()
                {
                    SId = sid,
                    Name = name,
                    Phone = phone,
                    PageIndex = pageIndex
                };

                baseResponse = await teachRepo.GetAll(para).ConfigureAwait(false);

                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                baseResponse.Message = ex.Message;
                baseResponse.ResponseCode = 500;
                baseResponse.Result = new PaggedTeacherModel();
                return Ok(baseResponse);
            }
        }

        // GET api/<TeacherApiController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var baseResponse = new BaseResponseModel<TeacherModel>();
            try
            {
                baseResponse = await teachRepo.Get(id).ConfigureAwait(false);
                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                baseResponse.Message = ex.Message;
                baseResponse.ResponseCode = 500;
                baseResponse.Result = new TeacherModel();
                return Ok(baseResponse);
            }
        }

        [HttpGet("Export")]
        public IActionResult GetAllTeachers()
        {
            var baseResponse = new BaseResponseModel<TeacherModel>();
            try
            {
                baseResponse = teachRepo.GetAllTeachers();
                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                baseResponse.Message = ex.Message;
                baseResponse.ResponseCode = 500;
                baseResponse.Result = new TeacherModel();
                return Ok(baseResponse);
            }
        }

        // POST api/<TeacherApiController>
        [HttpPost]
        public async Task<IActionResult> Post(TeacherModel teacher)
        {
            var baseResponse = new BaseResponseModel<TeacherModel>();
            try
            {
                baseResponse = await teachRepo.Add(teacher).ConfigureAwait(false);
                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                baseResponse.Message = ex.Message;
                baseResponse.ResponseCode = 500;
                baseResponse.Result = new TeacherModel();
                return Ok(baseResponse);
            }
        }

        // PUT api/<TeacherApiController>/5
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] TeacherModel teacher)
        {
            var baseResponse = new BaseResponseModel<TeacherModel>();
            try
            {
                baseResponse = await teachRepo.Update(teacher).ConfigureAwait(false);
                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                baseResponse.Message = ex.Message;
                baseResponse.ResponseCode = 500;
                baseResponse.Result = new TeacherModel();
                return Ok(baseResponse);
            }
        }

        // DELETE api/<TeacherApiController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var baseResponse = new BaseResponseModel<TeacherModel>();
            try
            {
                baseResponse = await teachRepo.Delete(id).ConfigureAwait(false);
                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                baseResponse.Message = ex.Message;
                baseResponse.ResponseCode = 500;
                baseResponse.Result = new TeacherModel();
                return Ok(baseResponse);
            }
        }
    }
}
