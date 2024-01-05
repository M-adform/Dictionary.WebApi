namespace Dictionary.WebApi.Models.DTOs.RequestDTOs
{
    public class AppendItem
    {
        public string? Key { get; set; }
        public string? ContentToAppend { get; set; }

        [System.ComponentModel.DefaultValue(null)] 
        public int? ExpirationPeriod { get; set; }
    }
}
