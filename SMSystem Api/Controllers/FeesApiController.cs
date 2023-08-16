using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMSystem_Api.Helpers;
using SMSystem_Api.Model.Department;
using SMSystem_Api.Model.Fees;
using SMSystem_Api.Model.Students;
using SMSystem_Api.Repository.Interfaces;

namespace SMSystem_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeesApiController : ControllerBase
    {
        private readonly IFeesApiRepository FeesRepo;

        public FeesApiController(IFeesApiRepository FeesRepo)
        {
            this.FeesRepo = FeesRepo;
        }

        // to Get all data searched/none searched
        [HttpGet]
        public async Task<IActionResult> GetAll(int pageIndex)
        {
            var para = new SearchingPara()
            {
                PageIndex = pageIndex
            };
            var fees = await FeesRepo.GetAll(para).ConfigureAwait(false);
            return Ok(fees);
        }

        // GET api/<FeesApiController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var fee = await FeesRepo.Get(id).ConfigureAwait(false);
            return Ok(fee);
        }

        // POST api/<FeesApiController>/Export
        [HttpGet("Export")]
        public IActionResult GetAllFees()
        {
            var data = FeesRepo.GetAllFees();
            return Ok(data);
        }

        // POST api/<FeesApiController>/model
        [HttpPost]
        public async Task<IActionResult> Post(FeesModel fee)
        {
            await FeesRepo.Add(fee).ConfigureAwait(false);
            return Ok();
        }

        // PUT api/<FeesApiController>/model
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] FeesModel fee)
        {
            await FeesRepo.Update(fee).ConfigureAwait(false);
            return Ok();
        }

        // DELETE api/<FeesApiController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await FeesRepo.Delete(id).ConfigureAwait(false);
            return Ok();
        }
    }
}
