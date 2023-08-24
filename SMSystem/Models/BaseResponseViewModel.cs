namespace SMSystem.Models
{
    public class BaseResponseViewModel<T> where T : class
    {
        public BaseResponseViewModel()
        {

        }
        public BaseResponseViewModel(T result)
        {
            Result = result;
        }
        public BaseResponseViewModel(List<T> results)
        {
            Results = results;
        }
        public int? ResponseCode { get; set; }
        public string? Message { get; set; }
        public T Result { get; set; }
        public List<T> Results { get; set; }
    }
}