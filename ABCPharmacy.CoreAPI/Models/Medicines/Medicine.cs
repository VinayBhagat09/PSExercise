using ABCPharmacy.CoreAPI.Models.Convertors;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ABCPharmacy.CoreAPI.Models.Medicines
{
    public class Medicine
    {
        [Key]
        public int Id { get; set; }
        public string? MedicineName { get; set; }
        public string? Notes { get; set; }
        [JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateTime ExpiryDate { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string? Brand { get; set; }
    }
}
