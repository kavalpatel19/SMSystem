using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SMSystem_Api.Data;
using SMSystem_Api.Helpers;
using SMSystem_Api.Model.Exam;
using SMSystem_Api.Model;
using SMSystem_Api.Model.Students;
using SMSystem_Api.Repository.Interfaces;
using System.Data.Common;
using DocumentFormat.OpenXml.Office2010.Excel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SMSystem_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentApiController : ControllerBase
    {
        private readonly IStudentApiRepository StudentRepo;

        public StudentApiController(IStudentApiRepository repo) 
        {
            StudentRepo = repo;
        }

        // to Get all data searched/none searched
        [HttpGet]
        public async Task<IActionResult> GetAll(string? sid ,string? name , string? phone , int pageIndex)
        {
            var baseResponse = new BaseResponseModel<PaggedStudentModel>();
            try
            {
                var para = new SearchingPara()
                {
                    SId = sid,
                    Name = name,
                    Phone = phone,
                    PageIndex = pageIndex
                };

                baseResponse = await StudentRepo.GetAll(para).ConfigureAwait(false);

                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                baseResponse.Message = ex.Message;
                baseResponse.ResponseCode = 500;
                baseResponse.Result = new PaggedStudentModel();
                return Ok(baseResponse);
            }
        }

        // GET api/<StudentApiController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var baseResponse = new BaseResponseModel<StudentModel>();
            try
            {
                baseResponse = await StudentRepo.Get(id).ConfigureAwait(false);
                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                baseResponse.Message = ex.Message;
                baseResponse.ResponseCode = 500;
                baseResponse.Result = new StudentModel();
                return Ok(baseResponse);
            }
        }

        [HttpGet("Export")]
        public IActionResult GetAllStudents()
        {
            var baseResponse = new BaseResponseModel<StudentModel>();
            try
            {
                baseResponse = StudentRepo.GetAllStudents();
                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                baseResponse.Message = ex.Message;
                baseResponse.ResponseCode = 500;
                baseResponse.Result = new StudentModel();
                return Ok(baseResponse);
            }
        }

        // POST api/<StudentApiController>
        [HttpPost]
        public async Task<IActionResult> Post(StudentModel student)
        {
            var baseResponse = new BaseResponseModel<StudentModel>();
            try
            {
                baseResponse = await StudentRepo.Add(student).ConfigureAwait(false);
                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                baseResponse.Message = ex.Message;
                baseResponse.ResponseCode = 500;
                baseResponse.Result = new StudentModel();
                return Ok(baseResponse);
            }
        }

        // PUT api/<StudentApiController>/5
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] StudentModel student)
        {
            var baseResponse = new BaseResponseModel<StudentModel>();
            try
            {
                baseResponse = await StudentRepo.Update(student).ConfigureAwait(false);

                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                baseResponse.Message = ex.Message;
                baseResponse.ResponseCode = 500;
                baseResponse.Result = new StudentModel();
                return Ok(baseResponse);
            }
        }

        // DELETE api/<StudentApiController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var baseResponse = new BaseResponseModel<StudentModel>();
            try
            {
                baseResponse = await StudentRepo.Delete(id).ConfigureAwait(false);

                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                baseResponse.Message = ex.Message;
                baseResponse.ResponseCode = 500;
                baseResponse.Result = new StudentModel();
                return Ok(baseResponse);
            }
        }
    }
}
