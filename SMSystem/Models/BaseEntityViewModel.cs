namespace SMSystem.Models
{
    public class BaseEntityViewModel
    {
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDelete { get; set; }

        public BaseEntityViewModel()
        {
            CreatedBy = 0;
            CreatedDate = DateTime.Now;
            ModifiedBy = 0;
            ModifiedDate = DateTime.Now;
            IsActive = true;
            IsDelete = false;
        }
    }
}
