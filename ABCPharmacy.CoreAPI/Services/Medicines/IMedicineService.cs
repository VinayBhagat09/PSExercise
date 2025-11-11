using ABCPharmacy.CoreAPI.Models.DTOs.Medicine;
using ABCPharmacy.CoreAPI.Models.Medicines;
namespace ABCPharmacy.CoreAPI.Services.Medicines
{
    public interface IMedicineService : IBaseService<Medicine,CreateUpdateMedicineDTO>
    {
    }
}
