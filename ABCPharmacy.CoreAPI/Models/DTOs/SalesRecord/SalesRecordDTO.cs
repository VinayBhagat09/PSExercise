using ABCPharmacy.CoreAPI.Models.Convertors;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ABCPharmacy.CoreAPI.Models.DTOs.SalesRecord
{
    public class SalesRecordDTO
    {
        public int Id { get; set; }
        public int MedicineId { get; set; }
        public int QuantitySold { get; set; }
        [JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateTime SaleDate { get; set; }
    }
}
