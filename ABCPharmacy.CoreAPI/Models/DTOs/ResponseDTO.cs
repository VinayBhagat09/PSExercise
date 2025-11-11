namespace ABCPharmacy.CoreAPI.Models.DTOs
{
    public class ResponseDTO
    {
        public object? result { get; set; }
        public bool isSucess { get; set; } = true;
        public string? meessage { get; set; }
    }
}
