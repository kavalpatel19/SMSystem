using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMSystem_Api.Helpers;
using SMSystem_Api.Migrations;
using SMSystem_Api.Model;
using SMSystem_Api.Model.Department;
using SMSystem_Api.Model.Exam;
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
            var baseResponse = new BaseResponseModel<PaggedFeesModel>();
            try
            {
                var para = new SearchingPara()
                {
                    PageIndex = pageIndex
                };

                baseResponse = await FeesRepo.GetAll(para).ConfigureAwait(false);

                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                baseResponse.Message = ex.Message;
                baseResponse.ResponseCode = 500;
                baseResponse.Result = new PaggedFeesModel();
                return Ok(baseResponse);
            }
        }

        // GET api/<FeesApiController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var baseResponse = new BaseResponseModel<FeesModel>();
            try
            {
                baseResponse = await FeesRepo.Get(id).ConfigureAwait(false);
                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                baseResponse.Message = ex.Message;
                baseResponse.ResponseCode = 500;
                baseResponse.Result = new FeesModel();
                return Ok(baseResponse);
            }
        }

        // POST api/<FeesApiController>/Export
        [HttpGet("Export")]
        public IActionResult GetAllFees()
        {
            var baseResponse = new BaseResponseModel<FeesModel>();
            try
            {
                baseResponse = FeesRepo.GetAllFees();
                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                baseResponse.Message = ex.Message;
                baseResponse.ResponseCode = 500;
                baseResponse.Results = new List<FeesModel>();
                return Ok(baseResponse);
            }
        }

        // POST api/<FeesApiController>/model
        [HttpPost]
        public async Task<IActionResult> Post(FeesModel fee)
        {
            var baseResponse = new BaseResponseModel<FeesModel>();
            try
            {
                baseResponse = await FeesRepo.Add(fee).ConfigureAwait(false);

                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                baseResponse.Message = ex.Message;
                baseResponse.ResponseCode = 500;
                baseResponse.Result = new FeesModel();
                return Ok(baseResponse);
            }
        }

        // PUT api/<FeesApiController>/model
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] FeesModel fee)
        {
            var baseResponse = new BaseResponseModel<FeesModel>();
            try
            {
                baseResponse = await FeesRepo.Update(fee).ConfigureAwait(false);
                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                baseResponse.Message = ex.Message;
                baseResponse.ResponseCode = 500;
                baseResponse.Result = new FeesModel();
                return Ok(baseResponse);
            }
        }

        // DELETE api/<FeesApiController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var baseResponse = new BaseResponseModel<FeesModel>();
            try
            {
                baseResponse = await FeesRepo.Delete(id).ConfigureAwait(false);

                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                baseResponse.Message = ex.Message;
                baseResponse.ResponseCode = 500;
                baseResponse.Result = new FeesModel();
                return Ok(baseResponse);
            }
        }
    }
}
