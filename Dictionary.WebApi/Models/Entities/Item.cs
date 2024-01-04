namespace Dictionary.WebApi.Models.Entities
{
    public class Item
    {
        public string Key { get; set; }

        public string Content { get; set; }

        public int ExpirationPeriod { get; set; }

        public DateTime ExpirationDate { get; set; }
    }
}
