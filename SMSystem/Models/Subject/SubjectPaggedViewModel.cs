namespace SMSystem.Models.Subject
{
    public class SubjectPaggedViewModel
    {
        public IList<SubjectViewModel> SubjectModel { get; set; }
        public PaggedViewModel PaggedModel { get; set; }
    }
}
