using Microsoft.AspNetCore.Mvc;
using SMSystem_Api.Helpers;
using SMSystem_Api.Model.Department;
using SMSystem_Api.Model.Students;
using SMSystem_Api.Repository;
using System.Net.Security;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SMSystem_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentApiController : ControllerBase
    {
        IDepartmentApiRepository DepRepo;
        public DepartmentApiController(IDepartmentApiRepository depRepo)
        {
            DepRepo = depRepo;
        }
        // GET: api/<DepartmentApiController>
        [HttpGet]
        public async Task<IActionResult> Get(string? sid,string? name, string? year,int pageIndex)
        {
            SearchingPara para = new SearchingPara()
            {
                SId = sid,
                Name = name,
                Year = year,
                PageIndex = pageIndex
            };
            PaggedDepartmentModel departments = await DepRepo.GetAll(para);
            return Ok(departments);
        }

        // GET api/<DepartmentApiController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {  
            DepartmentModel department = await DepRepo.Get(id);
            return Ok(department);
        }

        [HttpGet("Export")]
        public IActionResult GetAllData()
        {
            List<DepartmentModel> data = DepRepo.GetAllDepartments();
            return Ok(data);
        }

        // POST api/<DepartmentApiController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DepartmentModel department)
        {
            DepRepo.Add(department);
            return Ok();
        }

        // PUT api/<DepartmentApiController>/5
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] DepartmentModel department)
        {
            DepRepo.Update(department);
            return Ok();
        }

        // DELETE api/<DepartmentApiController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            DepRepo.Delete(id);
            return Ok();
        }
    }
}
