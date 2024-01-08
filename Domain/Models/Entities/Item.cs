namespace Domain.Models.Entities
{
    public class Item
    {
        public int Id { get; set; }

        public string Key { get; set; }

        public string Content { get; set; }

        public int? ExpirationPeriod { get; set; }

        public DateTime ExpiresAt { get; set; }
    }
}
