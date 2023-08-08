namespace SMSystem.Models
{
    public class PaggedViewModel
    {
        public int PageIndex { get; set; }
        public int TotalPage { get; set; }
        public bool PreviousPage { get; set; }
        public bool NextPage { get; set; }
    }
}
