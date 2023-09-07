namespace SMSystem_Api.Model.Accounts.FeesCollection
{
    public class FeesCollectionModel:BaseEntityModel
    {
        public int Id { get; set; }
        public string StudentId { get; set; }
        public string StudentName { get; set; }
        public string Gender { get; set; }
        public string FeesType { get; set; }
        public int FeesAmount { get; set; }
        public DateTime PaidDate { get; set; }
    }
}
