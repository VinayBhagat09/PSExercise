namespace ABCPharmacy.CoreAPI.Models.DTOs.SalesRecord
{
    public class CreateUpdateSalesRecordDTO
    {
        public int MedicineId { get; set; }
        public int QuantitySold { get; set; }
        public DateTime SaleDate { get; set; }
    }
}
