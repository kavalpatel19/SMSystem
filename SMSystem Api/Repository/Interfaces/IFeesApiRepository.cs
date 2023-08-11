using SMSystem_Api.Helpers;
using SMSystem_Api.Model.Fees;
using SMSystem_Api.Model.Subjects;

namespace SMSystem_Api.Repository.Interfaces
{
    public interface IFeesApiRepository
    {
        List<FeesModel> GetAllFees();
        Task<PaggedFeesModel> GetAll(SearchingPara para);
        Task<FeesModel> Get(int id);
        Task Add(FeesModel fee);
        Task Update(FeesModel fee);
        Task Delete(int id);
    }
}
