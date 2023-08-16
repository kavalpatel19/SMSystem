using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SMSystem_Api.Model
{
    public class BaseResponseModel<T> where T : class
    {
        public int? ResponseCode { get; set; }
        public string? Message { get; set; }
        public T Result { get; set; }
        public List<T> Results { get; set; }
    }
}
