using SMSystem.Helpers;
using SMSystem.Models.Department;
using SMSystem.Models.Fees;

namespace SMSystem.Repository.Interfaces
{
    public interface IFeesRepository
    {
        List<FeesViewModel> GetAllFees();
        Task<FeesPaggedViewModel> GetFees(SearchingParaModel para);
        Task<FeesViewModel> GetFee(int id);
        Task<bool> Add(FeesViewModel fee);
        Task<bool> Update(FeesViewModel fee);
        Task<bool> Delete(int id);
    }
}
