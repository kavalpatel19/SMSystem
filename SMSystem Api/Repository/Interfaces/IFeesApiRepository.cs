using SMSystem_Api.Helpers;
using SMSystem_Api.Model.Exam;
using SMSystem_Api.Model;
using SMSystem_Api.Model.Fees;
using SMSystem_Api.Model.Subjects;

namespace SMSystem_Api.Repository.Interfaces
{
    public interface IFeesApiRepository
    {
        BaseResponseModel<FeesModel> GetAllFees();
        Task<BaseResponseModel<PaggedFeesModel>> GetAll(SearchingPara para);
        Task<BaseResponseModel<FeesModel>> Get(int id);
        Task<BaseResponseModel<FeesModel>> Add(FeesModel fee);
        Task<BaseResponseModel<FeesModel>> Update(FeesModel fee);
        Task<BaseResponseModel<FeesModel>> Delete(int id);
    }
}
