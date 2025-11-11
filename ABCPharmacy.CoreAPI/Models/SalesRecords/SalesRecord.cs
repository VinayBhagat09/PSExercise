using ABCPharmacy.CoreAPI.Models.Convertors;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ABCPharmacy.CoreAPI.Models.SalesRecord
{
    public class SalesRecord
    {

        [Key]
        public int Id { get; set; }
        [Required]
        public int MedicineId { get; set; }
        [Required]
        public int QuantitySold { get; set; }
        [JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateTime SaleDate { get; set; }
    }
}
