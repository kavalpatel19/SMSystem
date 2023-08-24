namespace SMSystem_Api.Helpers
{
    public class SearchingPara
    {
        public int pagesize 
        {
            get
            {
                return 5;
            }
        }
        public string? SId { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Year { get; set; }
        public string? Class { get; set; }
        public int PageIndex { get; set; }
    }
}
