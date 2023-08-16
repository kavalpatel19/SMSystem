using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMSystem_Api.Helpers;
using SMSystem_Api.Model.Department;
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
        public async Task<IActionResult> Get(string? sid, string? name, string? year, int pageIndex)
        {
            var para = new SearchingPara()
            {
                SId = sid,
                Name = name,
                Year = year,
                PageIndex = pageIndex
            };
            var holidays = await HoliRepo.GetAll(para).ConfigureAwait(false);
            return Ok(holidays);
        }

        // GET: api/<DepartmentApiController>/Export
        [HttpGet("Export")]
        public IActionResult GetAllData()
        {
            var data = HoliRepo.GetAllHolidays();
            return Ok(data);
        }

        // POST api/<DepartmentApiController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] HolidayModel holiday)
        {
            await HoliRepo.Add(holiday).ConfigureAwait(false);
            return Ok();
        }
    }
}
