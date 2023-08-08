namespace SMSystem_Api.Model
{
    public class PaggedModel
    {
        public int PageIndex { get; set; }
        public int TotalPage { get; set; }
        public bool PreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }

        public bool NextPage
        {
            get
            {
                return (PageIndex < TotalPage);
            }
        }
    }
}
