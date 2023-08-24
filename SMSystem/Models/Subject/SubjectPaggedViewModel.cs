using SMSystem.Models.Department;

namespace SMSystem.Models.Subject
{
    public class SubjectPaggedViewModel
    {
        public SubjectPaggedViewModel()
        {
            SubjectModel = new List<SubjectViewModel>();
            PaggedModel = new PaggedViewModel();
        }
        public IList<SubjectViewModel> SubjectModel { get; set; }
        public PaggedViewModel PaggedModel { get; set; }
    }
}
