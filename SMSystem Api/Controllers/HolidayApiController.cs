using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMSystem_Api.Helpers;
using SMSystem_Api.Model;
using SMSystem_Api.Model.Department;
using SMSystem_Api.Model.Exam;
using SMSystem_Api.Model.Holiday;
using SMSystem_Api.Repository.Interfaces;

namespace SMSystem_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HolidayApiController : ControllerBase
    {
        private readonly IHolidayApiRepository HoliRepo;

        public HolidayApiController(IHolidayApiRepository HoliRepo) 
        {
            this.HoliRepo = HoliRepo;
        }

        // GET: api/<DepartmentApiController>
        [HttpGet]
        public async Task<IActionResult> Get(int pageIndex)
        {
            var baseResponse = new BaseResponseModel<PaggedHolidayModel>();
            try
            {
                var para = new SearchingPara()
                {
                    PageIndex = pageIndex
                };

                baseResponse = await HoliRepo.GetAll(para).ConfigureAwait(false);

                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                baseResponse.Message = ex.Message;
                baseResponse.ResponseCode = 500;
                baseResponse.Result = new PaggedHolidayModel();
                return Ok(baseResponse);
            }
        }

        // GET: api/<DepartmentApiController>/Export
        [HttpGet("Export")]
        public IActionResult GetAllData()
        {
            var baseResponse = new BaseResponseModel<HolidayModel>();
            try
            {
                baseResponse = HoliRepo.GetAllHolidays();
                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                baseResponse.Message = ex.Message;
                baseResponse.ResponseCode = 500;
                baseResponse.Results = new List<HolidayModel>();
                return Ok(baseResponse);
            }
        }

        // POST api/<DepartmentApiController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] HolidayModel holiday)
        {
            var baseResponse = new BaseResponseModel<HolidayModel>();
            try
            {
                baseResponse = await HoliRepo.Add(holiday).ConfigureAwait(false);
                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                baseResponse.Message = ex.Message;
                baseResponse.ResponseCode = 500;
                baseResponse.Results = new List<HolidayModel>();
                return Ok(baseResponse);
            }
        }
    }
}
