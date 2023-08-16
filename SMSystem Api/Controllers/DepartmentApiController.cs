using Microsoft.AspNetCore.Mvc;
using SMSystem_Api.Helpers;
using SMSystem_Api.Model.Department;
using SMSystem_Api.Model.Students;
using SMSystem_Api.Repository.Interfaces;
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
        public async Task<IActionResult> Get(string? sid,string? name, string? year,int pageIndex)
        {
            var para = new SearchingPara()
            {
                SId = sid,
                Name = name,
                Year = year,
                PageIndex = pageIndex
            };
            var departments = await DepRepo.GetAll(para).ConfigureAwait(false);
            return Ok(departments);
        }

        // GET api/<DepartmentApiController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {  
            var department = await DepRepo.Get(id).ConfigureAwait(false);
            return Ok(department);
        }

        [HttpGet("Export")]
        public IActionResult GetAllData()
        {
            var data = DepRepo.GetAllDepartments();
            return Ok(data);
        }

        // POST api/<DepartmentApiController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DepartmentModel department)
        {
            await DepRepo.Add(department).ConfigureAwait(false);
            return Ok();
        }

        // PUT api/<DepartmentApiController>/5
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] DepartmentModel department)
        {
            await DepRepo.Update(department).ConfigureAwait(false);
            return Ok();
        }

        // DELETE api/<DepartmentApiController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await DepRepo.Delete(id).ConfigureAwait(false);
            return Ok();
        }
    }
}
