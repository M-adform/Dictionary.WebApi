namespace Dictionary.WebApi.Models.Entities
{
    public class Item
    {
        public string Key { get; set; }

        public List<object> Content { get; set; }

        public DateTime ExpirationDate { get; set; }
    }
}
