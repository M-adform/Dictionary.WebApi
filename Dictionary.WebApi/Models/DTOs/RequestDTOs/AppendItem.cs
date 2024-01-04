namespace Dictionary.WebApi.Models.DTOs.RequestDTOs
{
    public class AppendItem
    {
        public string ?Key { get; set; }
        public string ?ContentToAppend { get; set; }
        public int? ExpirationPeriod { get; set; }
    }
}
