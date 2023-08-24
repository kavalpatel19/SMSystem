using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using SMSystem_Api.Helpers;
using SMSystem_Api.Model;
using SMSystem_Api.Model.Department;
using SMSystem_Api.Model.Students;
using SMSystem_Api.Repository.Interfaces;
using System.ComponentModel;
using System.Net;
using System.Net.Security;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SMSystem_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentApiController : ControllerBase
    {
        private readonly IDepartmentApiRepository DepRepo;
        public DepartmentApiController(IDepartmentApiRepository depRepo)
        {
            DepRepo = depRepo;
        }

        // GET: api/<DepartmentApiController>
        [HttpGet]
        public async Task<IActionResult> Get(string? sid, string? name, string? year, int pageIndex)
        {
            var baseResponse = new BaseResponseModel<PaggedDepartmentModel>();
            try
            {
                var para = new SearchingPara()
                {
                    SId = sid,
                    Name = name,
                    Year = year,
                    PageIndex = pageIndex
                };

                 baseResponse = await DepRepo.GetAll(para).ConfigureAwait(false);

                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                baseResponse.Message = ex.Message;
                baseResponse.ResponseCode = 500;
                baseResponse.Result = new PaggedDepartmentModel();
                return Ok(baseResponse);
            }
        }

        // GET api/<DepartmentApiController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var baseResponse = new BaseResponseModel<DepartmentModel>();
            try
            {
                baseResponse = await DepRepo.Get(id).ConfigureAwait(false);
                return Ok(baseResponse);
            }
            catch(Exception ex)
            {
                baseResponse.Message = ex.Message;
                baseResponse.ResponseCode = 500;
                baseResponse.Result = new DepartmentModel();
                return Ok(baseResponse);
            }
        }

        [HttpGet("Export")]
        public IActionResult GetAllData()
        {
            var baseResponse = new BaseResponseModel<DepartmentModel>();
            try
            {
                baseResponse = DepRepo.GetAllDepartments();
                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                baseResponse.Message = ex.Message;
                baseResponse.ResponseCode = 500;
                baseResponse.Results = new List<DepartmentModel>();
                return Ok(baseResponse);
            }
        }

        // POST api/<DepartmentApiController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DepartmentModel department)
        {
            var baseResponse = new BaseResponseModel<DepartmentModel>();
            try
            {
                baseResponse = await DepRepo.Add(department).ConfigureAwait(false);
                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                baseResponse.Message = ex.Message;
                baseResponse.ResponseCode = 500;
                baseResponse.Result = new DepartmentModel();
                return Ok(baseResponse);
            }
        }

        // PUT api/<DepartmentApiController>/5
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] DepartmentModel department)
        {
            var baseResponse = new BaseResponseModel<DepartmentModel>();
            try
            {
                baseResponse = await DepRepo.Update(department).ConfigureAwait(false);
                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                baseResponse.Message = ex.Message;
                baseResponse.ResponseCode = 500;
                baseResponse.Result = new DepartmentModel();
                return Ok(baseResponse);
            }
        }

        // DELETE api/<DepartmentApiController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var baseResponse = new BaseResponseModel<DepartmentModel>();
            try
            {
                baseResponse = await DepRepo.Delete(id).ConfigureAwait(false);
                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                baseResponse.Message = ex.Message;
                baseResponse.ResponseCode = 500;
                baseResponse.Result = new DepartmentModel();
                return Ok(baseResponse);
            }
        }
    }
}
