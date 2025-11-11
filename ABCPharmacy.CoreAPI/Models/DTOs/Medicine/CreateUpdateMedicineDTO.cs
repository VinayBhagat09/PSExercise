namespace ABCPharmacy.CoreAPI.Models.DTOs.Medicine
{
    public class CreateUpdateMedicineDTO
    {
        public string? MedicineName { get; set; }
        public string? Notes { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string? Brand { get; set; }
    }
}
