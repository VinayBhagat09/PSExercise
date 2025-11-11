using ABCPharmacy.CoreAPI.Models.DTOs.Medicine;
using ABCPharmacy.CoreAPI.Models.Medicines;
using AutoMapper;
namespace ABCPharmacy.CoreAPI.MapperProfiles.Medicines
{
    public class MedicinesProfile : Profile
    {
        public MedicinesProfile()
        {
            CreateMap<CreateUpdateMedicineDTO, Medicine>(). ReverseMap();
            CreateMap<MedicineDTO, Medicine>().ReverseMap();
        }
    }
}
