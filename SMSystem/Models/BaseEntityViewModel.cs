namespace SMSystem.Models
{
    public class BaseEntityViewModel
    {
        public BaseEntityViewModel()
        {
            CreatedBy = null;
            CreatedDate = DateTime.Now;
            ModifiedBy = null;
            ModifiedDate = DateTime.Now;
            IsActive = true;
            IsDelete = false;
        }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDelete { get; set; }
    }
}
