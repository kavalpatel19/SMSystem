using SMSystem.Helpers;
using SMSystem.Models;
using SMSystem.Models.Department;
using SMSystem.Models.Exam;
using SMSystem.Models.Fees;

namespace SMSystem.Repository.Interfaces
{
    public interface IFeesRepository
    {
        BaseResponseViewModel<FeesViewModel> GetAllFees();
        Task<BaseResponseViewModel<FeesPaggedViewModel>> GetFees(SearchingParaModel para);
        Task<BaseResponseViewModel<FeesViewModel>> GetFee(int id);
        Task<BaseResponseViewModel<FeesViewModel>> Add(FeesViewModel fee);
        Task<BaseResponseViewModel<FeesViewModel>> Update(FeesViewModel fee);
        Task<BaseResponseViewModel<FeesViewModel>> Delete(int id);
    }
}
