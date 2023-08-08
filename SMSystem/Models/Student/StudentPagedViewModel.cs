namespace SMSystem.Models.Students
{
    public class StudentPagedViewModel
    {
        public List<StudentViewModel> StudentModel { get; set; }
        public PaggedViewModel PaggedModel { get; set; }
    }
}